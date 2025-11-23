using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class CurvatureTermUncrackedCalculator : ICurvatureTermCalculator
    {
        private CurvatureTermCalculatorResult result;
        private List<INdm> ndms;

        public ICurvatureTermCalculatorInputData InputData { get; set; }

        public IResult Result => result;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public Guid Id => throw new NotImplementedException();

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Run()
        {
            PrepareNewResult();
            TriangulatePrimitives();
            Calculate();
        }

        private void Calculate()
        {
            try
            {
                var inputData = new ForceTupleInputData()
                {
                    NdmCollection = ndms,
                    ForceTuple = InputData.ForceTuple
                };
                var calculator = new ForceTupleCalculator()
                {
                    InputData = inputData,
                };
                calculator.Run();
                var calcResult = calculator.Result as IForceTupleCalculatorResult;
                if (calcResult.IsValid != true)
                {
                    result.IsValid = false;
                    result.Description += calcResult.Description;
                    return;
                }
                result.CurvatureValues = ForceTupleConverter.ConvertToForceTuple(calcResult.LoaderResults.StrainMatrix);
                ConvertCurvaturesToDeflections();
            }
            catch (Exception ex)
            {
                result.IsValid = false;
                result.Description = ex.Message;
            }
        }

        private void ConvertCurvaturesToDeflections()
        {
            ForceTuple deflections = new();
            double spanLength = InputData.DeflectionFactor.SpanLength;
            deflections.Mx = InputData.DeflectionFactor.DeflectionFactors.Mx * result.CurvatureValues.Mx * spanLength * spanLength;
            deflections.My = InputData.DeflectionFactor.DeflectionFactors.My * result.CurvatureValues.My * spanLength * spanLength;
            deflections.Nz = InputData.DeflectionFactor.DeflectionFactors.Nz * result.CurvatureValues.Nz * spanLength;
            result.Deflections = deflections;
        }

        private void TriangulatePrimitives()
        {
            var triangulateLogic = new TriangulatePrimitiveLogic()
            {
                Primitives = InputData.Primitives,
                LimitState = LimitStates.SLS,
                CalcTerm = InputData.CalculationTerm,
                TraceLogger = TraceLogger
            };
            ndms = triangulateLogic.GetNdms();
        }

        private void PrepareNewResult()
        {
            result = new();
        }
    }
}
