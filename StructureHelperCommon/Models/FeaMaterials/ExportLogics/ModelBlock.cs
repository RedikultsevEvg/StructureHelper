using StructureHelperCommon.Models.FeaMaterials.ExportLogics;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ModelBlock : IModelBlock
    {
        private const string forceFactor = "forceFactor";
        private const string lengthFactor = "lengthFactor";
        private const string stressFactor = "stressFactor";
        private const double forceFactorValue = 1.0;
        private const double lengthFactorValue = 1000.0;
        private readonly string modelName;
        public string ModelNameVar { get; set; } = "modelName";

        public ModelBlock(string modelName)
        {
            this.modelName = modelName;

        }

        public void Build(IAbaqusContext context)
        {
            context.Set(new ModelContext
            {
                ModelName = modelName,
                ModelNameVar = ModelNameVar,
                ForceFactorName = forceFactor,
                LengthFactorName = lengthFactor,
                StressFactorName = stressFactor,
            });

            var b = context.Builder;

            b.AddKeyword($"{ModelNameVar} = '{modelName}'");
            b.AddKeyword($"if {ModelNameVar} in mdb.models:\r\n   del mdb.models[{ModelNameVar}]");
            b.AddKeyword($"mdb.Model(name={ModelNameVar})");
            b.AddKeyword($"model = mdb.models[{ModelNameVar}]");
            b.AddRaw("");
            b.AddCommentedHeader("Unit factors");
            b.AddComment("Set 1.0 for SI unit");
            b.AddKeyword($"{forceFactor} = {forceFactorValue} #N");
            b.AddKeyword($"{lengthFactor} = {lengthFactorValue} #mm in m");
            b.AddKeyword($"{stressFactor} = {forceFactor} / ({lengthFactor} * {lengthFactor}) # = {forceFactorValue} / ({lengthFactorValue} * {lengthFactorValue}) = {forceFactorValue / (lengthFactorValue * lengthFactorValue)} MPa in Pa");

        }
    }
}
