using StructureHelperCommon.Models.FeaMaterials.ExportLogics;
using System;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ElasticMaterialBlock : IMaterialBlock
    {
        private IElasticFeaMaterial material;

        public ElasticMaterialBlock(IElasticFeaMaterial elasticFeaMaterial)
        {
            this.material = elasticFeaMaterial;
        }

        public void Build(IAbaqusContext context)
        {
            var model = context.Get<ModelContext>();
            var b = context.Builder;

            string modelName = model.ModelName;
            string materialName = material.Name;
            string youngModulus = $"{FormatConverter.FormatDouble(material.YoungModulus)} * stressFactor";
            string poissonRatio = FormatConverter.FormatDouble(material.PoissonRatio);

            b.AddComment($"Elastic material {materialName}");
            b.AddKeyword($"mdb.models['{modelName}'].Material(name = '{materialName}')");
            b.AddKeyword($"mdb.models['{modelName}'].material.Elastic(table=(({youngModulus},{poissonRatio}),))");
        }
    }
}
