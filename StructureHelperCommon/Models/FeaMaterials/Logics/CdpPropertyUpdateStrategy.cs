using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class CdpPropertyUpdateStrategy : IUpdateStrategy<ICdpProperty>
    {
        public void Update(ICdpProperty targetObject, ICdpProperty sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.DilationAngle = sourceObject.DilationAngle;
            targetObject.Eccentricity = sourceObject.Eccentricity;
            targetObject.Fb0Ratio = sourceObject.Fb0Ratio;
            targetObject.KRatio = sourceObject.KRatio;
            targetObject.Viscosity = sourceObject.Viscosity;
        }
    }
}
