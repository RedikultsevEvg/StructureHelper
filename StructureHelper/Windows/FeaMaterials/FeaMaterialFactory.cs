using StructureHelper.Infrastructure.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.FeaMaterials;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.FeaMaterials
{
    public static class FeaMaterialFactory
    {
        internal static IFeaMaterial GetFeaMaterial(MaterialType type)
        {
            if (type is MaterialType.Elastic)
            {
                var newItem = new ElasticFeaMaterial(Guid.NewGuid())
                {
                    Name = "New elastic material",
                    YoungModulus = 2e11,
                    PoissonRatio = 0.3,
                };
                return newItem;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(type));
            }
        }
    }
}
