using StructureHelper.Windows.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace StructureHelper.Windows.Shapes
{
    /// <summary>
    /// Asks for a polygon rotation angle in degrees.
    /// </summary>
    public class RotationAngleViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private double angleDegrees;

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
                OnPropertyChanged(nameof(AngleDegrees));
            }
        }

        public double AngleRadians => AngleDegrees * Math.PI / 180d;

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(AngleDegrees)
                    && (double.IsNaN(AngleDegrees) || double.IsInfinity(AngleDegrees)))
                {
                    return "Angle must be a finite number";
                }
                return string.Empty;
            }
        }

        public override void OkAction()
        {
            if (double.IsNaN(AngleDegrees) || double.IsInfinity(AngleDegrees))
            {
                MessageBox.Show("Angle must be a finite number", "Check angle", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            base.OkAction();
        }
    }
}
