using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Forces
{
    public abstract class ForceActionVMBase : ViewModelBase
    {
        IForceAction forceAction;

        public string Name
        {
            get => forceAction.Name;
            set
            {
                forceAction.Name = value;
            }
        }

        public bool SetInGravityCenter
        {
            get => forceAction.SetInGravityCenter;
            set
            {
                forceAction.SetInGravityCenter = value;
                OnPropertyChanged(nameof(SetInGravityCenter));
                OnPropertyChanged(nameof(CoordEnable));
            }
        }

        public bool CoordEnable => !SetInGravityCenter;

        public double CenterX
        {
            get => forceAction.ForcePoint.X;
            set
            {
                forceAction.ForcePoint.X = value;
                OnPropertyChanged(nameof(CenterX));
            }
        }

        public double CenterY
        {
            get => forceAction.ForcePoint.Y;
            set
            {
                forceAction.ForcePoint.Y = value;
                OnPropertyChanged(nameof(CenterY));
            }
        }
        public ForceActionVMBase(IForceAction forceAction)
        {
            this.forceAction = forceAction;
        }
    }
}
