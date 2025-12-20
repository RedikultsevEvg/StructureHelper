using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.Models.Materials;

namespace StructureHelper.Windows.MainWindow.Materials
{
    public class SteelMaterialViewModel : HeadMaterialBaseViewModel
    {
        private readonly ISteelLibMaterial steelLibMaterial;

        public SteelMaterialViewModel(IHeadMaterial steelHeadMaterial) : base(steelHeadMaterial)
        {
            if (steelHeadMaterial.HelperMaterial is not ISteelLibMaterial steelHelperMaterial)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(steelHeadMaterial));
            }
            this.steelLibMaterial = steelHelperMaterial;
            HelperMaterialViewModel = new SteelHelperMaterialViewModel(steelLibMaterial);

        }
    }
}
