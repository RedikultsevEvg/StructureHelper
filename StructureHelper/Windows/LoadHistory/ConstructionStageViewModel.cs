using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.LoadHistory;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace StructureHelper.Windows.LoadHistory
{
    public class ConstructionStageViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private const int minSubStepNumber = 3;
        private const double minDuration = 0d;

        public IConstructionStage ConstructionStage { get; }
        public SourceTargetVM<PrimitiveBase> PrimitivesViewModel { get; private set; }
        public SelectItemVM<IForceAction> CombinationsViewModel { get; private set; }

        public string Name
        {
            get => ConstructionStage.Name;
            set
            {
                ConstructionStage.Name = value;
                Refresh();
            }
        }

        public int SubStepNumber
        {
            get => ConstructionStage.SubStepNumber;
            set
            {
                ConstructionStage.SubStepNumber = Math.Max(value, minSubStepNumber);
            }
        }

        public double Duration
        {
            get => ConstructionStage.Duration;
            set
            {
                ConstructionStage.Duration = Math.Max(value, minDuration);
            }
        }

        public string WindowName => "Costruction stage: " + ConstructionStage.Name;
        public static string SubStepNumberName => $"Sub step number (min {minSubStepNumber})";

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                if (columnName == nameof(SubStepNumber))
                {
                    if (SubStepNumber < minSubStepNumber)
                    {
                        error = $"Substep number must not be less than minimum number n = {minSubStepNumber}";
                    }

                }
                if (columnName == nameof(Duration))
                {
                    if (Duration < minDuration)
                    {
                        error = $"Duration of stage must not be less than minimum duration t = {minDuration}";
                    }

                }
                return error;
            }
        }

        public int PrimitivesNumber => PrimitivesViewModel.TargetItems.Count;
        public string ForceActionName => ConstructionStage.ForceAction.Name;

        public override void OkAction()
        {
            ConstructionStage.ForceAction = CombinationsViewModel.SelectedItem;
            ConstructionStage.Primitives.Clear();
            foreach (var item in PrimitivesViewModel.GetTargetItems())
            {
                ConstructionStage.Primitives.Add(item.GetNdmPrimitive());
            }
            Refresh();
            base.OkAction();
        }

        private void Refresh()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(WindowName));
            OnPropertyChanged(nameof(SubStepNumber));
            OnPropertyChanged(nameof(PrimitivesNumber));
            OnPropertyChanged(nameof(ForceActionName));
        }

        public ConstructionStageViewModel(IConstructionStage constructionStage, IEnumerable<INdmPrimitive> allowedPrimitives, IEnumerable<IForceAction> allowedCombinations)
        {
            this.ConstructionStage = constructionStage;
            CombinationsViewModel = new([.. allowedCombinations])
            {
                SelectedItem = constructionStage.ForceAction
            };
            PrimitivesViewModel = SourceTargetFactory.GetSourceTargetVM(allowedPrimitives, constructionStage.Primitives);
        }
    }
}
