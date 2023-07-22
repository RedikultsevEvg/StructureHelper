using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.SourceData;
using LoaderCalculator;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceTupleCalculator : IForceTupleCalculator
    {
        public IForceTupleInputData InputData { get; set; }
        public string Name { get; set; }
        public IResult Result { get; private set; }

        public ForceTupleCalculator(IForceTupleInputData inputData)
        {
            InputData = inputData;
        }
        public ForceTupleCalculator()
        {
            
        }
        public void Run()
        {
            Result = CalculateResult();
        }

        private IForcesTupleResult CalculateResult()
        {
            var ndmCollection = InputData.NdmCollection;
            var tuple = InputData.Tuple;
            var accuracy = InputData.Accuracy;


            var mx = tuple.Mx;
            var my = tuple.My;
            var nz = tuple.Nz;

            try
            {
                var loaderData = new LoaderOptions
                {
                    Preconditions = new Preconditions
                    {
                        ConditionRate = accuracy.IterationAccuracy,
                        MaxIterationCount = accuracy.MaxIterationCount,
                        StartForceMatrix = new ForceMatrix { Mx = mx, My = my, Nz = nz }
                    },
                    NdmCollection = ndmCollection
                };
                var calculator = new Calculator();
                calculator.Run(loaderData, new CancellationToken());
                var calcResult = calculator.Result;
                if (calcResult.AccuracyRate <= accuracy.IterationAccuracy)
                {
                    return new ForcesTupleResult() { IsValid = true, Description = "Analysis is done succsefully", LoaderResults = calcResult };
                }
                else
                {
                    return new ForcesTupleResult() { IsValid = false, Description = "Required accuracy rate has not achived", LoaderResults = calcResult };
                }

            }
            catch (Exception ex)
            {
                var result = new ForcesTupleResult() { IsValid = false };
                if (ex.Message == "Calculation result is not valid: stiffness matrix is equal to zero") { result.Description = "Stiffness matrix is equal to zero \nProbably section was collapsed"; }
                else { result.Description = $"Error is appeared due to analysis. Error: {ex}"; }
                return result;
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
