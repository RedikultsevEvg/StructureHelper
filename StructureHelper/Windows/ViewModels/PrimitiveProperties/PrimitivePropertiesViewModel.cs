using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Models.Materials;
using StructureHelper.Windows.MainWindow.Materials;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using PointViewPrimitive = StructureHelper.Infrastructure.UI.DataContexts.PointViewPrimitive;
using RectangleViewPrimitive = StructureHelper.Infrastructure.UI.DataContexts.RectangleViewPrimitive;

namespace StructureHelper.Windows.ViewModels.PrimitiveProperties
{
    public class PrimitivePropertiesViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private PrimitiveBase primitive;
        private ICrossSectionRepository sectionRepository;

        public ICommand EditColorCommand { get; private set; }
        public ICommand EditMaterialCommand { get; private set; }

        public ObservableCollection<IHeadMaterial> HeadMaterials { get; private set; }
        public ObservableCollection<PrimitiveBase> HostPrimitives { get; private set; }

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
        public PrimitiveBase? HostPrimitive
        {
            get
            {
                if (primitive is not IHasHostPrimitive)
                {
                    return null;
                }
                else 
                {
                    var sPrimitive = ((IHasHostPrimitive)primitive).HostPrimitive;
                    if (sPrimitive is null) { return null; }
                    foreach (var item in HostPrimitives)
                    {
                        if (item.GetNdmPrimitive() == sPrimitive)
                        {
                            return item;
                        }
                    }
                    return null;
                }
            }
            set
            {
                if (value is not null)
                {
                    if (primitive is IHasHostPrimitive)
                    {
                        var sPrimitive = value.GetNdmPrimitive();
                        ((IHasHostPrimitive)primitive).HostPrimitive = sPrimitive;
                        OnPropertyChanged(nameof(HostPrimitive));
                    }
                    else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $", Actual type: {value.GetType()}");
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
        public bool Triangulate
        {
            get { return primitive.Triangulate; }
            set
            {
                primitive.Triangulate = value;
                OnPropertyChanged(nameof(Triangulate));
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

        public double AutoPrestrainKx => primitive.AutoPrestrainKx;
        public double AutoPrestrainKy => primitive.AutoPrestrainKy;
        public double AutoPrestrainEpsZ => primitive.AutoPrestrainEpsZ;

        public HasDivisionViewModel DivisionViewModel => primitive.DivisionViewModel;

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
        public double Diameter
        {
            get
            {
                if (primitive is CircleViewPrimitive)
                {
                    var shape = primitive as CircleViewPrimitive;
                    return shape.Diameter;
                }
                return 0d;
            }
            set
            {
                if (primitive is CircleViewPrimitive)
                {
                    var shape = primitive as CircleViewPrimitive;
                    shape.Diameter = value;
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
        public bool IsVisible
        {
            get => primitive.IsVisible;
            set
            {
                primitive.IsVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }
        public double Opacity
        {
            get => primitive.Opacity * 100d;
            set
            {
                if (value < 0d ) { value = 0d; }
                if (value > 100d) { value = 100d; }
                primitive.Opacity = value / 100d;
                OnPropertyChanged(nameof(Opacity));
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

        public PrimitivePropertiesViewModel(PrimitiveBase primitive, ICrossSectionRepository sectionRepository)
        {
            this.primitive = primitive;
            this.sectionRepository = sectionRepository;
            HeadMaterials = new ObservableCollection<IHeadMaterial>();
            foreach (var material in sectionRepository.HeadMaterials)
            {
                HeadMaterials.Add(material);
            }
            EditColorCommand = new RelayCommand(o => EditColor(), o => !SetMaterialColor);
            EditMaterialCommand = new RelayCommand(o => EditMaterial());
            HostPrimitives = new ObservableCollection<PrimitiveBase>();
            foreach (var item in sectionRepository.Primitives)
            {
                if (item is RectangleNdmPrimitive || item is EllipseNdmPrimitive)
                {
                    CheckHost(primitive, item);
                    HostPrimitives.Add(PrimitiveOperations.ConvertNdmPrimitiveToPrimitiveBase(item));
                }
            }
        }

        private void CheckHost(PrimitiveBase primitive, INdmPrimitive item)
        {
            var ndm = primitive.GetNdmPrimitive();
            if (ndm is RebarNdmPrimitive)
            {
                var host = item as IHasDivisionSize;
                var reinforcement = ndm as RebarNdmPrimitive;
                bool checkWhenPointLocatedInsideOfItsHost = host.IsPointInside(reinforcement.Center.Clone() as IPoint2D);
                if (checkWhenPointLocatedInsideOfItsHost
                    && reinforcement.HostPrimitive is null)
                {
                    var dialogResult = MessageBox.Show($"Primitive {reinforcement.Name} is inside primitive {item.Name}",
                        "Assign new host?", 
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        reinforcement.HostPrimitive = item;
                    }
                }

            }
        }

        private void EditMaterial()
        {
            var wnd = new HeadMaterialsView(sectionRepository);
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
