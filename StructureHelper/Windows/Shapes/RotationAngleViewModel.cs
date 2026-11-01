using StructureHelper.Infrastructure;
using StructureHelper.Windows.ViewModels;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Input;

namespace StructureHelper.Windows.Shapes
{
    /// <summary>
    /// Asks for a rotation angle, its direction and the centre of rotation.
    /// </summary>
    public class RotationAngleViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private double angleDegrees;
        private double centerX;
        private double centerY;
        private bool isClockwise = true;
        private RelayCommand? setAngleCommand;

        public double AngleDegrees
        {
            get => angleDegrees;
            set
            {
                if (angleDegrees == value)
                {
                    return;
                }
                angleDegrees = value;
                Refresh();
            }
        }

        /// <summary>
        /// X coordinate of the rotation centre, in metres.
        /// </summary>
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

        /// <summary>
        /// Y coordinate of the rotation centre, in metres.
        /// </summary>
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

        /// <summary>
        /// True for clockwise rotation (the default), false for counter-clockwise rotation.
        /// </summary>
        public bool IsClockwise
        {
            get => isClockwise;
            set
            {
                isClockwise = value;
                Refresh();
            }
        }

        private void Refresh()
        {
            OnPropertyChanged(nameof(AngleDegrees));
            OnPropertyChanged(nameof(IsClockwise));
            OnPropertyChanged(nameof(AngleRadians));
        }

        /// <summary>
        /// Rotation angle in radians. Clockwise rotation is negative in the local mathematical
        /// coordinate system, counter-clockwise rotation is positive.
        /// </summary>
        public double AngleRadians
        {
            get
            {
                double sign = IsClockwise ? -1d : 1d;
                return sign * AngleDegrees * Math.PI / 180d;
            }
        }

        public string Error => string.Empty;

        /// <summary>
        /// Sets <see cref="AngleDegrees"/> from a quick-angle button command parameter.
        /// </summary>
        public ICommand SetAngleCommand => setAngleCommand ??= new RelayCommand(SetAngle);

        private void SetAngle(object parameter)
        {
            if (parameter is null)
            {
                return;
            }
            if (parameter is double doubleValue)
            {
                AngleDegrees = doubleValue;
                return;
            }

            string text = parameter.ToString() ?? string.Empty;
            if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out double value)
                || double.TryParse(text, NumberStyles.Float, CultureInfo.CurrentCulture, out value))
            {
                AngleDegrees = value;
            }
        }

        public RotationAngleViewModel() : this(0d, 0d)
        {
        }

        public RotationAngleViewModel(double centerX, double centerY)
        {
            this.centerX = centerX;
            this.centerY = centerY;
            Refresh();
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(AngleDegrees)
                    && (double.IsNaN(AngleDegrees) || double.IsInfinity(AngleDegrees)))
                {
                    return "Angle must be a finite number";
                }
                if (columnName == nameof(CenterX) && (double.IsNaN(CenterX) || double.IsInfinity(CenterX)))
                {
                    return "Center X must be a finite number";
                }
                if (columnName == nameof(CenterY) && (double.IsNaN(CenterY) || double.IsInfinity(CenterY)))
                {
                    return "Center Y must be a finite number";
                }
                return string.Empty;
            }
        }

        public override void OkAction()
        {
            if (double.IsNaN(AngleDegrees) || double.IsInfinity(AngleDegrees)
                || double.IsNaN(CenterX) || double.IsInfinity(CenterX)
                || double.IsNaN(CenterY) || double.IsInfinity(CenterY))
            {
                MessageBox.Show(
                    "Angle and rotation center must be finite numbers",
                    "Check rotation data",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            base.OkAction();
        }
    }
}
