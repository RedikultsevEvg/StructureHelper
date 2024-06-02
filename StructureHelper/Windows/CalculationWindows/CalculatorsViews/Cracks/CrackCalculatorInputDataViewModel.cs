using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public class CrackCalculatorInputDataViewModel : OkCancelViewModelBase
    {
        private CrackCalculator calculator;
        CrackInputData crackInputData;
        private bool setUserValueSofteningFactor;
        private double softeningFactor;
        private string name;

        public SourceTargetVM<IForceAction> CombinationViewModel { get; }
        public SourceTargetVM<PrimitiveBase> PrimitivesViewModel { get; private set; }
        public string WindowTitle => "Crack calculator: " + Name;
        public string Name
        {
            get => calculator.Name;
            set
            {
                calculator.Name = value;
                OnPropertyChanged(nameof(Name));
                OnPropertyChanged(nameof(WindowTitle));
            }
        }
        public bool SetSofteningFactor
        {
            get => crackInputData.UserCrackInputData.SetSofteningFactor;
            set
            {
                crackInputData.UserCrackInputData.SetSofteningFactor = value;
                OnPropertyChanged(nameof(SetSofteningFactor));
            }
        }
        public double SofteningFactor
        {
            get => crackInputData.UserCrackInputData.SofteningFactor; set
            {
                if (value > 1d || value < 0d) { return; }
                crackInputData.UserCrackInputData.SofteningFactor = value;
                OnPropertyChanged(nameof(SetSofteningFactor));
            }
        }
        public bool SetLengthBetweenCracks
        {
            get => crackInputData.UserCrackInputData.SetLengthBetweenCracks;
            set
            {
                crackInputData.UserCrackInputData.SetLengthBetweenCracks = value;
                OnPropertyChanged(nameof(SetLengthBetweenCracks));
            }
        }
        public double LengthBetweenCracks
        {
            get => crackInputData.UserCrackInputData.LengthBetweenCracks; set
            {
                if (value <= 0d) { return; }
                crackInputData.UserCrackInputData.LengthBetweenCracks = value;
                OnPropertyChanged(nameof(SetLengthBetweenCracks));
            }
        }

        public double UltLongTermCrackWidth
        {
            get => crackInputData.UserCrackInputData.UltimateLongCrackWidth; set
            {
                if (value <= 0d) { return; }
                crackInputData.UserCrackInputData.UltimateLongCrackWidth = value;
                OnPropertyChanged(nameof(UltLongTermCrackWidth));
            }
        }
        public double UltShortTermCrackWidth
        {
            get => crackInputData.UserCrackInputData.UltimateShortCrackWidth; set
            {
                if (value <= 0d) { return; }
                crackInputData.UserCrackInputData.UltimateShortCrackWidth = value;
                OnPropertyChanged(nameof(UltShortTermCrackWidth));
            }
        }

        public CrackCalculatorInputDataViewModel(IEnumerable<INdmPrimitive> allowedPrimitives, IEnumerable<IForceAction> allowedCombinations, CrackCalculator crackCalculator)
        {
            calculator = crackCalculator;
            crackInputData = calculator.InputData;
            CombinationViewModel = SourceTargetFactory.GetSourceTargetVM(allowedCombinations, crackInputData.ForceActions);
            PrimitivesViewModel = SourceTargetFactory.GetSourceTargetVM(allowedPrimitives, crackInputData.Primitives);
        }

        public void Refresh()
        {
            var combinations = CombinationViewModel.GetTargetItems();
            crackInputData.ForceActions.Clear();
            foreach (var item in combinations)
            {
                crackInputData.ForceActions.Add(item);
            }
            crackInputData.Primitives.Clear();
            foreach (var item in PrimitivesViewModel.GetTargetItems())
            {
                crackInputData.Primitives.Add(item.GetNdmPrimitive());
            }
        }
    }
}
