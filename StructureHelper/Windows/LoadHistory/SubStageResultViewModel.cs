using StructureHelper.Infrastructure;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace StructureHelper.Windows.LoadHistory
{
    public class SubStageResultViewModel : ViewModelBase
    {
        private IMathRoundLogic mathRoundLogic = new FixedRoundLogic() { DigitQuant = 3};
        public ISubStageResult Result { get; }

        public string StageName => Result.ConstructionStage.Name;
        public bool IsValid => Result.IsValid;
        public string Position => Result.StagePosition.ToString();
        public string Description => Result.Description;

        public int TotalNumber => Result.TotalNumber;
        public int LocalNumber => Result.LocalNumber;
        public List<INdmPrimitive> Primitives => Result.Primitives;
        public int PrimitivesCount => Result.Primitives.Count;
        public string ForceTuple => $"Nz = {Result.ForceTuple.Nz}; Mx = {Result.ForceTuple.Mx}, My = {Result.ForceTuple.My}";
        public double Nz => mathRoundLogic.RoundValue(Result.ForceTuple.Nz);
        public double Mx => mathRoundLogic.RoundValue(Result.ForceTuple.Mx);
        public double My => mathRoundLogic.RoundValue(Result.ForceTuple.My);

        public double TotalTime => mathRoundLogic.RoundValue(Result.TotalTime);
        public double LocalTime => mathRoundLogic.RoundValue(Result.LocalTime);

        public SubStageResultViewModel(ISubStageResult result)
        {
            Result = result;
        }
    }
}
