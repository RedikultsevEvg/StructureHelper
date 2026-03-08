using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.FeaMaterials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StructureHelper.Windows.FeaMaterials
{
    public class CdpPropertyViewModel : ViewModelBase, IDataErrorInfo
    {
        private ICdpProperty cdpProperty;

        public ConcreteFeaMaterialViewModel ParentViewModel { get; set; }

        public double DilationAngle
        {
            get => cdpProperty.DilationAngle;
            set
            {
                cdpProperty.DilationAngle = value;
                OnPropertyChanged(nameof(DilationAngle));
            }
        }
        public double Eccentricity
        {
            get => cdpProperty.Eccentricity;
            set
            {
                cdpProperty.Eccentricity = value;
                OnPropertyChanged(nameof(Eccentricity));
            }
        }
        public double Fb0Ratio
        {
            get => cdpProperty.Fb0Ratio;
            set
            {
                cdpProperty.Fb0Ratio = value;
                OnPropertyChanged(nameof(Fb0Ratio));
            }
        }
        public double KRatio
        {
            get => cdpProperty.KRatio;
            set
            {
                cdpProperty.KRatio = value;
                OnPropertyChanged(nameof(KRatio));
            }
        }
        public double Viscosity
        {
            get => cdpProperty.Viscosity;
            set
            {
                cdpProperty.Viscosity = value;
                OnPropertyChanged(nameof(Viscosity));
            }
        }

        public CdpPropertyViewModel(ConcreteFeaMaterialViewModel concreteFeaMaterialViewModel, ICdpProperty cdpProperty)
        {
            this.cdpProperty = cdpProperty;
            ParentViewModel = concreteFeaMaterialViewModel;
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                if (columnName == nameof(DilationAngle))
                {
                    if (DilationAngle <= 0)
                    {
                        error = $"Dilation angle must be positive, but was {DilationAngle}";
                    }
                }
                if (columnName == nameof(Eccentricity))
                {
                    if (Eccentricity <= 0)
                    {
                        error = $"Eccentricity must be positive, but was {Eccentricity}";
                    }
                }
                if (columnName == nameof(Fb0Ratio))
                {
                    if (Fb0Ratio <= 0)
                    {
                        error = $"Fb0 / Fc0 ratio must be positive, but was {Fb0Ratio}";
                    }
                }
                if (columnName == nameof(KRatio))
                {
                    if (KRatio <= 0)
                    {
                        error = $"KRatio must be positive, but was {KRatio}";
                    }
                }
                if (columnName == nameof(Viscosity))
                {
                    if (Viscosity <= 0)
                    {
                        error = $"Viscosity must be positive, but was {Viscosity}";
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
