using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.MaterialBuilders.Decorators
{
    internal class RestrictStrainOption : IDecoratorOption
    {
        private double maxTensileStrain;
        private double maxCompessionStrain;

        public double MaxTensileStrain
        {
            get => maxTensileStrain;
            set
            {
                if (value < 0d)
                {
                    //to do exception
                }
                maxTensileStrain = value;
            }
        }
        public double MaxCompessionStrain { get => maxCompessionStrain; set => maxCompessionStrain = value; }
    }
}
