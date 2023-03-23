using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class ReinforcementViewPrimitive : PointViewPrimitive, IHasHostPrimitive
    {
        ReinforcementPrimitive primitive;

        public INdmPrimitive HostPrimitive
        {
            get => primitive.HostPrimitive;
            set
            {
                primitive.HostPrimitive = value;
                OnPropertyChanged(nameof(HostPrimitive));
            }
        }

        public ReinforcementViewPrimitive(ReinforcementPrimitive _primitive) : base(_primitive)
        {
            primitive = _primitive;
        }
    }
}
