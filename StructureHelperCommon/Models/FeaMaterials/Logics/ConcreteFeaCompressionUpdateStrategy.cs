using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ConcreteFeaCompressionUpdateStrategy : IUpdateStrategy<IConcreteFeaCompression>
    {
        public void Update(IConcreteFeaCompression targetObject, IConcreteFeaCompression sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Strength = sourceObject.Strength;
            targetObject.PeakStrain = sourceObject.PeakStrain;
            targetObject.ElasticStressRatio = sourceObject.ElasticStressRatio;
            targetObject.DescendingScaleFactor = sourceObject.DescendingScaleFactor;
        }
    }
}
