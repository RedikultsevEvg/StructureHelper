using StructureHelperCommon.Models.FeaMaterials.ExportLogics;
using StructureHelperCommon.Models.ScriptExports;
using System;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ElasticMaterialBlock : IMaterialBlock
    {
        private IElasticFeaMaterial material;
        private IKeywordBuilder builder;

        public string MaterialVariableName { get; set; } = "elasticMaterialName";
        public string ModelVariableName { get; set; } = "Model-1";

        public ElasticMaterialBlock(IElasticFeaMaterial elasticFeaMaterial)
        {
            this.material = elasticFeaMaterial;
        }

        public void Build(IAbaqusContext context)
        {
            var modelContext = context.Get<ModelContext>();
            builder = context.Builder;

            string materialName = material.Name;
            string stressFactor = modelContext.StressFactorName;
            string youngModulus = $"{FormatConverter.FormatDouble(material.YoungModulus)} * {stressFactor}";
            string poissonRatio = FormatConverter.FormatDouble(material.PoissonRatio);
            
            builder.AddRaw("");
            builder.AddCommentedHeader($"Elastic material {materialName}");
            builder.AddKeyword($"{ModelVariableName}.Material(name = '{materialName}')");
            builder.AddKeyword($"{MaterialVariableName} = {ModelVariableName}.materials['{materialName}']");
            builder.AddKeyword($"{MaterialVariableName}.Elastic(table=(({youngModulus},{poissonRatio}),))");
        }
    }
}
