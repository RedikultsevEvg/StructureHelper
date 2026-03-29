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

        public string MaterialNameVar { get; set; } = "elasticMaterialName";
        public string MaterialVar { get; set; } = "elasticMaterial";

        public ElasticMaterialBlock(IElasticFeaMaterial elasticFeaMaterial)
        {
            this.material = elasticFeaMaterial;
        }

        public void Build(IAbaqusContext context)
        {
            var model = context.Get<ModelContext>();
            builder = context.Builder;

            string modelNameVar = model.ModelNameVar;
            string materialName = material.Name;
            string stressFactor = model.StressFactorName;
            string youngModulus = $"{FormatConverter.FormatDouble(material.YoungModulus)} * {stressFactor}";
            string poissonRatio = FormatConverter.FormatDouble(material.PoissonRatio);
            
            builder.AddRaw("");
            builder.AddComment($"Elastic material {materialName}");
            builder.AddKeyword($"{MaterialNameVar} = '{materialName}'");
            builder.AddKeyword($"mdb.models[{modelNameVar}].Material(name = {MaterialNameVar})");
            builder.AddKeyword($"mdb.models[{modelNameVar}].materials[{MaterialNameVar}].Elastic(table=(({youngModulus},{poissonRatio}),))");
            builder.AddKeyword($"{MaterialVar} = mdb.models[{modelNameVar}].materials[{MaterialNameVar}]");
        }
    }
}
