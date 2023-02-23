using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Infrastructure.UI.Converters.Units;
using StructureHelper.Models.Materials;
using StructureHelper.Services.Primitives;
using StructureHelper.UnitSystem.Systems;
using StructureHelper.Windows.MainWindow;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Strings;
using StructureHelperLogics.Models.Materials;
using StructureHelperCommon.Services.ColorServices;
using StructureHelperLogics.Models.Primitives;
using System.Windows.Controls;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public abstract class PrimitiveBase : ViewModelBase
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
                OnPropertyChanged(nameof(InvertedCenterY));
            }
        }
        public double InvertedCenterY => - CenterY;
        public double PrestrainKx
        {   get => primitive.UsersPrestrain.Kx;
            set
            {
                primitive.UsersPrestrain.Kx = value;
                OnPropertyChanged(nameof(PrestrainKx));
            }
        }
        public double PrestrainKy
        {   get => primitive.UsersPrestrain.Ky;
            set
            {
                primitive.UsersPrestrain.Ky = value;
                OnPropertyChanged(nameof(PrestrainKy));
            }
        }
        public double PrestrainEpsZ
        {   get => primitive.UsersPrestrain.EpsZ;
            set
            {
                primitive.UsersPrestrain.EpsZ = value;
                OnPropertyChanged(nameof(PrestrainEpsZ));
            }
        }

        public double AutoPrestrainKx => primitive.AutoPrestrain.Kx;
        public double AutoPrestrainKy => primitive.AutoPrestrain.Ky;
        public double AutoPrestrainEpsZ => primitive.AutoPrestrain.EpsZ;

        public IHeadMaterial HeadMaterial
        {
            get => primitive.HeadMaterial;
            set
            {
                primitive.HeadMaterial = value;
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
                & (primitive.HeadMaterial !=null))? primitive.HeadMaterial.Color : primitive.VisualProperty.Color;
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

        public void RegisterDeltas(double dx, double dy)
        {
            DeltaX = dx;
            DeltaY = dy;
        }

        public MainViewModel OwnerVM { get; private set; }

        public double DeltaX { get; private set; }
        public double DeltaY { get; private set; }

        public virtual INdmPrimitive GetNdmPrimitive()
        {
            RefreshNdmPrimitive();
            return primitive;
        }

        public virtual void RefreshNdmPrimitive()
        {
        }

        public void RefreshColor()
        {
            OnPropertyChanged(nameof(Color));
        }
    }
}
