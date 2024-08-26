using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    internal class MeshElasticLogic : IMeshPrimitiveLogic
    {
        private IMeshPrimitiveLogic regularMeshLogic;
        public INdmPrimitive Primitive { get; set; }
        public ITriangulationOptions TriangulationOptions { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public List<INdm> MeshPrimitive()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            CheckInputData();
            List<INdm> ndms = new();
            regularMeshLogic = new MeshPrimitiveLogic()
            {
                Primitive = Primitive,
                TriangulationOptions = TriangulationOptions,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            ndms.AddRange(regularMeshLogic.MeshPrimitive());
            foreach (var ndm in ndms)
            {
                var material = ndm.Material;
                var materialFunc = material.Diagram;
                var newMaterialFunc = (IEnumerable<double> parameters, double strain) => strain * material.InitModulus;
                var existingPrestrain = ndm.PrestrainLogic.GetAll().Sum(x => x.PrestrainValue);
                var newPrestrain = materialFunc(null, existingPrestrain) / material.InitModulus;
                ndm.Material.Diagram = newMaterialFunc;
                ndm.PrestrainLogic.DeleteAll();
                ndm.PrestrainLogic.Add(PrestrainTypes.Prestrain, newPrestrain);
            }
            return ndms;
        }

        private void CheckInputData()
        {
            if (Primitive is null)
            {
                string errorMessage = string.Intern(ErrorStrings.ParameterIsNull);
                TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
            if (TriangulationOptions.LimiteState is not LimitStates.SLS)
            {
                string errorMessage = string.Intern(ErrorStrings.DataIsInCorrect + $": Limit state for cracking must correspondent limit state of serviceability");
                TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
            if (TriangulationOptions.CalcTerm is not CalcTerms.ShortTerm)
            {
                string errorMessage = string.Intern(ErrorStrings.DataIsInCorrect + $": Calc term for cracked concrete must correspondent short term");
                TraceLogger?.AddMessage(errorMessage, TraceLogStatuses.Error);
                throw new StructureHelperException(errorMessage);
            }
            TraceLogger?.AddMessage($"Primitive check is ok");
        }
    }
}
