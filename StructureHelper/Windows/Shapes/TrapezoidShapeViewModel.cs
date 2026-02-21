using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Shapes;
using System.ComponentModel;

namespace StructureHelper.Windows.Shapes
{
    public class TrapezoidShapeViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private const double MinFullHeight = 0.05;
        private const double MinWebWidth = 0.005;

        private ITrapezoidShape trapezoidShape;

        public double Height
        {
            get => trapezoidShape.Height;
            set
            {
                trapezoidShape.Height = value;
                OnPropertyChanged(nameof(Height));
            }
        }
        public double TopBase
        {
            get => trapezoidShape.TopBase;
            set
            {
                trapezoidShape.TopBase = value;
                OnPropertyChanged(nameof(TopBase));
            }
        }
        public double BottomBase
        {
            get => trapezoidShape.BottomBase;
            set
            {
                trapezoidShape.BottomBase = value;
                OnPropertyChanged(nameof(BottomBase));
            }
        }
        public double TopBaseOffset
        {
            get => trapezoidShape.TopBaseOffset;
            set
            {
                trapezoidShape.TopBaseOffset = value;
                OnPropertyChanged(nameof(TopBaseOffset));
            }
        }
        public TrapezoidShapeViewModel(ITrapezoidShape trapezoidShape)
        {
            this.trapezoidShape = trapezoidShape;
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                if (columnName == nameof(Height))
                {
                    if (Height <= MinFullHeight)
                    {
                        error = $"Full height must be greater than {MinFullHeight}(m)";
                    }
                }
                if (columnName == nameof(TopBase))
                {
                    if (TopBase <= MinWebWidth)
                    {
                        error = $"Top base must be greater than {MinWebWidth}(m)";
                    }
                }
                if (columnName == nameof(BottomBase))
                {
                    if (BottomBase <= MinWebWidth)
                    {
                        error = $"Bottom base must be greater than {MinWebWidth}(m)";
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
