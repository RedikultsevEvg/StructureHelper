using StructureHelper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public class DeformedShapeSymmetryViewModel : ViewModelBase
    {
        private IDeformedShapeSymmetry deformedShapeSymmetry;
        private int elementCount;
        private double dX;

        public int ElementCount
        {
            get => deformedShapeSymmetry.ElementCount;
            set
            {
                try
                {
                    deformedShapeSymmetry.ElementCount = Math.Max(value, 1);
                    OnPropertyChanged(nameof(ElementCount));
                }
                catch (Exception ex)
                {
                    deformedShapeSymmetry.ElementCount = 1;
                }
            }
        }

        public double DX
        {
            get => deformedShapeSymmetry.DX;
            set
            {
                try
                {
                    deformedShapeSymmetry.DX = value;
                    OnPropertyChanged(nameof(DX));
                }
                catch (Exception ex)
                {
                    deformedShapeSymmetry.DX = 0.0;
                }
            }
        }

        public double DY
        {
            get => deformedShapeSymmetry.DY;
            set
            {
                try
                {
                    deformedShapeSymmetry.DY = value;
                    OnPropertyChanged(nameof(DY));
                }
                catch (Exception ex)
                {
                    deformedShapeSymmetry.DY = 0.0;
                }
            }
        }

        public double DZ
        {
            get => deformedShapeSymmetry.DZ;
            set
            {
                try
                {
                    deformedShapeSymmetry.DZ = value;
                    OnPropertyChanged(nameof(DZ));
                }
                catch (Exception ex)
                {
                    deformedShapeSymmetry.DZ = 0.0;
                }
            }
        }

        public DeformedShapeSymmetryViewModel(IDeformedShapeSymmetry deformedShapeSymmetry)
        {
            this.deformedShapeSymmetry = deformedShapeSymmetry;
        }
    }
}
