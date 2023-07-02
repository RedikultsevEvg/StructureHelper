using LoaderCalculator.Data.Materials;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Triangulations
{
    public static class Triangulation
    {
        public static IEnumerable<INdm> GetNdms(IEnumerable<INdmPrimitive> ndmPrimitives, ITriangulationOptions options)
        {
            List<INdm> ndms = new List<INdm>();
            var headMaterials = GetPrimitiveMaterials(ndmPrimitives);
            Dictionary<Guid, IMaterial> materials = GetMaterials(headMaterials, options);
            foreach (var ndmPrimitive in ndmPrimitives)
            {
                IHeadMaterial headMaterial = ndmPrimitive.HeadMaterial;
                IMaterial material;
                if (materials.TryGetValue(headMaterial.Id, out material) == false) { throw new Exception("Material dictionary is not valid"); }
                IEnumerable<INdm> localNdms = GetNdmsByPrimitive(ndmPrimitive, material);
                ndms.AddRange(localNdms);
            }
            return ndms;
        }
        /// <summary>
        /// Returns dictionary of unique materials by collection of primitives
        /// </summary>
        /// <param name="ndmPrimitives"></param>
        /// <returns></returns>
        private static Dictionary<Guid, IHeadMaterial> GetPrimitiveMaterials(IEnumerable<INdmPrimitive> ndmPrimitives)
        {
            Dictionary<Guid, IHeadMaterial> headMaterials = new Dictionary<Guid, IHeadMaterial>();
            foreach (var ndmPrimitive in ndmPrimitives)
            {
                IHeadMaterial material = ndmPrimitive.HeadMaterial;
                if (!headMaterials.ContainsKey(material.Id)) { headMaterials.Add(material.Id, material); }
            }
            return headMaterials;
        }
        /// <summary>
        /// Return dictionary of ndm-materials by dictionary of primirive materials
        /// </summary>
        /// <param name="PrimitiveMaterials"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        /// <exception cref="StructureHelperException"></exception>
        private static Dictionary<Guid, IMaterial> GetMaterials(Dictionary<Guid, IHeadMaterial> PrimitiveMaterials, ITriangulationOptions options)
        {
            Dictionary<Guid, IMaterial> materials = new Dictionary<Guid, IMaterial>();
            IEnumerable<Guid> keyCollection = PrimitiveMaterials.Keys;
            IMaterial material;
            foreach (var id in keyCollection)
            {
                IHeadMaterial headMaterial;
                if (PrimitiveMaterials.TryGetValue(id, out headMaterial) == false) { throw new StructureHelperException("Material dictionary is not valid"); }
                material = headMaterial.GetLoaderMaterial(options.LimiteState, options.CalcTerm);
                materials.Add(id, material);
            }
            return materials;
        }

        private static IEnumerable<INdm> GetNdmsByPrimitive(INdmPrimitive primitive, IMaterial material)
        {
            List<INdm> ndms = new List<INdm>();
            ndms.AddRange(primitive.GetNdms(material));
            return ndms;
        }
    }
}
