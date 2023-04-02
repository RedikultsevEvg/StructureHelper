using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal class ReinforcementViewModel : LibMaterialViewModel
    {
        public override IEnumerable<ILibMaterialEntity> MaterialLibrary => LibMaterialPepository.GetReinforcementRepository(ProgramSetting.CodeType);
        public ReinforcementViewModel(ILibMaterial material) : base(material)
        {
            if (material is not IReinforcementLibMaterial)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $". Expected: {typeof(IConcreteLibMaterial)}, but was: {material.GetType()}");
            }
        }
    }
}
