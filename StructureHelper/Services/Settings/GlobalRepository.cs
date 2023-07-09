using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Repositories;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Services.Settings
{
    internal static class GlobalRepository
    {
        private static IDataRepository<IHeadMaterial> materials;
        private static IDataRepository<IAction> actions;

        public static IDataRepository<IHeadMaterial> Materials
        {
            get
            {
                materials ??= new ListRepository<IHeadMaterial>(new MaterialUpdateStrategy());
                return materials;
            }
        }
        public static IDataRepository<IAction> Actions
        {
            get
            {
                actions ??= new ListRepository<IAction>(new ActionUpdateStrategy());
                return actions;
            }
        }
    }
}
