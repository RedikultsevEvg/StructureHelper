using StructureHelper.Models.Materials;
using StructureHelper.Services.Primitives;
using StructureHelper.Windows.MainWindow;
using StructureHelper.Windows.ViewModels.NdmCrossSections;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Windows.Input;
using System.Windows.Media;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public abstract class PrimitiveBase : ViewModelBase, IObserver<IRectangleShape>
    {
        #region Поля
        private IPrimitiveRepository primitiveRepository;
        private INdmPrimitive primitive;
        private bool captured, parameterCaptured, elementLock, paramsPanelVisibilty, popupCanBeClosed = true, borderCaptured;
        private double showedOpacity = 0, x, y, xY1, yX1, primitiveWidth, primitiveHeight;
        protected double delta = 0.5;
        private int showedZIndex = 1;

        #endregion

        #region Свойства
        public HasDivisionViewModel DivisionViewModel { get; set; }
        public INdmPrimitive NdmPrimitive
        {
            get => primitive;
        }
        public IPrimitiveRepository PrimitiveRepository => primitiveRepository;
        
        public string Name
        {
            get => primitive.Name;
            set
            {
                primitive.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public double CenterX
        {
            get => primitive.Center.X;
            set
            {
                primitive.Center.X = value;
                OnPropertyChanged(nameof(CenterX));
            }
        }
        public double CenterY
        {
            get => primitive.Center.Y;
            set
            {
                primitive.Center.Y = value;
                OnPropertyChanged(nameof(CenterY));
                OnPropertyChanged(nameof(InvertedCenterY));
            }
        }
        public bool Triangulate
        {
            get => primitive.NdmElement.Triangulate;
            set
            {
                primitive.NdmElement.Triangulate = value;
                OnPropertyChanged(nameof(Triangulate));
            }
        }
        public double InvertedCenterY => - CenterY;
        public double PrestrainKx
        {   get => primitive.NdmElement.UsersPrestrain.Mx;
            set
            {
                primitive.NdmElement.UsersPrestrain.Mx = value;
                OnPropertyChanged(nameof(PrestrainKx));
            }
        }
        public double PrestrainKy
        {   get => primitive.NdmElement.UsersPrestrain.My;
            set
            {
                primitive.NdmElement.UsersPrestrain.My = value;
                OnPropertyChanged(nameof(PrestrainKy));
            }
        }
        public double PrestrainEpsZ
        {   get => primitive.NdmElement.UsersPrestrain.Nz;
            set
            {
                primitive.NdmElement.UsersPrestrain.Nz = value;
                OnPropertyChanged(nameof(PrestrainEpsZ));
            }
        }

        public double AutoPrestrainKx => primitive.NdmElement.AutoPrestrain.Mx;
        public double AutoPrestrainKy => primitive.NdmElement.AutoPrestrain.My;
        public double AutoPrestrainEpsZ => primitive.NdmElement.AutoPrestrain.Nz;

        public IHeadMaterial HeadMaterial
        {
            get => primitive.NdmElement.HeadMaterial;
            set
            {
                primitive.NdmElement.HeadMaterial = value;
                OnPropertyChanged(nameof(HeadMaterial));
                OnPropertyChanged(nameof(Color));
            }
        }

        public bool SetMaterialColor
        {
            get => primitive.VisualProperty.SetMaterialColor;
            set
            {
                primitive.VisualProperty.SetMaterialColor = value;
                OnPropertyChanged(nameof(Color));
            }

        }
        public Color Color
        {
            get => ((primitive.VisualProperty.SetMaterialColor == true)
                & (primitive.NdmElement.HeadMaterial !=null))? primitive.NdmElement.HeadMaterial.Color : primitive.VisualProperty.Color;
            set
            {
                SetMaterialColor = false;
                primitive.VisualProperty.Color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public bool Captured
        {
            set => OnPropertyChanged(value, ref captured);
            get => captured;
        }
        public bool ParameterCaptured
        {
            set => OnPropertyChanged(value, ref parameterCaptured);
            get => parameterCaptured;
        }
        public bool ElementLock
        {
            get => elementLock;
            set => OnPropertyChanged(value, ref elementLock);
        }

        public bool ParamsPanelVisibilty
        {
            get => paramsPanelVisibilty;
            set => OnPropertyChanged(value, ref paramsPanelVisibilty);
        }
        public bool PopupCanBeClosed
        {
            get => popupCanBeClosed;
            set => OnPropertyChanged(value, ref popupCanBeClosed);
        }
        public double ShowedOpacity
        {
            get => showedOpacity;
            set
            {
                Opacity = (100 - value) / 100;
                OnPropertyChanged(nameof(Opacity));
                OnPropertyChanged(value, ref showedOpacity);
            }
        }

        public bool IsVisible
        {
            get => primitive.VisualProperty.IsVisible;
            set
            {
                primitive.VisualProperty.IsVisible = value;
                OnPropertyChanged(nameof(IsVisible));
            }
        }

        public double Opacity
        {
            get => primitive.VisualProperty.Opacity;
            set
            {
                primitive.VisualProperty.Opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }
        public int ShowedZIndex
        {
            get => showedZIndex;
            set
            {
                ZIndex = value - 1;
                OnPropertyChanged(nameof(ZIndex));
                OnPropertyChanged(value, ref showedZIndex);
            }
        }
        public int ZIndex
        {
            get => primitive.VisualProperty.ZIndex;
            set
            {
                primitive.VisualProperty.ZIndex = value;
                OnPropertyChanged(nameof(ZIndex));
            }
        }

        public double Xy1
        {
            get => xY1;
            set => OnPropertyChanged(value, ref xY1);
        }
        public double Yx1
        {
            get => yX1;
            set => OnPropertyChanged(value, ref yX1);
        }
        public virtual double PrimitiveWidth { get; set; }
        public virtual double PrimitiveHeight { get;set; }  
        public bool BorderCaptured
        {
            get => borderCaptured;
            set => OnPropertyChanged(value, ref borderCaptured);
        }
        #endregion

        #region Команды
        public ICommand PrimitiveLeftButtonDown { get; }
        public ICommand PrimitiveLeftButtonUp { get; }
        public ICommand PreviewMouseMove { get; protected set; }
        public ICommand PrimitiveDoubleClick { get; }

        #endregion

        public PrimitiveBase(INdmPrimitive primitive)
        {
            this.primitive = primitive;
        }

        public CrossSectionViewModel OwnerVM { get; private set; }

        public double DeltaX { get; private set; }
        public double DeltaY { get; private set; }

        public virtual INdmPrimitive GetNdmPrimitive()
        {
            //RefreshNdmPrimitive();
            return primitive;
        }

        public virtual void Refresh()
        {
            OnPropertyChanged(nameof(Name));
            OnPropertyChanged(nameof(Color));
            OnPropertyChanged(nameof(CenterX));
            OnPropertyChanged(nameof(CenterY));
            OnPropertyChanged(nameof(InvertedCenterY));
            OnPropertyChanged(nameof(SetMaterialColor));
            OnPropertyChanged(nameof(Triangulate));
            OnPropertyChanged(nameof(PrimitiveWidth));
            OnPropertyChanged(nameof(PrimitiveHeight));
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(IRectangleShape value)
        {
            DeltaX = value.Width / 2d;
            DeltaY = value.Height / 2d;
            Refresh();
        }
    }
}
