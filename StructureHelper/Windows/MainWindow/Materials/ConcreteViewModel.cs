using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;

namespace StructureHelper.Windows.ViewModels.Materials
{
    public class ConcreteViewModel : LibMaterialViewModel<IConcreteMaterialEntity>
    {
        private readonly IConcreteLibMaterial concreteMaterial;
        public bool TensionForULS
        {
            get => concreteMaterial.TensionForULS;
            set
            {
                concreteMaterial.TensionForULS = value;
                OnPropertyChanged(nameof(TensionForULS));
            }
        }
        public bool TensionForULSVisibility { get; set; } = true;
        public bool TensionForSLS
        {
            get => concreteMaterial.TensionForSLS;
            set
            {
                concreteMaterial.TensionForSLS = value;
                OnPropertyChanged(nameof(TensionForSLS));
            }
        }
        public bool TensionForSLSVisibility { get; set; } = true;
        public double Humidity
        {
            get => concreteMaterial.RelativeHumidity;
            set
            {
                concreteMaterial.RelativeHumidity = value;
                OnPropertyChanged(nameof(Humidity));
            }
        }           
        public bool HumidityVisibility { get; set; } = true;

        public ConcreteViewModel(IConcreteLibMaterial concreteMaterial) : base(concreteMaterial)
        {
            this.concreteMaterial = concreteMaterial;
        }
    }
}
