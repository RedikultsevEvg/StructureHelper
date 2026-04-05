using StructureHelperCommon.Models.FeaMaterials.ExportLogics;
using StructureHelperCommon.Models.ScriptExports;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.FeaMaterials
{
    public class ConcreteCDPMaterialBlock : IMaterialBlock
    {
        private IFeaMaterialCDP material;
        private IKeywordBuilder builder;
        private string modelVariableName;
        private string materialName;
        private string stressFactor;
        private string youngModulus;
        private string poissonRatio;

        public string ModelVariableName { get; set; } = "Model-1";
        public string MaterialVariableName { get; set; } = "cdpMaterial";
        public string MaterialName { get; set; } = "CdpMaterial";

        public ConcreteCDPMaterialBlock(IFeaMaterialCDP material)
        {
            this.material = material;
        }

        public ConcreteCDPMaterialBlock(IConcreteFeaMaterial concreteMaterial)
        {
            var convertLogic = new ConcreteToCDPConvertStrategy();
            this.material = convertLogic.Convert(concreteMaterial);
        }

        public void Build(IAbaqusContext context)
        {
            var model = context.Get<ModelContext>();
            builder = context.Builder;

            modelVariableName = model.ModelVaribleName;
            materialName = material.Name;
            stressFactor = model.StressFactorName;
            youngModulus = $"{FormatConverter.FormatDouble(material.YoungModulus)} * {stressFactor}";
            poissonRatio = FormatConverter.FormatDouble(material.PoissonRatio);

            AddElasticProperties();

            AddCdpProperties();
            AddCdpCompression();
            AddCdpCompressionDamage();
            AddCdpTension();
            AddCdpTensionDamage();
        }

        private void AddElasticProperties()
        {
            builder.AddRaw("");
            builder.AddCommentedHeader($"Concrete damage plasticity material {materialName}");
            builder.AddComment($"Elastic material");
            builder.AddKeyword($"{ModelVariableName}.Material(name = '{materialName}')");
            builder.AddKeyword($"{MaterialVariableName} = {ModelVariableName}.materials['{materialName}']");
            builder.AddKeyword($"{MaterialVariableName}.Elastic(table=(({youngModulus},{poissonRatio}),))");
        }

        private void AddCdpTensionDamage()
        {
            builder.AddRaw("");
            builder.AddComment($"Concrete damage plasticity");
            builder.AddComment("Tension damage (damage, inelastic strain)");
            builder.AddKeyword($"{MaterialVariableName}.concreteDamagedPlasticity.ConcreteTensionDamage(");
            builder.AddKeyword("    table=(");
            List<double> valueList = material.TensionStrain.DamageList;
            List<double> strainList = material.TensionStrain.InelasticStrainList;
            GetValueTable(strainList, valueList);
        }

        private void AddCdpCompressionDamage()
        {
            builder.AddRaw("");
            builder.AddComment("Compression damage (damage, inelastic strain)");
            builder.AddKeyword($"{MaterialVariableName}.concreteDamagedPlasticity.ConcreteCompressionDamage(");
            builder.AddKeyword("    table=(");
            List<double> valueList = material.CompressionStrain.DamageList;
            List<double> strainList = material.CompressionStrain.InelasticStrainList;
            GetValueTable(strainList, valueList);
        }

        private void AddCdpTension()
        {
            builder.AddRaw("");
            builder.AddComment("Tension stiffening (stress, cracking strain))");
            builder.AddKeyword($"{MaterialVariableName}.concreteDamagedPlasticity.ConcreteTensionStiffening(");
            builder.AddKeyword("    table=(");
            List<double> valueList = material.TensionStrain.StressList;
            List<double> strainList = material.TensionStrain.InelasticStrainList;
            GetValueTable(strainList, valueList, stressFactor);
        }

        private void AddCdpCompression()
        {
            builder.AddRaw("");
            builder.AddComment("Compression hardening (stress, inelastic strain)");
            builder.AddKeyword($"{MaterialVariableName}.concreteDamagedPlasticity.ConcreteCompressionHardening(");
            builder.AddKeyword("    table=(");
            List<double> valueList = material.CompressionStrain.StressList;
            List<double> strainList = material.CompressionStrain.InelasticStrainList;
            GetValueTable(strainList, valueList, stressFactor);
        }

        private void GetValueTable(List<double> strainList, List<double> valueList, string? factor = null)
        {
            int count = strainList.Count;
            string factorString;
            if (factor != null)
            {
                factorString = $" * {factor}";
            }
            else
            {
                factorString = string.Empty ;
            }
            for (int i = 0; i < count; i++)
            {
                string keyword = $"        ({FormatConverter.FormatDouble(valueList[i])}{factorString}, {FormatConverter.FormatDouble(strainList[i])})";
                if (i < count - 1)
                {
                    keyword += ",";
                }
                builder.AddKeyword(keyword);
            }
            builder.AddKeyword(")) #end of concrete property table");
        }

        private void AddCdpProperties()
        {
            builder.AddComment("Concrete Damaged Plasticity");
            builder.AddKeyword($"{MaterialVariableName}.ConcreteDamagedPlasticity(");
            builder.AddKeyword("    table=(");
            builder.AddKeyword($"          ({FormatConverter.FormatDouble(material.CdpProperty.DilationAngle)}, #Dilation angle");
            builder.AddKeyword($"          {FormatConverter.FormatDouble(material.CdpProperty.Eccentricity)}, #Eccentricity");
            builder.AddKeyword($"          {FormatConverter.FormatDouble(material.CdpProperty.Fb0Ratio)}, #Fb0 / Fc0 ratio");
            builder.AddKeyword($"          {FormatConverter.FormatDouble(material.CdpProperty.KRatio)}, #K");
            builder.AddKeyword($"          {FormatConverter.FormatDouble(material.CdpProperty.Viscosity)}) #Viscosity");
            builder.AddKeyword("    , )) #end of Concrete damage plasticity");
        }
    }
}
