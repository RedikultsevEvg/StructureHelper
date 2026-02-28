using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ConcreteToCDPConvertStrategy : IObjectConvertStrategy<FeaMaterialCDP, IConcreteFeaMaterial>
    {
        private IObjectConvertStrategy<CDPInelasticStrain, IConcreteFeaMaterial> compressionStrategy;
        private IObjectConvertStrategy<CDPInelasticStrain, IConcreteFeaMaterial> tensionStrategy;
        private IObjectConvertStrategy<CDPInelasticStrain, IConcreteFeaMaterial> CompressionStrategy => compressionStrategy ??= new ConcreteToCDPCompressionConvertStrategy();
        private IObjectConvertStrategy<CDPInelasticStrain, IConcreteFeaMaterial> TensionStrategy => tensionStrategy ??= new ConcreteToCDPTensionConvertStrategy();

        public FeaMaterialCDP Convert(IConcreteFeaMaterial source)
        {
            FeaMaterialCDP cdp = new()
            {
                Name = source.Name,
                YoungsModulus = source.YoungsModulus,
                DilationAngle = 35
            };
            cdp.CompressionStrain = CompressionStrategy.Convert(source);
            cdp.TensionStrain = TensionStrategy.Convert(source);
            return cdp;
        }
    }
}
