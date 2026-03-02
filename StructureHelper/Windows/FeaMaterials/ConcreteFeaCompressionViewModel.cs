using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.FeaMaterials;
using System.ComponentModel;

namespace StructureHelper.Windows.FeaMaterials
{
    public class ConcreteFeaCompressionViewModel : ViewModelBase, IDataErrorInfo
    {
        private const double maxRatio = 0.9;
        private IConcreteFeaCompression material;
        private double descendingScaleFactor;

        public ConcreteFeaMaterialViewModel ParentViewModel {  get; set; }

        public double Strength
        {
            get => material.Strength;
            set
            {
                material.Strength = value;
                OnPropertyChanged(nameof(Strength));
            }
        }


        public double PeakStrain
        {
            get => material.PeakStrain;
            set
            {
                material.PeakStrain = value;
                OnPropertyChanged(nameof(PeakStrain));
            }
        }
        public double ElasticStressRatio
        {
            get => material.ElasticStressRatio;
            set
            {
                material.ElasticStressRatio = value;
                OnPropertyChanged(nameof(ElasticStressRatio));
            }
        }

        public double DescendingScaleFactor
        {
            get => material.DescendingScaleFactor;
            set
            {
                material.DescendingScaleFactor = value;
            }
        }

        public ConcreteFeaCompressionViewModel(ConcreteFeaMaterialViewModel concreteFeaMaterialViewModel, IConcreteFeaCompression material)
        {
            this.material = material;
            ParentViewModel = concreteFeaMaterialViewModel;
        }
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                if (columnName == nameof(Strength))
                {
                    if (Strength <= 0)
                    {
                        error = $"Strength of concrete must be positive, but was {Strength}";
                    }
                }
                if (columnName == nameof(PeakStrain))
                {
                    if (PeakStrain <= 0)
                    {
                        error = $"Peak strain must be positive, but was {PeakStrain}";
                    }
                }
                if (columnName == nameof(ElasticStressRatio))
                {
                    if (ElasticStressRatio <= 0)
                    {
                        error = $"Ratio of elastic stress to peak stress must be positive, but was {ElasticStressRatio}";
                    }
                    if (ElasticStressRatio >= maxRatio)
                    {
                        error = $"Ratio of elastic stress to peak stress must be less than {maxRatio}, but was {ElasticStressRatio}";
                    }
                }

                if (columnName == nameof(DescendingScaleFactor))
                {
                    if (DescendingScaleFactor <= 0.01)
                    {
                        error = $"Factor for descending branch must be positive, but was {DescendingScaleFactor}";
                    }
                }

                if (error != string.Empty)
                {
                    ParentViewModel?.IsOkAvailable = false;
                }
                else
                {
                    ParentViewModel?.IsOkAvailable = true;
                }
                return error;
            }
        }
    }
}
