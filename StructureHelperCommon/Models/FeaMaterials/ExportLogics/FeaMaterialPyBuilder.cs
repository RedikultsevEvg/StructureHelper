using StructureHelperCommon.Infrastructures.Exceptions;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class FeaMaterialPyBuilder : AbaqusMaterialPythonScriptBuilder
    {
        public override string Build(IFeaMaterial material)
        {
            ShortMaterialName = (material.Name).Replace(" ", string.Empty);
            MaterialVariableName = ShortMaterialName;

            MaterialName = material.Name;
            var modelVariableName = "model";
            ModelBlock modelBlockBuilder = new("ConcreteCDP")
            {
                ModelVariableName = modelVariableName,
            };
            var materialBlockBuilder = MaterialBlockFactory.GetMaterialBlock(material, modelVariableName , MaterialVariableName);

            var script = new AbaqusScript()
                .Add(modelBlockBuilder)
                .Add(materialBlockBuilder)
                .Build();

            return script.ToString();
        }
    }
}
