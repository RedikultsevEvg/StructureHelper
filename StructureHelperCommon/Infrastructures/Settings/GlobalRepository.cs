using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Repositories;

namespace StructureHelperCommon.Infrastructures.Settings
{
    internal static class GlobalRepository
    {
        private static IDataRepository<IHeadMaterial> materials;
        private static IDataRepository<IAction> actions;

        //public static IDataRepository<IHeadMaterial> Materials
        //{
        //    get
        //    {
        //        materials ??= new ListRepository<IHeadMaterial>(new HeadMaterialUpdateStrategy());
        //        return materials;
        //    }
        //}
        //public static IDataRepository<IAction> Actions
        //{
        //    get
        //    {
        //        actions ??= new ListRepository<IAction>(new ActionUpdateStrategy());
        //        return actions;
        //    }
        //}
    }
}
