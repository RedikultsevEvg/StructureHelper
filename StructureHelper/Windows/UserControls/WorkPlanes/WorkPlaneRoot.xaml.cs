using StructureHelper.Windows.Graphs;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StructureHelper.Windows.UserControls.WorkPlanes
{
    /// <summary>
    /// Логика взаимодействия для WorkPlaneRoot.xaml
    /// </summary>
    public partial class WorkPlaneRoot : UserControl
    {
        private Point? _lastPanPoint;
        public SaveCopyFWElementViewModel SaveCopyViewModel { get; } = new();
        public Canvas WorkPlaneCanvas => this.RootCanvas;

        public WorkPlaneRootViewModel? ViewModel
        {
            get => (WorkPlaneRootViewModel?)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(WorkPlaneRootViewModel),
                typeof(WorkPlaneRoot),
                new PropertyMetadata(null));

        public object? OverlayContent
        {
            get => GetValue(OverlayContentProperty);
            set => SetValue(OverlayContentProperty, value);
        }

        public static readonly DependencyProperty OverlayContentProperty =
            DependencyProperty.Register(
                nameof(OverlayContent),
                typeof(object),
                typeof(WorkPlaneRoot),
                new PropertyMetadata(null));

        public WorkPlaneRoot()
        {
            InitializeComponent();
            this.PreviewMouseWheel += WorkPlaneRoot_PreviewMouseWheel;
            this.MouseDown += WorkPlaneRoot_MouseDown;
            this.MouseMove += WorkPlaneRoot_MouseMove;
            this.MouseUp += WorkPlaneRoot_MouseUp;
            SaveCopyViewModel.FrameWorkElement =  BackgroundGrid;
        }


        private void WorkPlaneRoot_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
                _lastPanPoint = e.GetPosition(this);
        }

        private void WorkPlaneRoot_MouseMove(object sender, MouseEventArgs e)
        {
            if (_lastPanPoint.HasValue && e.MiddleButton == MouseButtonState.Pressed)
            {
                var current = e.GetPosition(this);
                var dx = current.X - _lastPanPoint.Value.X;
                var dy = current.Y - _lastPanPoint.Value.Y;

                if (ViewModel is WorkPlaneRootViewModel dc)
                {
                    if(dc.WorkPlaneConfig.ScaleValue != 1.0)
                    {
                        double factor = 1.0 - dc.WorkPlaneConfig.ScaleValue;
                        //factor = Math.Min(factor, 10);
                        //factor = Math.Max(factor, 0.1);
                        dx /= factor;
                        dy /= factor;
                    }

                    dc.WorkPlaneConfig.ZoomCenterX += dx;
                    dc.WorkPlaneConfig.ZoomCenterY -= dy;
                }

                _lastPanPoint = current;
            }
        }

        private void WorkPlaneRoot_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _lastPanPoint = null;
        }


        private void WorkPlaneRoot_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            //if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                var pos = e.GetPosition(this);

                if (ViewModel is WorkPlaneRootViewModel dc)
                {
                    double zoomFactor = e.Delta > 0 ? 1.1 : 0.9;
                    double actualHeight = WorkPlaneGrid.ActualHeight;
                    double actualWidth = WorkPlaneGrid.ActualWidth;
                    dc.WorkPlaneConfig.ZoomAt(actualHeight, actualWidth, pos, zoomFactor);
                    e.Handled = true;
                }
            }
        }

        private void WorkPlaneGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }
    }
}
