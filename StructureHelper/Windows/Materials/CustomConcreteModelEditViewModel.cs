using StructureHelper.Windows.Materials;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Logics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace StructureHelper.Windows.ViewModels.Materials
{
    /// <summary>
    /// Modal editor for one ICustomConcreteModel. It edits the original object and restores
    /// its previous values when the dialog is cancelled.
    /// </summary>
    public class CustomConcreteModelEditViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private readonly ICustomConcreteModel model;
        private readonly IReadOnlyCollection<ICustomConcreteModel> allModels;
        private readonly CustomConcreteModelSnapshot snapshot;

        public ICustomConcreteModel Model => model;

        public CustomConcreteCompressionOptionsViewModel CompressionOptions { get; }
        public CustomConcreteTensionOptionsViewModel TensionOptions { get; }

        public List<LimitStates> LimitStatesCollection { get; } = Enum.GetValues<LimitStates>().ToList();
        public List<CalcTerms> CalcTermsCollection { get; } = Enum.GetValues<CalcTerms>().ToList();

        public CustomConcreteModelEditViewModel(
            ICustomConcreteModel model,
            IReadOnlyCollection<ICustomConcreteModel> allModels)
        {
            this.model = model ?? throw new ArgumentNullException(nameof(model));
            this.allModels = allModels ?? throw new ArgumentNullException(nameof(allModels));
            snapshot = new CustomConcreteModelSnapshot(model);
            CompressionOptions = new CustomConcreteCompressionOptionsViewModel(model.CompressionOptions);
            TensionOptions = new CustomConcreteTensionOptionsViewModel(model.TensionOptions);
        }

        public string Name
        {
            get => model.Name;
            set
            {
                if (model.Name == value) return;
                model.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public bool IsActive
        {
            get => model.IsActive;
            set
            {
                if (model.IsActive == value) return;
                model.IsActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        public double YoungModulus
        {
            get => model.YoungModulus;
            set
            {
                if (model.YoungModulus == value) return;
                model.YoungModulus = value;
                OnPropertyChanged(nameof(YoungModulus));
            }
        }

        public LimitStates LimitState
        {
            get => model.StateCalcTermPair.LimitState;
            set
            {
                if (model.StateCalcTermPair.LimitState == value) return;
                model.StateCalcTermPair.LimitState = value;
                OnPropertyChanged(nameof(LimitState));
            }
        }

        public CalcTerms CalcTerm
        {
            get => model.StateCalcTermPair.CalcTerm;
            set
            {
                if (model.StateCalcTermPair.CalcTerm == value) return;
                model.StateCalcTermPair.CalcTerm = value;
                OnPropertyChanged(nameof(CalcTerm));
            }
        }

        public string Error => GetValidationError();

        public string ValidationError => GetValidationError();

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Name) && string.IsNullOrWhiteSpace(Name))
                {
                    return "Name must not be empty";
                }
                if (columnName == nameof(YoungModulus)
                    && (!IsFinite(YoungModulus) || YoungModulus <= 0))
                {
                    return "Young modulus must be positive";
                }
                if (columnName == nameof(IsActive)
                    || columnName == nameof(LimitState)
                    || columnName == nameof(CalcTerm))
                {
                    return GetActivePairError();
                }
                return string.Empty;
            }
        }

        public override void OkAction()
        {
            string error = GetValidationError();
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Check custom concrete model", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            base.OkAction();
        }

        public override void CancelAction()
        {
            RestoreOriginalValues();
            base.CancelAction();
        }

        public void RestoreOriginalValues()
        {
            snapshot.Restore();
            CompressionOptions.Refresh();
            TensionOptions.Refresh();
        }

        private string GetValidationError()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return "Name must not be empty";
            }
            var modelCheckLogic = new CustomConcreteModelCheckLogic
            {
                Entity = model,
                TraceLogger = null
            };
            if (!modelCheckLogic.Check())
            {
                return modelCheckLogic.CheckResult.Trim();
            }
            string activePairError = GetActivePairError();
            if (!string.IsNullOrEmpty(activePairError))
            {
                return activePairError;
            }
            return string.Empty;
        }

        private string GetActivePairError()
        {
            if (!IsActive)
            {
                return string.Empty;
            }
            bool hasDuplicate = allModels.Any(x =>
                !ReferenceEquals(x, model)
                && x.IsActive
                && x.StateCalcTermPair.LimitState == LimitState
                && x.StateCalcTermPair.CalcTerm == CalcTerm);
            return hasDuplicate
                ? "Another active model already uses the same limit state and calculation term"
                : string.Empty;
        }

        private static bool IsFinite(double value) => !double.IsNaN(value) && !double.IsInfinity(value);
    }
}
