using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.BeamShears
{
    public class DistributedLoadViewModel : OkCancelViewModelBase
    {
        private readonly IDistributedLoad distributedLoad;

        public string Name
        {
            get => distributedLoad.Name;
            set
            {
                distributedLoad.Name = value;
            }
        }
        public double LoadRatio
        {
            get => distributedLoad.LoadRatio;
            set
            {
                distributedLoad.LoadRatio = value;
            }
        }
        public double LoadValue
        {
            get => distributedLoad.LoadValue.Qy;
            set
            {
                distributedLoad.LoadValue.Qy = value;
            }
        }
        public double RelativeLevel
        {
            get => distributedLoad.RelativeLoadLevel;
            set
            {
                if (value > 0.5d)
                {
                    distributedLoad.RelativeLoadLevel = 0.5;
                    return;
                }
                if (value < -0.5d)
                {
                    distributedLoad.RelativeLoadLevel = -0.5;
                    return;
                }
                distributedLoad.RelativeLoadLevel = value;
            }
        }
        public double StartCoordinate
        {
            get => distributedLoad.StartCoordinate;
            set
            {
                distributedLoad.StartCoordinate = value;
            }
        }
        public double EndCoordinate
        {
            get => distributedLoad.EndCoordinate;
            set
            {
                distributedLoad.EndCoordinate = value;
            }
        }

        public DistributedLoadViewModel(IDistributedLoad distributedLoad)
        {
            this.distributedLoad = distributedLoad;
        }
    }
}
