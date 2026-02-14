using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Shapes;
using System;
using System.ComponentModel;

namespace StructureHelper.Windows.Shapes
{
    public class RingShapeViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private const double innerDiameterMinValue = 1e-3;
        private IRingShape ringShape;

        public double OuterDiameter
        {
            get => ringShape.OuterDiameter;
            set
            {
                ringShape.OuterDiameter = value;
                Refresh();
            }
        }

        private void Refresh()
        {
            OnPropertyChanged(nameof(OuterDiameter));
            OnPropertyChanged(nameof(InnerDiameter));
            OnPropertyChanged(nameof(OuterRadius));
            OnPropertyChanged(nameof(InnerRadius));
        }

        public double InnerDiameter
        {
            get => ringShape.InnerDiameter;
            set
            {
                ringShape.InnerDiameter = value;
                Refresh();
            }
        }
        public double OuterRadius
        {
            get
            {
                return ringShape.OuterRadius;
            }
            set
            {
                ringShape.OuterRadius = value;
                Refresh();
            }
        }

        public double InnerRadius
        {
            get
            {
                return ringShape.InnerRadius;
            }
            set
            {
                ringShape.InnerRadius = value;
                Refresh();
            }
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                if (columnName == nameof(OuterDiameter))
                {
                    if (OuterDiameter <= InnerDiameter)
                    {
                        error = $"Outer Diameter must be greater than Inner Diameter, but was {OuterDiameter} <= {InnerDiameter}(m)";
                    }
                }
                if (columnName == nameof(InnerDiameter))
                {
                    if (InnerDiameter <= innerDiameterMinValue)
                    {
                        error = $"Inner Diameter must be greater than {innerDiameterMinValue}(m)";
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

        public RingShapeViewModel(IRingShape ringShape)
        {
            this.ringShape = ringShape;
        }
    }
}
