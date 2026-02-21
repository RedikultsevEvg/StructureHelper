using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace StructureHelper.Windows.Shapes
{
    public class VerticalDoubleTShapeViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private const double MinFullHeight = 0.05;
        private const double MinWebWidth = 0.005;
        private const double MinFlangeHeight = 0.005;

        private IVerticalDoubleTShape shape;

        public double FullHeight
        {
            get => shape.FullHeight;
            set
            {
                shape.FullHeight = value;
                Refresh();
            }
        }



        public double TopFlangeWidth
        {
            get => shape.TopFlangeWidth;
            set
            {
                shape.TopFlangeWidth = value;
                Refresh();
            }
        }
        public double TopFlangeThickness
        {
            get => shape.TopFlangeThickness;
            set
            {
                shape.TopFlangeThickness = value;
                Refresh();
            }
        }
        public double BottomFlangeWidth
        {
            get => shape.BottomFlangeWidth;
            set
            {
                shape.BottomFlangeWidth = value;
                Refresh();
            }
        }
        public double BottomFlangeThickness
        {
            get => shape.BottomFlangeThickness;
            set
            {
                shape.BottomFlangeThickness = value;
                Refresh();
            }
        }
        public double WebThickness
        {
            get => shape.WebThickness;
            set
            {
                shape.WebThickness = value;
                Refresh();
            }
        }

        public VerticalDoubleTShapeViewModel(IVerticalDoubleTShape doubleTShape)
        {
            this.shape = doubleTShape;
        }

        public string Error => null;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                if (columnName == nameof(FullHeight))
                {
                    if (FullHeight <= MinFullHeight)
                    {
                        error = $"Full height must be greater than {MinFullHeight}(m)";
                    }
                }
                if (columnName == nameof(WebThickness))
                {
                    if (WebThickness <= MinWebWidth)
                    {
                        error = $"Width of web must be greater than {MinWebWidth}(m)";
                    }
                }
                if (columnName == nameof(TopFlangeThickness))
                {
                    if (TopFlangeThickness <= MinFlangeHeight)
                    {
                        error = $"Height of top flange must be greater than {MinFlangeHeight}(m), but was {TopFlangeThickness}(m)";
                    }
                    if (TopFlangeThickness >= FullHeight)
                    {
                        error = $"Height of top flange must be less than full height {FullHeight}(m), but was {TopFlangeThickness}(m)";
                    }
                }
                if (columnName == nameof(BottomFlangeThickness))
                {
                    if (BottomFlangeThickness <= MinFlangeHeight)
                    {
                        error = $"Height of bottom flange must be greater than {MinFlangeHeight}(m), but was {BottomFlangeThickness}(m)";
                    }
                    if (BottomFlangeThickness >= FullHeight)
                    {
                        error = $"Height of bottom flange must be less than full height {FullHeight}(m), but was {BottomFlangeThickness}(m)";
                    }
                }
                if (columnName == nameof(TopFlangeWidth))
                {
                    if (TopFlangeWidth <= WebThickness)
                    {
                        error = $"Width of top flange must be greater than width of web {WebThickness}(m), but was {TopFlangeWidth}(m)";
                    }
                }
                if (columnName == nameof(BottomFlangeWidth))
                {
                    if (BottomFlangeWidth <= WebThickness)
                    {
                        error = $"Width of bottom flange must be greater than width of web {WebThickness}(m), but was {BottomFlangeWidth}(m)";
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
            OnPropertyChanged(nameof(FullHeight));
            OnPropertyChanged(nameof(WebThickness));
            OnPropertyChanged(nameof(TopFlangeThickness));
            OnPropertyChanged(nameof(TopFlangeWidth));
            OnPropertyChanged(nameof(BottomFlangeThickness));
            OnPropertyChanged(nameof(BottomFlangeWidth));
        }
    }
}
