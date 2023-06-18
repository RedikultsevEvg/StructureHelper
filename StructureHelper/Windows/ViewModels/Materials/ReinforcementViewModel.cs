using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperCommon.Models.Codes;
using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal class ReinforcementViewModel
    {
        public IEnumerable<ICodeEntity> CodeList => ProgramSetting
            .MaterialRepository
            .Repository
            .Select( x => x.Code)
            .Distinct();
        public IEnumerable<ILibMaterialEntity> MaterialLibrary => LibMaterialPepository.GetReinforcementRepository();


        public ReinforcementViewModel(ILibMaterial material)
        {
            if (material is not IReinforcementLibMaterial)
            {
                throw new StructureHelperException(ErrorStrings.DataIsInCorrect + $". Expected: {typeof(IConcreteLibMaterial)}, but was: {material.GetType()}");
            }
        }
    }
}
