using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.ColorPickerWindow;
using StructureHelper.Windows.MainWindow.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using PointViewPrimitive = StructureHelper.Infrastructure.UI.DataContexts.PointViewPrimitive;
using RectangleViewPrimitive = StructureHelper.Infrastructure.UI.DataContexts.RectangleViewPrimitive;

namespace StructureHelper.Windows.ViewModels.PrimitiveProperties
{
    public class PrimitivePropertiesViewModel : ViewModelBase, IDataErrorInfo
    {
        private PrimitiveBase primitive;
        private IHasHeadMaterials hasHeadMaterials;

        public ICommand EditColorCommand { get; private set; }
        public ICommand EditMaterialCommand { get; private set; }

        public ObservableCollection<IHeadMaterial> HeadMaterials { get; private set; }

        public string Name
        {
            get => primitive.Name;
            set
            {
                primitive.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public IHeadMaterial PrimitiveMaterial
        { get => primitive.HeadMaterial;
            set
            {
                primitive.HeadMaterial = value;
                OnPropertyChanged(nameof(PrimitiveMaterial));
                if (primitive.SetMaterialColor == true)
                {
                    OnPropertyChanged(nameof(Color));
                }
            }
        }

        public double CenterX
        {
            get => primitive.CenterX;
            set
            {
                primitive.CenterX = value;
                OnPropertyChanged(nameof(CenterX));
            }

        }
        public double CenterY
        {
            get => primitive.CenterY;
            set
            {
                primitive.CenterY = value;
                OnPropertyChanged(nameof(CenterY));
            }
        }

        public double PrestrainKx
        {
            get => primitive.PrestrainKx;
            set => primitive.PrestrainKx = value;
        }
        public double PrestrainKy
        {
            get => primitive.PrestrainKy;
            set => primitive.PrestrainKy = value;
        }
        public double PrestrainEpsZ
        {
            get => primitive.PrestrainEpsZ;
            set => primitive.PrestrainEpsZ = value;
        }

        public int MinElementDivision
        {
            get => (primitive as IHasDivision).NdmMinDivision;
            set
            {
                (primitive as IHasDivision).NdmMinDivision = value; 
                OnPropertyChanged(nameof(MinElementDivision));
            }
        }

        public double MaxElementSize
        {
            get => (primitive as IHasDivision).NdmMaxSize;
            set
            {
                (primitive as IHasDivision).NdmMaxSize = value;
                OnPropertyChanged(nameof(MaxElementSize));
            }
        }

        public double Width
        {
            get
            {
                if (primitive is RectangleViewPrimitive)
                {
                    var shape = primitive as RectangleViewPrimitive;
                    return shape.PrimitiveWidth;
                }
                return 0d;
            }
            set
            {
                if (primitive is RectangleViewPrimitive)
                {
                    var shape = primitive as RectangleViewPrimitive;
                    shape.PrimitiveWidth = value;
                }
                CenterX = CenterX;
            }
        }

        public double Height
        {
            get
            {
                if (primitive is RectangleViewPrimitive)
                {
                    var shape = primitive as RectangleViewPrimitive;
                    return shape.PrimitiveHeight;
                }
                return 0d;
            }
            set
            {
                if (primitive is RectangleViewPrimitive)
                {
                    var shape = primitive as RectangleViewPrimitive;
                    shape.PrimitiveHeight = value;
                }
                CenterY = CenterY; ;
            }
    }

        public double Area
        {
            get
            {
                if (primitive is PointViewPrimitive)
                {
                    var shape = primitive as PointViewPrimitive;
                    return shape.Area;
                }
                return 0d;
            }
            set
            {
                if (primitive is PointViewPrimitive)
                {
                    var shape = primitive as PointViewPrimitive;
                    shape.Area = value;
                    OnPropertyChanged(nameof(Area));
                    OnPropertyChanged(nameof(shape.Diameter));
                }
            }
        }

        public Color Color
        {
            get => primitive.Color;
            set
            {
                primitive.Color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public bool SetMaterialColor
        {
            get => primitive.SetMaterialColor;
            set
            {
                primitive.SetMaterialColor = value;
                OnPropertyChanged(nameof(Color));
                OnPropertyChanged(nameof(SetMaterialColor));
            }
        }

        public int ZIndex
        {   get => primitive.ZIndex;
            set
            {
                primitive.ZIndex = value;
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

        public PrimitivePropertiesViewModel(PrimitiveBase primitive, IHasHeadMaterials hasHeadMaterials)
        {
            this.primitive = primitive;
            this.hasHeadMaterials = hasHeadMaterials;
            HeadMaterials = new ObservableCollection<IHeadMaterial>();
            foreach (var material in hasHeadMaterials.HeadMaterials)
            {
                HeadMaterials.Add(material);
            }
            EditColorCommand = new RelayCommand(o => EditColor(), o => !SetMaterialColor);
            EditMaterialCommand = new RelayCommand(o => EditMaterial());

        }

        private void EditMaterial()
        {
            var wnd = new HeadMaterialsView(hasHeadMaterials);
            wnd.ShowDialog();
        }

        public void EditColor()
        {
            Color color = Color;
            ColorProcessor.EditColor(ref color);
            Color = color;
        }
    }
}
