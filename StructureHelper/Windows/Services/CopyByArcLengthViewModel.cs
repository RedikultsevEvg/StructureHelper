using StructureHelper.Windows.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace StructureHelper.Windows.Services
{
    /// <summary>
    /// Parameters of the copy-along-arc operation: rotation centre, start angle, arc length
    /// between adjacent copies, direction and the number of copies.
    /// </summary>
    public class CopyByArcLengthViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private double centerX;
        private double centerY;
        private double startAngle = 30d;
        private double arcLength = 0.1d;
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

        /// <summary>
        /// Distance along the arc between adjacent copies, in metres.
        /// </summary>
        public double ArcLength
        {
            get => arcLength;
            set
            {
                if (arcLength == value)
                {
                    return;
                }
                arcLength = value;
                OnPropertyChanged(nameof(ArcLength));
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
            }
        }

        public double StartAngleRadians => StartAngle * Math.PI / 180d;

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
                if (columnName == nameof(StartAngle) && !IsFinite(StartAngle))
                {
                    return "Start angle must be a finite number";
                }
                if (columnName == nameof(ArcLength) && (!IsFinite(ArcLength) || ArcLength <= 0))
                {
                    return "Arc length must be a positive finite number";
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
                || !IsFinite(StartAngle)
                || !IsFinite(ArcLength) || ArcLength <= 0)
            {
                MessageBox.Show(
                    "Rotation center, start angle and arc length must be valid",
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
