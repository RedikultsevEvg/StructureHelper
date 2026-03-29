using StructureHelperCommon.Models.FeaMaterials.ExportLogics;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ModelBlock : IModelBlock
    {
        private readonly string modelName;

        public ModelBlock(string modelName)
        {
            this.modelName = modelName;
        }

        public void Build(IAbaqusContext context)
        {
            context.Set(new ModelContext
            {
                ModelName = modelName
            });

            var b = context.Builder;

            b.AddKeyword($"modelName = '{modelName}'");
            b.AddKeyword($"mdb.Model(name=modelName)");
            b.AddKeyword($"model = mdb.models[modelName]");
            b.AddCommentedHeader("Unit factors");
            b.AddComment("Set 1.0 for SI unit");
            b.AddKeyword($"stressFactor = 1e-6");
            b.AddKeyword($"lengthFactor = 1000");
        }
    }
}
