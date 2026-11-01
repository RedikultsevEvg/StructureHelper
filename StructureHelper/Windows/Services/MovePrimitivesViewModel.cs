using StructureHelper.Windows.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace StructureHelper.Windows.Services
{
    /// <summary>
    /// Asks for the offset used to move primitives. Values are stored in metres and shown in millimetres.
    /// </summary>
    public class MovePrimitivesViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private double deltaX;
        private double deltaY;

        public double DeltaX
        {
            get => deltaX;
            set
            {
                if (deltaX == value)
                {
                    return;
                }
                deltaX = value;
                OnPropertyChanged(nameof(DeltaX));
            }
        }

        public double DeltaY
        {
            get => deltaY;
            set
            {
                if (deltaY == value)
                {
                    return;
                }
                deltaY = value;
                OnPropertyChanged(nameof(DeltaY));
            }
        }

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(DeltaX) && (double.IsNaN(DeltaX) || double.IsInfinity(DeltaX)))
                {
                    return "Offset X must be a finite number";
                }
                if (columnName == nameof(DeltaY) && (double.IsNaN(DeltaY) || double.IsInfinity(DeltaY)))
                {
                    return "Offset Y must be a finite number";
                }
                return string.Empty;
            }
        }

        public override void OkAction()
        {
            if (double.IsNaN(DeltaX) || double.IsInfinity(DeltaX)
                || double.IsNaN(DeltaY) || double.IsInfinity(DeltaY))
            {
                MessageBox.Show("Offsets must be finite numbers", "Check offsets", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            base.OkAction();
        }
    }
}
