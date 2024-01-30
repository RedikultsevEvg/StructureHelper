using StructureHelperCommon.Models.Materials.Libraries;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Materials
{
    internal class FRViewModel : ElasticViewModel
    {
        IFRMaterial material;
        public double ULSConcreteStrength
        {
            get
            {
                return material.ULSConcreteStrength;
            }

            set
            {
                material.ULSConcreteStrength = value;
                OnPropertyChanged(nameof(ULSConcreteStrength));
            }
        }
        public double SumThickness
        {
            get
            {
                return material.SumThickness;
            }

            set
            {
                material.SumThickness = value;
                OnPropertyChanged(nameof(SumThickness));
            }
        }
        public double GammaF2
        {
            get => material.GammaF2;
            set {}
        }
        public FRViewModel(IFRMaterial material) : base(material)
        {
            this.material = material;
        }
    }
}
