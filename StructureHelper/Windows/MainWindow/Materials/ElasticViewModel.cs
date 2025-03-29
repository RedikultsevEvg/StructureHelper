using LoaderCalculator.Data.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal class ElasticViewModel : HelperMaterialViewModel
    {
        IElasticMaterial material;
        SafetyFactorsViewModel safetyFactorsViewModel;
        public SafetyFactorsViewModel SafetyFactors => safetyFactorsViewModel;
        public double Modulus
        {
            get => material.Modulus;
            set
            {
                material.Modulus = value;
                OnPropertyChanged(nameof(Modulus));
            }
        }
        public double CompressiveStrength
        {
            get => material.CompressiveStrength;
            set
            {
                material.CompressiveStrength = value;
                OnPropertyChanged(nameof(CompressiveStrength));
            }
        }
        public double TensileStrength
        {
            get => material.TensileStrength;
            set
            {
                material.TensileStrength = value;
                OnPropertyChanged(nameof(TensileStrength));
            }
        }

        public ElasticViewModel(IElasticMaterial material)
        {
            this.material = material;
            safetyFactorsViewModel = new SafetyFactorsViewModel(material.SafetyFactors);
        }
    }
}
