using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ConcreteFeaMaterialUpdateStrategy : IParentUpdateStrategy<IConcreteFeaMaterial>
    {
        private IUpdateStrategy<ICdpProperty> cdpUpdateStrategy;
        private IUpdateStrategy<IConcreteFeaCompression> compressionUpdateStrategy;
        private IUpdateStrategy<IConcreteFeaTension> tensionUpdateStrategy;
        private IUpdateStrategy<ICdpProperty> CdpUpdateStrategy => cdpUpdateStrategy ??= new CdpPropertyUpdateStrategy();
        private IUpdateStrategy<IConcreteFeaCompression> CompressionUpdateStrategy => compressionUpdateStrategy ??= new ConcreteFeaCompressionUpdateStrategy();
        private IUpdateStrategy<IConcreteFeaTension> TensionUpdateStrategy => tensionUpdateStrategy ??= new ConcreteFeaTensionUpdateStrategy();
        public bool UpdateChildren { get; set; } = true;

        public void Update(IConcreteFeaMaterial targetObject, IConcreteFeaMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            targetObject.YoungModulus = sourceObject.YoungModulus;
            targetObject.PoissonRatio = sourceObject.PoissonRatio;
            if (UpdateChildren == true)
            {
                CheckObject.ThrowIfNull(sourceObject.CdpProperty);
                CheckObject.ThrowIfNull(targetObject.CdpProperty);
                CheckObject.ThrowIfNull(sourceObject.CompressionProperties);
                CheckObject.ThrowIfNull(sourceObject.TensionProperties);
                CheckObject.ThrowIfNull(targetObject.CompressionProperties);
                CheckObject.ThrowIfNull(targetObject.TensionProperties);
                CdpUpdateStrategy.Update(targetObject.CdpProperty, sourceObject.CdpProperty);
                CompressionUpdateStrategy.Update(targetObject.CompressionProperties, sourceObject.CompressionProperties);
                TensionUpdateStrategy.Update(targetObject.TensionProperties, sourceObject.TensionProperties);
            }
        }
    }
}
