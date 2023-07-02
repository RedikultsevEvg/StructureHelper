using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
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
        public static IDataRepository<IHeadMaterial> Materials
        {
            get
            {
                materials ??= new ListRepository<IHeadMaterial>(new MaterialUpdateStrategy());
                return materials;
            }
        }
    }
}
