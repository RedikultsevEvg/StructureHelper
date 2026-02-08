using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Shapes;
using System;
using System.ComponentModel;

namespace StructureHelper.Windows.Shapes
{
    public class VerticalTShapeViewModel : ViewModelBase, IDataErrorInfo
    {
        private const double MinFullHeight = 0.05;
        private const double MinWebWidth = 0.005;
        private const double MinFlangeHeight = 0.005;
        private IVerticalTShape _verticalTShape;

        public double FullHeight
        {
            get => _verticalTShape.FullHeight;
            set
            {
                _verticalTShape.FullHeight = value;
                Refresh();
            }
        }
        public double WebWidth
        {
            get => _verticalTShape.WebWidth;
            set
            {
                _verticalTShape.WebWidth = value;
                Refresh();
            }
        }
        public double FlangeHeight
        {
            get => _verticalTShape.FlangeHeight;
            set
            {
                _verticalTShape.FlangeHeight = value;
                Refresh();
            }
        }
        public double FlangeWidth
        {
            get => _verticalTShape.FlangeWidth;
            set
            {
                _verticalTShape.FlangeWidth = value;
                Refresh();
            }
        }
        public double FlangeXOffset
        {
            get => _verticalTShape.FlangeXOffset;
            set
            {
                _verticalTShape.FlangeXOffset = value;
                Refresh();
            }
        }
        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = null;
                if (columnName == nameof(FullHeight))
                {
                    if (FullHeight <= MinFullHeight)
                    {
                        error = $"Full height must be greater than {MinFullHeight}(m)";
                    }
                }
                if (columnName == nameof(WebWidth))
                {
                    if (WebWidth <= MinWebWidth)
                    {
                        error = $"Width of web must be greater than {MinWebWidth}(m)";
                    }
                }
                if (columnName == nameof(FlangeHeight))
                {
                    if (FlangeHeight <= MinFlangeHeight)
                    {
                        error = $"Height of flange must be greater than {MinFlangeHeight}(m), but was {FlangeHeight}(m)";
                    }
                    if (FlangeHeight >= FullHeight)
                    {
                        error = $"Height of flange must be less than full height {FullHeight}(m), but was {FlangeHeight}(m)";
                    }
                }
                if (columnName == nameof(FlangeWidth))
                {
                    if (FlangeWidth <= WebWidth)
                    {
                        error = $"Width of flange must be greater than width of web {WebWidth}(m), but was {FlangeWidth}(m)";
                    }
                }
                return error;
            }
        }

        public VerticalTShapeViewModel(IVerticalTShape verticalTShape)
        {
            _verticalTShape = verticalTShape;
        }

        private void Refresh()
        {
            OnPropertyChanged(nameof(FullHeight));
            OnPropertyChanged(nameof(WebWidth));
            OnPropertyChanged(nameof(FlangeHeight));
            OnPropertyChanged(nameof(FlangeWidth));
            OnPropertyChanged(nameof(FlangeXOffset));
        }
    }
}
