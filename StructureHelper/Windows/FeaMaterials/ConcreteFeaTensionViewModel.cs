using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.FeaMaterials;
using System.ComponentModel;

namespace StructureHelper.Windows.FeaMaterials
{
    public class ConcreteFeaTensionViewModel : ViewModelBase, IDataErrorInfo
    {
        private readonly IConcreteFeaTension material;

        public ConcreteFeaTensionViewModel(ConcreteFeaMaterialViewModel parentViewModel, IConcreteFeaTension material)
        {
            this.material = material;
            ParentViewModel = parentViewModel;
        }

        public ConcreteFeaMaterialViewModel ParentViewModel { get; set; }

        public double Strength
        {
            get => material.Strength;
            set
            {
                material.Strength = value;
                Refresh();
            }
        }



        public double FractureEnergy
        {
            get => material.FractureEnergy;
            set
            {
                material.FractureEnergy = value;
                Refresh();
            }
        }
        public double FeSize
        {
            get => material.FeSize;
            set
            {
                material.FeSize = value;
                Refresh();
            }
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
                if (columnName == nameof(FractureEnergy))
                {
                    if (FractureEnergy <= 0)
                    {
                        error = $"Fracture energy must be positive, but was {FractureEnergy}";
                    }
                }
                if (columnName == nameof(FeSize))
                {
                    if (FeSize <= 0)
                    {
                        error = $"Ratio of elastic stress to peak stress must be positive, but was {FeSize}";
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

        private void Refresh()
        {
            OnPropertyChanged(nameof(Strength));
            OnPropertyChanged(nameof(FractureEnergy));
            OnPropertyChanged(nameof(FeSize));
        }
    }
}
