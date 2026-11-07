using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Materials.Libraries;
using System;
using System.Windows;

namespace StructureHelper.Windows.ViewModels.Materials
{
    /// <summary>
    /// Simple editor for one material safety factor.
    /// </summary>
    public class SafetyFactorEditViewModel : OkCancelViewModelBase
    {
        private readonly IMaterialSafetyFactor factor;
        private readonly string originalName;
        private readonly bool originalTake;
        private readonly string originalDescription;

        public SafetyFactorEditViewModel(IMaterialSafetyFactor factor)
        {
            this.factor = factor ?? throw new ArgumentNullException(nameof(factor));
            originalName = factor.Name;
            originalTake = factor.Take;
            originalDescription = factor.Description;
        }

        public string Name
        {
            get => factor.Name;
            set
            {
                if (factor.Name == value) return;
                factor.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public bool Take
        {
            get => factor.Take;
            set
            {
                if (factor.Take == value) return;
                factor.Take = value;
                OnPropertyChanged(nameof(Take));
            }
        }

        public string Description
        {
            get => factor.Description;
            set
            {
                if (factor.Description == value) return;
                factor.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public override void CancelAction()
        {
            RestoreOriginalValues();
            base.CancelAction();
        }

        public void RestoreOriginalValues()
        {
            factor.Name = originalName;
            factor.Take = originalTake;
            factor.Description = originalDescription;
        }
    }
}
