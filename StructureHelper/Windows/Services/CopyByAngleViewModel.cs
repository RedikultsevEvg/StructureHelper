using StructureHelper.Windows.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace StructureHelper.Windows.Services
{
    /// <summary>
    /// Parameters of the polar copy operation: rotation centre, start angle, angular step,
    /// direction and the number of copies.
    /// </summary>
    public class CopyByAngleViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private double centerX;
        private double centerY;
        private double startAngle = 30d;
        private double angleOfRotation = 30d;
        private int copyCount = 3;
        private bool isClockwise;

        public double CenterX
        {
            get => centerX;
            set
            {
                if (centerX == value)
                {
                    return;
                }
                centerX = value;
                OnPropertyChanged(nameof(CenterX));
            }
        }

        public double CenterY
        {
            get => centerY;
            set
            {
                if (centerY == value)
                {
                    return;
                }
                centerY = value;
                OnPropertyChanged(nameof(CenterY));
            }
        }

        public double StartAngle
        {
            get => startAngle;
            set
            {
                if (startAngle == value)
                {
                    return;
                }
                startAngle = value;
                OnPropertyChanged(nameof(StartAngle));
                OnPropertyChanged(nameof(StartAngleRadians));
            }
        }

        public double AngleOfRotation
        {
            get => angleOfRotation;
            set
            {
                if (angleOfRotation == value)
                {
                    return;
                }
                angleOfRotation = value;
                OnPropertyChanged(nameof(AngleOfRotation));
                OnPropertyChanged(nameof(AngleStepRadians));
            }
        }

        public int CopyCount
        {
            get => copyCount;
            set
            {
                if (copyCount == value)
                {
                    return;
                }
                copyCount = value;
                OnPropertyChanged(nameof(CopyCount));
            }
        }

        public bool IsClockwise
        {
            get => isClockwise;
            set
            {
                if (isClockwise == value)
                {
                    return;
                }
                isClockwise = value;
                OnPropertyChanged(nameof(IsClockwise));
                OnPropertyChanged(nameof(AngleStepRadians));
            }
        }

        public double StartAngleRadians => StartAngle * Math.PI / 180d;

        public double AngleStepRadians
        {
            get
            {
                double sign = IsClockwise ? -1d : 1d;
                return sign * AngleOfRotation * Math.PI / 180d;
            }
        }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if ((columnName == nameof(CenterX) && !IsFinite(CenterX))
                    || (columnName == nameof(CenterY) && !IsFinite(CenterY)))
                {
                    return "Rotation center must contain finite numbers";
                }
                if ((columnName == nameof(StartAngle) && !IsFinite(StartAngle))
                    || (columnName == nameof(AngleOfRotation) && !IsFinite(AngleOfRotation)))
                {
                    return "Angles must be finite numbers";
                }
                if (columnName == nameof(CopyCount) && CopyCount < 1)
                {
                    return "Number of copies must be at least 1";
                }
                return string.Empty;
            }
        }

        public override void OkAction()
        {
            if (!IsFinite(CenterX) || !IsFinite(CenterY)
                || !IsFinite(StartAngle) || !IsFinite(AngleOfRotation))
            {
                MessageBox.Show(
                    "Rotation center and angles must be finite numbers",
                    "Check copy parameters",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            if (CopyCount < 1)
            {
                MessageBox.Show(
                    "Number of copies must be at least 1",
                    "Check copy parameters",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            base.OkAction();
        }

        private static bool IsFinite(double value) => !double.IsNaN(value) && !double.IsInfinity(value);
    }
}
