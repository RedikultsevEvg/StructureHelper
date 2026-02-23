using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ElasticFeaMaterialUpdateStrategy : IUpdateStrategy<IElasticFeaMaterial>
    {
        public void Update(IElasticFeaMaterial targetObject, IElasticFeaMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.YoungModulus = sourceObject.YoungModulus;
            targetObject.PoissonRatio = sourceObject.PoissonRatio;
        }
    }
}
