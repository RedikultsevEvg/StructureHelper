using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Point = StructureHelper.Infrastructure.UI.DataContexts.Point;
using Rectangle = StructureHelper.Infrastructure.UI.DataContexts.Rectangle;

namespace StructureHelper.Windows.ViewModels.PrimitiveProperties
{
    public class PrimitivePropertiesViewModel : ViewModelBase, IDataErrorInfo
    {
        private PrimitiveBase primitive;

        public string Name
        {
            get => primitive.Name;
            set
            {
                primitive.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string MaterialName
        {
            get => primitive.MaterialName;
            set
            {
                primitive.Name = value;
                OnPropertyChanged(nameof(MaterialName));
            }
        }

        public double CenterX
        {
            get => primitive.CenterX;
            set
            {
                primitive.CenterX = value;
                OnPropertyChanged(nameof(CenterX));
                OnPropertyChanged(nameof(primitive.ShowedX));
                OnPropertyChanged(nameof(primitive.X));
            }

        }

        public double CenterY
        {
            get => primitive.CenterY;
            set
            {
                primitive.CenterY = value;
                OnPropertyChanged(nameof(CenterY));
                OnPropertyChanged(nameof(primitive.ShowedY));
                OnPropertyChanged("Y");
            }
        }

        public double Prestrain_Kx
        {
            get => primitive.Pre
        }

        public int MinElementDivision
        {
            get => primitive.MinElementDivision;
            set
            {
                primitive.MinElementDivision = value; 
                OnPropertyChanged(nameof(MinElementDivision));
            }
        }

        public double MaxElementSize
        {
            get => primitive.MaxElementSize;
            set { primitive.MaxElementSize = value; }
        }

        public double Width
        {
            get
            {
                if (primitive is Rectangle)
                {
                    var shape = primitive as Rectangle;
                    return shape.PrimitiveWidth;
                }
                return 0d;
            }
            set
            {
                if (primitive is Rectangle)
                {
                    var shape = primitive as Rectangle;
                    shape.PrimitiveWidth = value;
                }
            }
        }

        public double Height
        {
            get
            {
                if (primitive is Rectangle)
                {
                    var shape = primitive as Rectangle;
                    return shape.PrimitiveHeight;
                }
                return 0d;
            }
            set
            {
                if (primitive is Rectangle)
                {
                    var shape = primitive as Rectangle;
                    shape.PrimitiveHeight = value;
                }
            }
        }

        public double Area
        {
            get
            {
                if (primitive is Point)
                {
                    var shape = primitive as Point;
                    return shape.Area;
                }
                return 0d;
            }
            set
            {
                if (primitive is Point)
                {
                    var shape = primitive as Point;
                    shape.Area = value;
                }
            }
        }

        public string this[string columnName]
        {
            get
            {
                string error = null;

                if (columnName == nameof(Width)
                    ||
                    columnName == nameof(Height))
                {
                    if (this.Width <= 0 || this.Height <=0)
                    {
                        error = "Width and Height of rectangle must be greater than zero";
                    }
                }
                return error;
            }
        }

        public string Error => throw new NotImplementedException();

        public PrimitivePropertiesViewModel(PrimitiveBase primitive)
        {
            this.primitive = primitive;
        }
    }
}
