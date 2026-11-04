using StructureHelper.Infrastructure;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperCommon.Models.Materials.Logics;
using StructureHelperCommon.Services;
using StructureHelperCommon.Services.ColorServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Windows.ViewModels.Materials
{
    /// <summary>
    /// Material-level editor for a custom concrete helper material.
    /// Models are edited in place; Cancel restores the complete original state.
    /// </summary>
    public class CustomConcreteMaterialViewModel : OkCancelViewModelBase
    {
        private readonly IHeadMaterial headMaterial;
        private readonly string originalName;
        private readonly Color originalColor;
        private readonly List<ICustomConcreteModel> originalModels;
        private readonly List<CustomConcreteModelSnapshot> originalSnapshots;
        private readonly List<IMaterialSafetyFactor> originalSafetyFactors;
        private readonly bool originalTensionForULS;
        private readonly bool originalTensionForSLS;
        private RelayCommand editColorCommand;

        public IHeadMaterial HeadMaterial => headMaterial;
        public ICustomConcreteHelperMaterial HelperMaterial { get; }
        public CustomConcreteModelsViewModel Models { get; }
        public SafetyFactorsViewModel SafetyFactors { get; }

        public string Name
        {
            get => headMaterial.Name;
            set
            {
                if (headMaterial.Name == value) return;
                headMaterial.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public Color Color => headMaterial.Color;

        public bool TensionForULS
        {
            get => HelperMaterial.TensionForULS;
            set
            {
                if (HelperMaterial.TensionForULS == value)
                {
                    return;
                }
                HelperMaterial.TensionForULS = value;
                OnPropertyChanged(nameof(TensionForULS));
            }
        }

        public bool TensionForSLS
        {
            get => HelperMaterial.TensionForSLS;
            set
            {
                if (HelperMaterial.TensionForSLS == value)
                {
                    return;
                }
                HelperMaterial.TensionForSLS = value;
                OnPropertyChanged(nameof(TensionForSLS));
            }
        }

        public ICommand EditColorCommand => editColorCommand ??= new RelayCommand(EditColor);

        public CustomConcreteMaterialViewModel(IHeadMaterial headMaterial)
        {
            CheckObject.ThrowIfNull(headMaterial);
            if (headMaterial.HelperMaterial is not ICustomConcreteHelperMaterial concreteHelperMaterial)
            {
                throw new StructureHelperException(
                    ErrorStrings.ObjectTypeIsUnknown
                    + $". Expected: {typeof(ICustomConcreteHelperMaterial)}, but was: {headMaterial.HelperMaterial?.GetType()}");
            }

            this.headMaterial = headMaterial;
            HelperMaterial = concreteHelperMaterial;
            originalName = headMaterial.Name;
            originalColor = headMaterial.Color;
            originalModels = HelperMaterial.MaterialModels.ToList();
            originalSnapshots = originalModels
                .Select(x => new CustomConcreteModelSnapshot(x))
                .ToList();
            originalSafetyFactors = HelperMaterial.SafetyFactors
                .Select(x => x.Clone() as IMaterialSafetyFactor)
                .ToList();
            originalTensionForULS = HelperMaterial.TensionForULS;
            originalTensionForSLS = HelperMaterial.TensionForSLS;
            Models = new CustomConcreteModelsViewModel(HelperMaterial.MaterialModels);
            SafetyFactors = new SafetyFactorsViewModel(HelperMaterial.SafetyFactors);
        }

        public override void OkAction()
        {
            string error = GetMaterialValidationError();
            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error, "Check custom concrete material", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            base.OkAction();
        }

        public override void CancelAction()
        {
            RestoreOriginalState();
            base.CancelAction();
        }

        public string ValidationError => GetMaterialValidationError();

        public void RestoreOriginalState()
        {
            headMaterial.Name = originalName;
            headMaterial.Color = originalColor;
            HelperMaterial.MaterialModels.Clear();
            HelperMaterial.MaterialModels.AddRange(originalModels);
            HelperMaterial.SafetyFactors.Clear();
            HelperMaterial.SafetyFactors.AddRange(originalSafetyFactors);
            foreach (CustomConcreteModelSnapshot snapshot in originalSnapshots)
            {
                snapshot.Restore();
            }
            HelperMaterial.TensionForULS = originalTensionForULS;
            HelperMaterial.TensionForSLS = originalTensionForSLS;
            OnPropertyChanged(nameof(TensionForULS));
            OnPropertyChanged(nameof(TensionForSLS));
            Models.Reload();
            SafetyFactors.Refresh();
        }

        private void EditColor(object commandParameter)
        {
            Color color = headMaterial.Color;
            ColorProcessor.EditColor(ref color);
            headMaterial.Color = color;
            OnPropertyChanged(nameof(Color));
        }

        private string GetMaterialValidationError()
        {
            var checkLogic = new CustomConcreteHelperMaterialCheckLogic
            {
                Entity = HelperMaterial,
                TraceLogger = null
            };
            return checkLogic.Check()
                ? string.Empty
                : checkLogic.CheckResult.Trim();
        }
    }
}
