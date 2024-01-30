using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
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
    public class PredicateFactory
    {
        private ForceTupleCalculator calculator;
        private ForceTuple tuple;
        private ForceTupleInputData inputData;
        public IEnumerable<INdm> Ndms { get; set; }
        public IConvert2DPointTo3DPointLogic ConvertLogic { get; set; }
        public PredicateFactory()
        {
            inputData = new();
            calculator = new() { InputData = inputData };                  
        }
        public Predicate<IPoint2D> GetPredicate(PredicateTypes predicateType)
        {
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
            var result = calculator.Result;
            return !result.IsValid;
        }

        private bool IsSectionCracked(IPoint2D point2D)
        {
            var logic = new HoleSectionCrackedLogic();
            var point3D = ConvertLogic.GetPoint3D(point2D);
            tuple = new()
            {
                Nz = point3D.Z,
                Mx = point3D.X,
                My = point3D.Y
            };
            logic.Tuple = tuple;
            logic.NdmCollection = Ndms;
            try
            {
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
