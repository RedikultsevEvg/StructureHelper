using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.ScriptExports;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials.ExportLogics
{
    public class ElasticMaterialAbaqusPyBuilder : AbaqusMaterialPythonScriptBuilder
    {
        const double stressFactor = 1.0e-6;

        public override string Build(IFeaMaterial material)
        {
            if (material is not IElasticFeaMaterial elasticMaterial)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(material));
            }
            ShortMaterialName = (material.Name).Replace(" ", string.Empty);
            ScriptMaterialName = "mat" + ShortMaterialName;
            MaterialName = material.Name;

            double youngModulus = elasticMaterial.YoungModulus * stressFactor;
            AddReferenceToModel();
            Builder.AddComment($"Elastic material {elasticMaterial.Name}");
            Builder.AddKeyword($"{ScriptMaterialName}={ModelName}.Material(name='{elasticMaterial.Name}')");
            Builder.AddKeyword($"{ScriptMaterialName}.Elastic(table=(({FormatDouble(youngModulus)},{FormatDouble(elasticMaterial.PoissonRatio)}),))");

            return Builder.ToString();
        }
    }
}
