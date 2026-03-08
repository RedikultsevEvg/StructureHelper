using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.ScriptExports;

namespace StructureHelperCommon.Models.FeaMaterials.ExportLogics
{
    public class ElasticMaterialAbaqusInpBuilder : IScriptBuilder
    {
        const double stressFactor = 1.0e-6;

        public string Build(IFeaMaterial material)
        {
            if (material is not IElasticFeaMaterial elasticMaterial)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(material));
            }
            var builder = new KeywordBuilder();

            builder.AddKeyword("*Material", "name", elasticMaterial.Name);
            builder.AddKeyword("*Elastic");
            builder.AddData(elasticMaterial.YoungModulus * stressFactor, elasticMaterial.PoissonRatio);

            return builder.ToString();
        }

        public bool CanBuild(IFeaMaterial material)
        {
            return material is IElasticFeaMaterial;
        }
    }
}
