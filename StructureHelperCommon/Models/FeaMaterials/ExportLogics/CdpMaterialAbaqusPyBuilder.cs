using StructureHelperCommon.Infrastructures.Exceptions;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class CdpMaterialAbaqusPyBuilder : AbaqusMaterialPythonScriptBuilder
    {
        const double stressFactor = 1.0e-6;
        const double densityFactor = 1.0e-12;
        private FeaMaterialCDP cdp;

        public override string Build(IFeaMaterial material)
        {
            if (material is not IConcreteFeaMaterial concreteMaterial)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(material));
            }
            GetCdp(concreteMaterial);

            ShortMaterialName = (material.Name).Replace(" ", string.Empty);
            MaterialVariableName = ShortMaterialName;
            MaterialName = material.Name;

            AddReferenceToModel();
            Builder.AddComment($"Concrete damage plasticity material {concreteMaterial.Name}");
            Builder.AddKeyword($"{MaterialVariableName}={ModelName}.Material(name='{concreteMaterial.Name}')");
            Builder.AddKeyword($"{MaterialVariableName}.Elastic(table=(({FormatDouble(cdp.YoungModulus * stressFactor)},{FormatDouble(cdp.PoissonRatio)}),))");
            Builder.AddKeyword($"{MaterialVariableName}.Density(table=(({FormatDouble(cdp.Density * densityFactor)},),))");

            AddCdpProperties();
            AddCdpCompression();
            AddCdpCompressionDamage();
            AddCdpTension();
            AddCdpTensionDamage();
            return Builder.ToString();
        }

        private void AddCdpTensionDamage()
        {
            Builder.AddComment("Tension damage (damage, inelastic strain)");
            Builder.AddKeyword($"{MaterialVariableName}.concreteDamagedPlasticity.ConcreteTensionDamage(");
            Builder.AddKeyword("    table=(");
            List<double> valueList = cdp.TensionStrain.DamageList;
            List<double> strainList = cdp.TensionStrain.InelasticStrainList;
            GetValueTable(strainList, valueList, 1.0);
        }

        private void AddCdpCompressionDamage()
        {
            Builder.AddComment("Compression damage (damage, inelastic strain)");
            Builder.AddKeyword($"{MaterialVariableName}.concreteDamagedPlasticity.ConcreteCompressionDamage(");
            Builder.AddKeyword("    table=(");
            List<double> valueList = cdp.CompressionStrain.DamageList;
            List<double> strainList = cdp.CompressionStrain.InelasticStrainList;
            GetValueTable(strainList, valueList, 1.0);
        }

        private void AddCdpTension()
        {
            Builder.AddComment("Tension stiffening (stress, cracking strain))");
            Builder.AddKeyword($"{MaterialVariableName}.concreteDamagedPlasticity.ConcreteTensionStiffening(");
            Builder.AddKeyword("    table=(");
            List<double> valueList = cdp.TensionStrain.StressList;
            List<double> strainList = cdp.TensionStrain.InelasticStrainList;
            GetValueTable(strainList, valueList, stressFactor);
        }

        private void AddCdpCompression()
        {
            Builder.AddComment("Compression hardening (stress, inelastic strain)");
            Builder.AddKeyword($"{MaterialVariableName}.concreteDamagedPlasticity.ConcreteCompressionHardening(");
            Builder.AddKeyword("    table=(");
            List<double> valueList = cdp.CompressionStrain.StressList;
            List<double> strainList = cdp.CompressionStrain.InelasticStrainList;
            GetValueTable(strainList, valueList, stressFactor);
        }

        private void GetValueTable(List<double> strainList, List<double> valueList, double factor)
        {
            int count = strainList.Count;
            for (int i = 0; i < count; i++)
            {
                string keyword = $"        ({FormatDouble(valueList[i] * factor)}, {FormatDouble(strainList[i])})";
                if (i < count - 1)
                {
                    keyword += ",";
                }
                Builder.AddKeyword(keyword);
            }
            Builder.AddKeyword(")) #end of concrete property table");
        }

        private void AddCdpProperties()
        {
            Builder.AddComment("Concrete Damaged Plasticity");
            Builder.AddKeyword($"{MaterialVariableName}.ConcreteDamagedPlasticity(");
            Builder.AddKeyword("    table=(");
            Builder.AddKeyword($"          ({FormatDouble(cdp.CdpProperty.DilationAngle)}, #Dilation angle");
            Builder.AddKeyword($"          {FormatDouble(cdp.CdpProperty.Eccentricity)}, #Eccentricity");
            Builder.AddKeyword($"          {FormatDouble(cdp.CdpProperty.Fb0Ratio)}, #Fb0 / Fc0 ratio");
            Builder.AddKeyword($"          {FormatDouble(cdp.CdpProperty.KRatio)}, #K");
            Builder.AddKeyword($"          {FormatDouble(cdp.CdpProperty.Viscosity)}) #Viscosity");
            Builder.AddKeyword("    , )) #end of Concrete damage plasticity");
        }

        private void GetCdp(IConcreteFeaMaterial concreteMaterial)
        {
            var convertLogic = new ConcreteToCDPConvertStrategy();
            cdp = convertLogic.Convert(concreteMaterial);
        }
    }
}
