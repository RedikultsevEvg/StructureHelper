using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.FeaMaterials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StructureHelper.Windows.FeaMaterials
{
    public class ConcreteFeaMaterialViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        IConcreteFeaMaterial material;

        public string Name
        {
            get => material.Name;
            set
            {
                material.Name = value;
                Refresh();
            }
        }

        public double YoungsModulus
        {
            get => material.YoungModulus;
            set
            {
                material.YoungModulus = value;
                Refresh();
            }
        }
        public double PoissonsRatio
        {
            get => material.PoissonRatio;
            set
            {
                material.PoissonRatio = value;
                Refresh();
            }
        }

        public CdpPropertyViewModel CdpProperty { get; }
        public ConcreteFeaCompressionViewModel Compression { get; }
        public ConcreteFeaTensionViewModel Tension { get; }

        public ConcreteFeaMaterialViewModel(IConcreteFeaMaterial material)
        {
            this.material = material;
            CdpProperty = new(this, material.CdpProperty);
            Compression = new(this, material.CompressionProperties);
            Tension = new(this, material.TensionProperties);
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                if (columnName == nameof(YoungsModulus))
                {
                    if (YoungsModulus <= 0)
                    {
                        error = $"Young's modulus must be positive, but was {YoungsModulus}(Pa)";
                    }
                }
                if (columnName == nameof(PoissonsRatio))
                {
                    if (PoissonsRatio <= 0)
                    {
                        error = $"Poisson ratio must be positive, but was {PoissonsRatio}(dimensionless)";
                    }
                }

                if (error != string.Empty)
                {
                    IsOkAvailable = false;
                }
                else
                {
                    IsOkAvailable = true;
                }
                return error;
            }
        }

        private void Refresh()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(YoungsModulus));
            OnPropertyChanged(nameof(PoissonsRatio));
        }

    }
}
