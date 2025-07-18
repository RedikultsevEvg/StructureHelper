using StructureHelper.Infrastructure;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearDesignRangePropertyViewModel : ViewModelBase
    {
        private const double minRelativeValue = 0.5;
        private const int minAbsoluteValue = 0;
        private const int minStepCount = 10;
        private const int maxStepCount = 1000;
        private readonly IBeamShearDesignRangeProperty designRangeProperty;

        public double AbsoluteRangeValue
        {
            get => designRangeProperty.AbsoluteRangeValue;
            set
            {
                value = Math.Max(value, minAbsoluteValue);
                designRangeProperty.AbsoluteRangeValue = value;
                OnPropertyChanged(nameof(AbsoluteRangeValue));
            }
        }
        public double RelativeEffectiveDepthRangeValue
        {
            get => designRangeProperty.RelativeEffectiveDepthRangeValue;
            set
            {
                value = Math.Max(value, minRelativeValue);
                designRangeProperty.RelativeEffectiveDepthRangeValue = value;
            }
        }
        public int StepCount
        {
            get => designRangeProperty.StepCount;
            set
            {
                value = Math.Max(value, minStepCount);
                value = Math.Min(value, maxStepCount);
                designRangeProperty.StepCount = value;
            }
        }


        public BeamShearDesignRangePropertyViewModel(IBeamShearDesignRangeProperty designRangeProperty)
        {
            this.designRangeProperty = designRangeProperty;
        }

    }
}
