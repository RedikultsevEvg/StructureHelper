using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public class DesignForcePair : IDesignForcePair
    {
        readonly IUpdateStrategy<IAction> updateStrategy = new ActionUpdateStrategy();
        public Guid Id { get; }
        public string Name { get; set; }
        public IPoint2D ForcePoint { get; set; }
        public bool SetInGravityCenter { get; set; }
        public LimitStates LimitState { get; set; }
        public IForceTuple LongForceTuple { get; set; }
        public IForceTuple FullForceTuple { get; set; }


        public DesignForcePair(Guid id)
        {
            Id = id;
            LongForceTuple = new ForceTuple();
            FullForceTuple = new ForceTuple();
        }

        public DesignForcePair() : this(Guid.NewGuid()) {}

        public IForceCombinationList GetCombinations()
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            var newItem = new DesignForcePair();
            updateStrategy.Update(newItem, this);
            return newItem;
        }
    }
}
