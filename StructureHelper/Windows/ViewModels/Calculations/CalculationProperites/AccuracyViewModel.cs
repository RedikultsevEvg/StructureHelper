using StructureHelperCommon.Models.Calculators;
using System;
using System.ComponentModel;
using System.Windows;

namespace StructureHelper.Windows.ViewModels
{
    /// <summary>
    /// Edits a copy of an <see cref="IAccuracy"/> entity. Changes are applied to the target only
    /// when the OK action succeeds; cancel or a failed validation leaves the target untouched.
    /// </summary>
    public class AccuracyViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private readonly IAccuracy accuracy;
        private readonly Accuracy editedAccuracy;
        private readonly AccuracyUpdateStrategy updateStrategy = new();
        private string validationMessage = string.Empty;

        public AccuracyViewModel(IAccuracy accuracy)
        {
            this.accuracy = accuracy ?? throw new ArgumentNullException(nameof(accuracy));
            editedAccuracy = new Accuracy
            {
                IterationAccuracy = accuracy.IterationAccuracy,
                MaxIterationCount = accuracy.MaxIterationCount
            };
        }

        public double IterationAccuracy
        {
            get => editedAccuracy.IterationAccuracy;
            set
            {
                if (editedAccuracy.IterationAccuracy == value)
                {
                    return;
                }
                editedAccuracy.IterationAccuracy = value;
                OnPropertyChanged(nameof(IterationAccuracy));
            }
        }

        public int MaxIterationCount
        {
            get => editedAccuracy.MaxIterationCount;
            set
            {
                if (editedAccuracy.MaxIterationCount == value)
                {
                    return;
                }
                editedAccuracy.MaxIterationCount = value;
                OnPropertyChanged(nameof(MaxIterationCount));
            }
        }

        public string ValidationMessage
        {
            get => validationMessage;
            private set
            {
                if (validationMessage == value)
                {
                    return;
                }
                validationMessage = value;
                OnPropertyChanged(nameof(ValidationMessage));
            }
        }

        /// <summary>
        /// Validates the edited values and copies them to the target. Returns false without
        /// modifying the target when the values are not valid.
        /// </summary>
        public bool TryApply()
        {
            if (double.IsNaN(IterationAccuracy) || double.IsInfinity(IterationAccuracy))
            {
                ValidationMessage = "Accuracy must be a finite number";
                return false;
            }

            var checkLogic = new CheckAccuracyLogic { Entity = editedAccuracy };
            if (checkLogic.Check() == false)
            {
                ValidationMessage = checkLogic.CheckResult;
                return false;
            }

            ValidationMessage = string.Empty;
            updateStrategy.Update(accuracy, editedAccuracy);
            return true;
        }

        public override void OkAction()
        {
            if (TryApply() == false)
            {
                MessageBox.Show(ValidationMessage, "Check accuracy", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            base.OkAction();
        }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(IterationAccuracy)
                    && (double.IsNaN(IterationAccuracy)
                        || double.IsInfinity(IterationAccuracy)
                        || IterationAccuracy <= 0))
                {
                    return "Accuracy must be a positive finite number";
                }
                if (columnName == nameof(MaxIterationCount) && MaxIterationCount <= 0)
                {
                    return "Maximum iteration count must be greater than zero";
                }
                return string.Empty;
            }
        }
    }
}
