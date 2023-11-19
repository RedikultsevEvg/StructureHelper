using LoaderCalculator.Data.Materials;
using StructureHelper.Infrastructure;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal class ConcreteViewModel : LibMaterialViewModel<IConcreteMaterialEntity>
    {
        readonly IConcreteLibMaterial concreteMaterial;
        public bool TensionForULS
        {
            get => concreteMaterial.TensionForULS;
            set
            {
                concreteMaterial.TensionForULS = value;
                OnPropertyChanged(nameof(TensionForULS));
            }
        }
        public bool TensionForSLS
        {
            get => concreteMaterial.TensionForSLS;
            set
            {
                concreteMaterial.TensionForSLS = value;
                OnPropertyChanged(nameof(TensionForSLS));
            }
        }
        public double Humidity
        {
            get => concreteMaterial.RelativeHumidity;
            set
            {
                concreteMaterial.RelativeHumidity = value;
                OnPropertyChanged(nameof(Humidity));
            }
        }           

        public ConcreteViewModel(ILibMaterial material) : base(material)
        {
            if (material is not IConcreteLibMaterial)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $". Expected: {typeof(IConcreteLibMaterial)}, but was: {material.GetType()}");
            }
            concreteMaterial = material as IConcreteLibMaterial;
        }
    }
}
