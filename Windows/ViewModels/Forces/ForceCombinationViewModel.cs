using StructureHelper.Infrastructure;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.ViewModels.Forces
{
    public class ForceCombinationViewModel : ViewModelBase
    {
        IForceCombinationList combinationList;

        public IDesignForceTuple SelectedTuple { get; set; }
        public string Name
        {
            get => combinationList.Name;
            set
            {
                combinationList.Name = value;
            }
        }

        public bool SetInGravityCenter
        {
            get => combinationList.SetInGravityCenter;
            set
            {
                combinationList.SetInGravityCenter = value;
                OnPropertyChanged(nameof(SetInGravityCenter));
                OnPropertyChanged(nameof(CoordEnable));
            }
        }

        public bool CoordEnable => !SetInGravityCenter;

        public double CenterX
        {
            get => combinationList.ForcePoint.X;
            set
            {
                combinationList.ForcePoint.X = value;
                OnPropertyChanged(nameof(CenterX));
            }
        }

        public double CenterY
        {
            get => combinationList.ForcePoint.Y;
            set
            {
                combinationList.ForcePoint.Y = value;
                OnPropertyChanged(nameof(CenterY));
            }
        }

        public IEnumerable<IDesignForceTuple>  ForceTuples { get => combinationList.DesignForces; }

        public ForceCombinationViewModel(IForceCombinationList combinationList)
        {
            this.combinationList = combinationList;
        }
    }
}
