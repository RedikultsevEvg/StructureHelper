using StructureHelper.Windows.ViewModels.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;

namespace StructureHelper.Windows.MainWindow.Materials
{
    public class SteelHelperMaterialViewModel : HelperMaterialViewModel
    {
        private readonly ISteelLibMaterial steelLibMaterial;
        private double workConditionFactor;

        public LibMaterialViewModel<ISteelMaterialEntity> LibMaterialViewModel { get; set; }

        public double UlsFactor
        {
            get => steelLibMaterial.UlsFactor;
            set
            {
                steelLibMaterial.UlsFactor = Math.Min(value, 1.0);
                OnPropertyChanged(nameof(UlsFactor));
            }
        }
        public double ThicknessFactor
        {
            get => steelLibMaterial.ThicknessFactor;
            set
            {
                value = Math.Max(value, 0.0);
                steelLibMaterial.ThicknessFactor = Math.Min(value, 1.0);
                OnPropertyChanged(nameof(ThicknessFactor));
            }
        }
        public double MaxPlasticStrainRatio
        {
            get => steelLibMaterial.MaxPlasticStrainRatio;
            set
            {
                steelLibMaterial.MaxPlasticStrainRatio = Math.Max(value, 0.0);
            }
        }

        public double WorkConditionFactor
        {
            get => steelLibMaterial.WorkConditionFactor;
            set
            {
                steelLibMaterial.WorkConditionFactor = Math.Max(value, 0.0);
                OnPropertyChanged(nameof(WorkConditionFactor));
            }
        }

        public SteelHelperMaterialViewModel(ISteelLibMaterial steelLibMaterial)
        {
            this.steelLibMaterial = steelLibMaterial;
            LibMaterialViewModel = new(steelLibMaterial);
        }
    }
}
