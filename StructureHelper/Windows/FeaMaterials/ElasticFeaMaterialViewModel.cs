using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.FeaMaterials;
using System.ComponentModel;

namespace StructureHelper.Windows.FeaMaterials
{
    public class ElasticFeaMaterialViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private IElasticFeaMaterial _material;

        public string Name
        {
            get => _material.Name;
            set
            {
                _material.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public double YoungsModulus
        {
            get => _material.YoungsModulus;
            set
            {
                _material.YoungsModulus = value;
                OnPropertyChanged(nameof(YoungsModulus));
            }
        }
        public double PoissonsRatio
        {
            get => _material.PoissonsRatio;
            set
            {
                _material.PoissonsRatio = value;
                OnPropertyChanged(nameof(PoissonsRatio));
            }
        }

        public ElasticFeaMaterialViewModel(IElasticFeaMaterial material)
        {
            _material = material;
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
                        error = $"Young's modulus must be positive, but was {YoungsModulus}(m)";
                    }
                }
                if (columnName == nameof(PoissonsRatio))
                {
                    if (PoissonsRatio <= 0)
                    {
                        error = $"Poisson ratio must be positive, but was {PoissonsRatio}(m)";
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
    }
}
