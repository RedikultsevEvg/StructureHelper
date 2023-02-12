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
        public string Name { get; set; }
        public INdmResult Result { get; private set; }

        private IForceTupleInputData inputData;

        public ForceTupleCalculator(IForceTupleInputData inputData)
        {
            this.inputData = inputData;
        }

        public void Run()
        {
            Result = CalculateResult();
        }

        private IForcesTupleResult CalculateResult()
        {
            var ndmCollection = inputData.NdmCollection;
            var tuple = inputData.Tuple;
            var accuracy = inputData.Accuracy;


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
                    return new ForcesTupleResult() { IsValid = true, Desctription = "Analysis is done succsefully", LoaderResults = calcResult };
                }
                else
                {
                    return new ForcesTupleResult() { IsValid = false, Desctription = "Required accuracy rate has not achived", LoaderResults = calcResult };
                }

            }
            catch (Exception ex)
            {
                var result = new ForcesTupleResult() { IsValid = false };
                if (ex.Message == "Calculation result is not valid: stiffness matrix is equal to zero") { result.Desctription = "Stiffness matrix is equal to zero \nProbably section was collapsed"; }
                else { result.Desctription = $"Error is appeared due to analysis. Error: {ex}"; }
                return result;
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
