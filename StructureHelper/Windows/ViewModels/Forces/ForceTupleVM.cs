using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                forceTuple.Mx = value;
                OnPropertyChanged(nameof(Mx));
            }
        }
        public double My
        {
            get => forceTuple.My;
            set
            {
                forceTuple.My = value;
                OnPropertyChanged(nameof(My));
            }
        }
        public double Nz
        {
            get => forceTuple.Nz;
            set
            {
                forceTuple.Nz = value;
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
        public ForceTupleVM(IForceTuple forceTuple)
        {
            this.forceTuple = forceTuple;
        }
    }
}
