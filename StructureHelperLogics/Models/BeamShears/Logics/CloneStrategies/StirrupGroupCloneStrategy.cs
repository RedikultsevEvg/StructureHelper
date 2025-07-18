using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupGroupCloneStrategy : ICloneStrategy<IStirrupGroup>
    {
        private IUpdateStrategy<IStirrupGroup> updateStrategy;
        private IUpdateStrategy<IHasStirrups> hasStirrupsUpdateStrategy;
        private readonly ICloningStrategy cloningStrategy;

        public StirrupGroupCloneStrategy(ICloningStrategy cloningStrategy)
        {
            this.cloningStrategy = cloningStrategy;
        }

        public IStirrupGroup GetClone(IStirrupGroup sourceObject)
        {
            //updateStrategy ??= new StirrupGroupUpdateStrategy() { UpdateChildren = false };
            //hasStirrupsUpdateStrategy ??= new HasStirrupsUpdateCloneStrategy(cloningStrategy);
            //StirrupGroup newItem = new(Guid.NewGuid());
            //updateStrategy.Update(newItem,sourceObject);
            //hasStirrupsUpdateStrategy.Update(newItem,sourceObject);
            //return newItem;
            return cloningStrategy.Clone(sourceObject);
        }
    }
}
