using StructureHelper.Windows.ViewModels;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic
{
    public class SurroundDataViewModel : OkCancelViewModelBase, IDataErrorInfo
    {
        private readonly SurroundData surroundData;

        public double XMax
        {
            get => surroundData.XMax; set
            {
                surroundData.XMax = value;
                OnPropertyChanged(nameof(XMax));
            }
        }
        public double XMin
        {
            get => surroundData.XMin; set
            {
                surroundData.XMin = value;
                OnPropertyChanged(nameof(XMin));
            }
        }
        public double YMax
        {
            get => surroundData.YMax; set
            {
                surroundData.YMax = value;
                OnPropertyChanged(nameof(YMax));
            }
        }
        public double YMin
        {
            get => surroundData.YMin; set
            {
                surroundData.YMin = value;
                OnPropertyChanged(nameof(YMin));
            }
        }
        public double ConstZ
        {
            get => surroundData.ConstZ; set
            {
                surroundData.ConstZ = value;
                OnPropertyChanged(nameof(ConstZ));
            }
        }
        public int PointCount
        {
            get => surroundData.PointCount; set
            {
                try
                {
                    surroundData.PointCount = value;
                }
                catch (Exception)
                {
                    surroundData.PointCount = 40;
                }
                OnPropertyChanged(nameof(PointCount));
            }
        }

        public string Error => throw new NotImplementedException();

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case nameof(PointCount):
                        if (PointCount < 24)
                        {
                            error = "Point count must be greater than 24";
                        }
                        break;
                }
                return error;
            }
        }

        public SurroundDataViewModel(SurroundData surroundData)
        {
            this.surroundData = surroundData;
        }

        internal void RefreshAll()
        {
            OnPropertyChanged(nameof(XMax));
            OnPropertyChanged(nameof(XMin));
            OnPropertyChanged(nameof(YMax));
            OnPropertyChanged(nameof(YMin));
            OnPropertyChanged(nameof(ConstZ));
            OnPropertyChanged(nameof(PointCount));
        }
    }
}
