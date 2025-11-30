using HelixToolkit.SharpDX;
using HelixToolkit.Wpf.SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace StructureHelper.Infrastructure
{
    public class HelixViewModelBase : ViewModelBase
    {
        private string subTitle = string.Empty;
        private string title = string.Empty;
        private DefaultEffectsManager effectsManager;
        private HelixToolkit.Wpf.SharpDX.Camera camera;
        private Color ambientLightColor;
        private Viewport3DX viewPort3D;
        private Vector3D directionalLightDirection;
        private Stream? backgroundTexture;

        public HelixToolkit.Wpf.SharpDX.Camera Camera
        {
            get => camera;
            set
            {
                camera = value;
                OnPropertyChanged(nameof(Camera));
            }
        }
        public System.Windows.Media.Color AmbientLightColor
        {
            get => ambientLightColor;
            set
            {
                ambientLightColor = value;
                OnPropertyChanged(nameof(AmbientLightColor));
            }
        }
        public System.Windows.Media.Color DirectionalLightColor { get; set; }
        public Vector3D DirectionalLightDirection
        {
            get => directionalLightDirection;
            set
            {
                directionalLightDirection = value;
                OnPropertyChanged(nameof(DirectionalLightDirection)); 
            }
        }
        public Stream? BackgroundTexture
        {
            get => backgroundTexture;
            set
            {
                backgroundTexture = value;
                OnPropertyChanged(nameof(BackgroundTexture));
            }
        }


        public Viewport3DX Viewport3D
        {
            get => viewPort3D;
            set
            {
                viewPort3D = value;
                OnPropertyChanged(nameof(Viewport3DX));
            }
        }
        public DefaultEffectsManager EffectsManager
        {
            get => effectsManager;
            set
            {
                effectsManager = value;
                OnPropertyChanged(nameof(EffectsManager));
            }
        }
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        public string SubTitle
        {
            get => subTitle;
            set
            {
                subTitle = value;
                OnPropertyChanged(nameof(SubTitle));
            }
        }

        public HelixViewModelBase()
        {
            EffectsManager = new DefaultEffectsManager();

            Camera = new HelixToolkit.Wpf.SharpDX.OrthographicCamera
            {
                Position = new Point3D(3, 3, 5),
                LookDirection = new Vector3D(-3, -3, -5),
                UpDirection = new Vector3D(0, 1, 0),
                FarPlaneDistance = 50000
            };

            // setup lighting            
            AmbientLightColor = Colors.DimGray;
            DirectionalLightColor = Colors.White;
            DirectionalLightDirection = new Vector3D(-2, -5, -2);

            BackgroundTexture =
    BitmapExtensions.CreateLinearGradientBitmapStream(EffectsManager, 128, 128, Direct2DImageFormat.Bmp,
    new Vector2(0, 0), new Vector2(0, 128), new SharpDX.Direct2D1.GradientStop[]
    {
                    new(){ Color = Colors.White.ToRawColor4(), Position = 0f },
                    new(){ Color = Colors.DarkGray.ToRawColor4(), Position = 1f }
    });
        }
    }
}
