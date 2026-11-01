using StructureHelper.Windows.ViewModels;
using System;
using System.ComponentModel;
using System.Windows;

namespace StructureHelper.Windows.Shapes
{
    /// <summary>
    /// Parameters of the polygon offset operation.
    /// </summary>
    public class PolygonOffsetViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private double distance = 0.1d;
        private bool isOutward = true;

        /// <summary>
        /// Offset distance in metres.
        /// </summary>
        public double Distance
        {
            get => distance;
            set
            {
                if (distance == value)
                {
                    return;
                }
                distance = value;
                OnPropertyChanged(nameof(Distance));
                OnPropertyChanged(nameof(SignedDistance));
            }
        }

        public bool IsOutward
        {
            get => isOutward;
            set
            {
                if (isOutward == value)
                {
                    return;
                }
                isOutward = value;
                OnPropertyChanged(nameof(IsOutward));
                OnPropertyChanged(nameof(SignedDistance));
            }
        }

        /// <summary>
        /// Positive distance for an outward offset, negative for an inward offset.
        /// </summary>
        public double SignedDistance => IsOutward ? Distance : -Distance;

        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Distance)
                    && (double.IsNaN(Distance) || double.IsInfinity(Distance) || Distance <= 0d))
                {
                    return "Offset distance must be a positive finite number";
                }
                return string.Empty;
            }
        }

        public override void OkAction()
        {
            if (double.IsNaN(Distance) || double.IsInfinity(Distance) || Distance <= 0d)
            {
                MessageBox.Show(
                    "Offset distance must be a positive finite number",
                    "Check offset",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            base.OkAction();
        }
    }
}
