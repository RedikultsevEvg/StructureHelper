using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class ReinforcementViewPrimitive : PointViewPrimitive, IHasSurroundingPrimitive
    {
        ReinforcementPrimitive primitive;

        public INdmPrimitive SurroundingPrimitive
        {
            get => primitive.SurroundingPrimitive;
            set
            {
                primitive.SurroundingPrimitive = value;
                OnPropertyChanged(nameof(SurroundingPrimitive));
            }
        }

        public ReinforcementViewPrimitive(ReinforcementPrimitive _primitive) : base(_primitive)
        {
            primitive = _primitive;
        }
    }
}
