using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public static class MaterialBlockFactory
    {
        public static IMaterialBlock GetMaterialBlock(IFeaMaterial material, string modelVariableName, string materialVariableName)
        {
            if (material is IElasticFeaMaterial elasticFeaMaterial)
            {
                return new ElasticMaterialBlock(elasticFeaMaterial)
                {
                    ModelVariableName = modelVariableName,
                    MaterialVariableName = materialVariableName
                };
            }
            else if (material is IConcreteFeaMaterial concreteMaterial)
            {
                return new ConcreteCDPMaterialBlock(concreteMaterial)
                {
                    ModelVariableName = modelVariableName,
                    MaterialVariableName = materialVariableName
                };
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(material));
            }
        }
    }
}
