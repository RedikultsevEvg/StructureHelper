using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Forces;
using System;

namespace StructureHelper.Windows.ViewModels.Forces
{
    public class ForceTupleVM : ViewModelBase
    {
        IForceTuple forceTuple;
        public double Mx
        {
            get => forceTuple.Mx;
            set
            {
                forceTuple.Mx = Math.Max(value, MinMx);
                forceTuple.Mx = Math.Min(value, MaxMx);
                OnPropertyChanged(nameof(Mx));
            }
        }
        public double My
        {
            get => forceTuple.My;
            set
            {
                forceTuple.My = Math.Max(value, MinMy);
                forceTuple.My = Math.Min(value, MaxMy);
                OnPropertyChanged(nameof(My));
            }
        }
        public double Nz
        {
            get => forceTuple.Nz;
            set
            {
                forceTuple.Nz = Math.Max(value, MinNz);
                forceTuple.Nz = Math.Min(value, MaxNz);
                OnPropertyChanged(nameof(Nz));
            }
        }
        public double Qx
        {
            get => forceTuple.Qx;
            set
            {
                forceTuple.Qx = value;
                OnPropertyChanged(nameof(Qx));
            }
        }
        public double Qy
        {
            get => forceTuple.Qy;
            set
            {
                forceTuple.Qy = value;
                OnPropertyChanged(nameof(Qy));
            }
        }
        public double Mz
        {
            get => forceTuple.Mz;
            set
            {
                forceTuple.Mz = value;
                OnPropertyChanged(nameof(Mz));
            }
        }

        public double MaxMx { get; set; } = double.PositiveInfinity;
        public double MinMx { get; set; } = double.NegativeInfinity;
        public double MaxMy { get; set; } = double.PositiveInfinity;
        public double MinMy { get; set; } = double.NegativeInfinity;
        public double MaxNz { get; set; } = double.PositiveInfinity;
        public double MinNz { get; set; } = double.NegativeInfinity;
        public double MaxMz { get; set; } = double.PositiveInfinity;
        public double MinMz { get; set; } = double.NegativeInfinity;
        public double MaxQx { get; set; } = double.PositiveInfinity;
        public double MinQx { get; set; } = double.NegativeInfinity;
        public double MaxQy { get; set; } = double.PositiveInfinity;
        public double MinQy { get; set; } = double.NegativeInfinity;

        public ForceTupleVM(IForceTuple forceTuple)
        {
            this.forceTuple = forceTuple;
        }
    }
}
