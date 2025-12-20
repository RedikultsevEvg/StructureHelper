using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;

namespace StructureHelper.Windows.ViewModels.Materials
{
    public class ReinforcementViewModel : LibMaterialViewModel<IReinforcementMaterialEntity>
    {
        public ReinforcementViewModel(IReinforcementLibMaterial reinforcementMaterial) : base(reinforcementMaterial)
        {
        }
    }
}
