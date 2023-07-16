using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class CrackForceCalculator : ICalculator
    {
        static readonly CrackedLogic crackedLogic = new();
        private CrackForceResult result;

        public string Name { get; set; }
        public IForceTuple StartTuple { get; set; }
        public IForceTuple EndTuple { get; set; }
        public IEnumerable<INdm> NdmCollection { get; set; }
        public Accuracy Accuracy {get;set; }
        public IResult Result => result;
        public CrackForceCalculator()
        {
            StartTuple ??= new ForceTuple();
            Accuracy ??= new Accuracy() { IterationAccuracy = 0.0001d, MaxIterationCount = 10000 };
        }

        public void Run()
        {
            result = new CrackForceResult();
            crackedLogic.StartTuple = StartTuple;
            crackedLogic.EndTuple = EndTuple;
            crackedLogic.NdmCollection = NdmCollection;
            try
            {
                Check();
            }
            catch(Exception ex)
            {
                result.IsValid = false;
                result.Description += ex;
                return;
            }
            if (crackedLogic.IsSectionCracked(0d) == true)
            {
                result.IsValid = true;
                result.ActualFactor = 0d;
                result.ActualTuple = (IForceTuple)StartTuple.Clone();
                result.IsSectionCracked = true;
                result.Description += "Section cracked in start tuple";
                return;
            }
            if (crackedLogic.IsSectionCracked(1d) == false)
            {
                result.IsValid = true;
                result.IsSectionCracked = false;
                result.Description = "Section is not cracked";
                return;
            }
            var parameterCalculator = new FindParameterCalculator()
            {
                Accuracy = Accuracy,
                Predicate = crackedLogic.IsSectionCracked
            };
            parameterCalculator.Run();
            var paramResult = parameterCalculator.Result as FindParameterResult;
            if (paramResult.IsValid == true)
            {
                result.IsValid = true;
                result.IsSectionCracked = true;
                result.Description += paramResult.Description;
                result.ActualFactor = paramResult.Parameter;
                result.ActualTuple = ForceTupleService.InterpolateTuples(EndTuple, StartTuple, paramResult.Parameter);
            }
            else
            {
                result.IsValid = false;
                result.Description += paramResult.Description;
            }
        }

        private void Check()
        {
            CheckObject.IsNull(EndTuple);
            if (StartTuple == EndTuple)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + ": Section is not cracked");
            }
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
