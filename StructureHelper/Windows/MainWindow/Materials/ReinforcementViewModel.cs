using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Codes;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Materials
{
    public class ReinforcementViewModel : LibMaterialViewModel<IReinforcementMaterialEntity>
    {
        public ReinforcementViewModel(IReinforcementLibMaterial reinforcementMaterial) : base(reinforcementMaterial)
        {
        }
    }
}
