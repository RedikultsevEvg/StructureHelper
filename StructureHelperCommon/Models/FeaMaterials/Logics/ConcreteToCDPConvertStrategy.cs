using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ConcreteToCDPConvertStrategy : IObjectConvertStrategy<FeaMaterialCDP, IConcreteFeaMaterial>
    {
        private IUpdateStrategy<ICdpProperty> cdpUpdateStrategy;
        private IObjectConvertStrategy<CDPInelasticStrain, IConcreteFeaMaterial> compressionStrategy;
        private IObjectConvertStrategy<CDPInelasticStrain, IConcreteFeaMaterial> tensionStrategy;
        private IUpdateStrategy<ICdpProperty> CdpUpdateStrategy => cdpUpdateStrategy ??= new CdpPropertyUpdateStrategy();
        private IObjectConvertStrategy<CDPInelasticStrain, IConcreteFeaMaterial> CompressionStrategy => compressionStrategy ??= new ConcreteToCDPCompressionConvertStrategy();
        private IObjectConvertStrategy<CDPInelasticStrain, IConcreteFeaMaterial> TensionStrategy => tensionStrategy ??= new ConcreteToCDPTensionConvertStrategy();

        public FeaMaterialCDP Convert(IConcreteFeaMaterial source)
        {
            FeaMaterialCDP cdp = new()
            {
                Name = source.Name,
                YoungModulus = source.YoungModulus,
                PoissonRatio = source.PoissonRatio,
            };
            CdpUpdateStrategy.Update(cdp.CdpProperty, source.CdpProperty);
            cdp.CompressionStrain = CompressionStrategy.Convert(source);
            cdp.TensionStrain = TensionStrategy.Convert(source);
            return cdp;
        }
    }
}
