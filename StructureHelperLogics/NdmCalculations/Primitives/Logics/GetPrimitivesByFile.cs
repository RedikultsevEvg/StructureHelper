using StructureHelperCommon.Services.Exports;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class GetPrimitivesByFile : IGetPrimitivesLogic
    {
        private List<INdmPrimitive> primitives = [];

        public string FileName { get; set; }
        public List<INdmPrimitive> Primitives => primitives;

        public void Import()
        {
            primitives.Clear();
            var importEntitiesLogic = new EntitiesImportFromDxfLogic() { FileName = FileName};
            importEntitiesLogic.Import();
            if (importEntitiesLogic.Entities.Count != 0)
            {
                var primitivesLogic = new GetPrimitivesByDxfEntities();
                primitives = primitivesLogic.Convert(importEntitiesLogic.Entities);
            }
        }
    }
}
