using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.FeaMaterials.ExportLogics;
using StructureHelperCommon.Models.ScriptExports;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class FeaMaterialPyBuilder : AbaqusMaterialPythonScriptBuilder
    {
        public override string Build(IFeaMaterial material)
        {
            ShortMaterialName = (material.Name).Replace(" ", string.Empty);
            ScriptMaterialName = "mat" + ShortMaterialName;
            MaterialName = material.Name;

            if (material is IElasticFeaMaterial elasticFeaMaterial)
            {
                var builder = new ElasticMaterialAbaqusPyBuilder();
                return builder.Build(elasticFeaMaterial);
            }
            else if (material is IConcreteFeaMaterial concreteMaterial)
            {
                var builder = new CdpMaterialAbaqusPyBuilder();
                return builder.Build(concreteMaterial);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(material));
            }
        }
    }
}
