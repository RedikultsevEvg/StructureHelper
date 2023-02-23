using System.Collections.Generic;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Models.Forces
{
    public class ForceCombinationList : IForceCombinationList
    {

        public string Name { get; set; }
        public bool SetInGravityCenter { get; set; }
        public Point2D ForcePoint { get; private set; }
        public List<IDesignForceTuple> DesignForces { get; private set; }
        

        public ForceCombinationList()
        {
            SetInGravityCenter = true;
            ForcePoint = new Point2D() { X = 0, Y = 0 };
            DesignForces = new List<IDesignForceTuple>();
            DesignForces.Add(new DesignForceTuple(LimitStates.ULS, CalcTerms.ShortTerm));
            DesignForces.Add(new DesignForceTuple(LimitStates.ULS, CalcTerms.LongTerm));
            DesignForces.Add(new DesignForceTuple(LimitStates.SLS, CalcTerms.ShortTerm));
            DesignForces.Add(new DesignForceTuple(LimitStates.SLS, CalcTerms.LongTerm));
        }

        public object Clone()
        {
            var newItem = new ForceCombinationList();
            newItem.Name = Name + " copy";
            newItem.SetInGravityCenter = SetInGravityCenter;
            newItem.ForcePoint.X = ForcePoint.X;
            newItem.ForcePoint.Y = ForcePoint.Y;
            newItem.DesignForces.Clear();
            foreach (var item in DesignForces)
            {
                var newForce = item.Clone() as IDesignForceTuple;
                newItem.DesignForces.Add(newForce);
            }
            return newItem;
        }
    }
}
