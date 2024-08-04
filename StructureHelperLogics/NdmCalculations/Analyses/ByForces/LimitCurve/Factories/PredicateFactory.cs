using LoaderCalculator;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Cracking;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public enum PredicateTypes
    {
        Strength,
        Cracking
    }
    public class PredicateFactory : ILogic
    {
        private ForceTupleCalculator calculator;
        private ForceTuple tuple;
        private ForceTupleInputData inputData;
        private IShiftTraceLogger logger;

        public IEnumerable<INdm> Ndms { get; set; }
        public IConvert2DPointTo3DPointLogic ConvertLogic { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public PredicateFactory()
        {
            inputData = new();
            calculator = new() { InputData = inputData };                  
        }
        public Predicate<IPoint2D> GetPredicate(PredicateTypes predicateType)
        {
            if (TraceLogger is not null)
            {
                logger = new ShiftTraceLogger()
                {
                    ShiftPriority = 500,
                    KeepErrorStatus = false
                };
                //calculator.TraceLogger = logger; // too much results
                //ConvertLogic.TraceLogger = logger; //wrong work in different threads
            }
            if (predicateType == PredicateTypes.Strength)
            {
                return point2D => IsSectionFailure(point2D);
            }
            else if (predicateType == PredicateTypes.Cracking)
            {
                return point2D => IsSectionCracked(point2D);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(predicateType));
            }
        }

        private bool IsSectionFailure(IPoint2D point2D)
        {
            logger?.TraceLoggerEntries.Clear();
            var point3D = ConvertLogic.GetPoint3D(point2D);
            tuple = new()
            {
                Nz = point3D.Z,
                Mx = point3D.X,
                My = point3D.Y
            };
            inputData.Tuple = tuple;
            inputData.NdmCollection = Ndms;
            calculator.Run();
            if (logger is not null)
            {
                TraceLogger?.TraceLoggerEntries.AddRange(logger.TraceLoggerEntries);
            }
            var result = calculator.Result;
            return !result.IsValid;
        }

        private bool IsSectionCracked(IPoint2D point2D)
        {
            logger?.TraceLoggerEntries.Clear();
            var logic = new IsSectionCrackedByForceLogic();
            var point3D = ConvertLogic.GetPoint3D(point2D);
            tuple = new()
            {
                Nz = point3D.Z,
                Mx = point3D.X,
                My = point3D.Y
            };
            logic.Tuple = tuple;
            logic.SectionNdmCollection = Ndms;
            try
            {
                if (logger is not null)
                {
                    TraceLogger?.TraceLoggerEntries.AddRange(logger.TraceLoggerEntries);
                }
                var result = logic.IsSectionCracked();
                return result;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
