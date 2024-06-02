using StructureHelper.Infrastructure;
using System;

namespace StructureHelper.Windows.Graphs
{
    public class GraphVisualProps : ViewModelBase
    {
        private double lineSmoothness;
        private double strokeSize;
        private double opacity;

        public double LineSmoothness
        {
            get { return lineSmoothness; }
            set
            {
                value = Math.Max(value, 0d);
                value = Math.Min(value, MaxLineSmoothness);
                value = Math.Round(value, 2);
                lineSmoothness = value;
                OnPropertyChanged(nameof(LineSmoothness));
            }
        }

        public double StrokeSize
        {
            get { return strokeSize; }
            set
            {
                value = Math.Max(value, 0d);
                value = Math.Min(value, MaxStrokeSize);
                value = Math.Round(value);
                strokeSize = value;
                OnPropertyChanged(nameof(StrokeSize));
            }
        }

        public double MaxLineSmoothness { get; }
        public double MaxStrokeSize { get; }

        public double Opacity
        {
            get => opacity; set
            {
                value = Math.Max(value, 0d);
                value = Math.Min(value, 1d);
                value = Math.Round(value, 2);
                opacity = value;
                OnPropertyChanged(nameof(Opacity));
            }
        }

        public GraphVisualProps()
        {
            MaxLineSmoothness = 1d;
            MaxStrokeSize = 20d;

            lineSmoothness = 0.3;
            strokeSize = 0;
            Opacity = 0d;
        }
    }
}
