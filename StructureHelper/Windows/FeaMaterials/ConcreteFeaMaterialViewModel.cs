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
        private string name;
        private double youngsModulus;
        private double poissonsRatio;

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
            get => material.YoungsModulus;
            set
            {
                material.YoungsModulus = value;
                Refresh();
            }
        }
        public double PoissonsRatio
        {
            get => material.PoissonsRatio;
            set
            {
                material.PoissonsRatio = value;
            }
        }
        public ConcreteFeaMaterialViewModel(IConcreteFeaMaterial material)
        {
            this.material = material;
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

        private void Refresh()
        {
            OnPropertyChanged(nameof(Name));
        }

    }
}
