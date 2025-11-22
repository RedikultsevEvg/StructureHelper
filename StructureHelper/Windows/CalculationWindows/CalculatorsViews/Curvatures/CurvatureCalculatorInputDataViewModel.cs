using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.Curvatures
{
    public class CurvatureCalculatorInputDataViewModel : ViewModelBase
    {
        private ICurvatureCalculatorInputData inputData;
        private double deflectionFactor;
        private double spanLength;

        public double DeflectionFactor
        {
            get => inputData.DeflectionFactor;
            set
            {
                inputData.DeflectionFactor = Math.Max(value, 0.0);
                OnPropertyChanged(nameof(DeflectionFactor));
            }
        }

        public double SpanLength
        {
            get => inputData.SpanLength;
            set
            {
                inputData.SpanLength = Math.Max(value, 0.0);
                OnPropertyChanged(nameof(SpanLength));
            }
        }

        public SourceTargetVM<IForceAction> CombinationViewModel { get; }
        public SourceTargetVM<PrimitiveBase> PrimitivesViewModel { get; }

        public CurvatureCalculatorInputDataViewModel(ICurvatureCalculatorInputData inputData, ICrossSectionRepository repository)
        {
            this.inputData = inputData;
            CombinationViewModel = SourceTargetFactory.GetSourceTargetVM(repository.ForceActions, inputData.ForceActions);
            PrimitivesViewModel = SourceTargetFactory.GetSourceTargetVM(repository.Primitives, inputData.Primitives);
        }
        public void Refresh()
        {
            var combinations = CombinationViewModel.GetTargetItems();
            inputData.ForceActions.Clear();
            foreach (var item in combinations)
            {
                inputData.ForceActions.Add(item);
            }
            inputData.Primitives.Clear();
            foreach (var item in PrimitivesViewModel.GetTargetItems())
            {
                inputData.Primitives.Add(item.GetNdmPrimitive());
            }
        }
    }
}
