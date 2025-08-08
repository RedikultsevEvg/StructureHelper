using StructureHelper.Infrastructure.UI.GraphicalPrimitives;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.BeamShears
{
    internal class StirrupToGraphicPrimitiveConvertLogic : IObjectConvertStrategy<List<IGraphicalPrimitive>, IStirrup>
    {
        private IInclinedSection inclinedSection;
        private List<IGraphicalPrimitive> primitives;

        public StirrupToGraphicPrimitiveConvertLogic(IInclinedSection inclinedSection)
        {
            this.inclinedSection = inclinedSection;
        }

        public List<IGraphicalPrimitive> Convert(IStirrup source)
        {
            primitives = new List<IGraphicalPrimitive>();
            GetStirrupPrimitives(source);
            return primitives;
        }

        private void GetStirrupPrimitives(IStirrup source)
        {
            if (source is IHasStirrups hasStirrup) //if stirrup group
            {
                foreach (var item in hasStirrup.Stirrups)
                {
                    GetStirrupPrimitives(item); //recursion for stirrup group
                }
            }
            else
            {
                GetStirrupPrimitive(source);
            }
        }

        private void GetStirrupPrimitive(IStirrup stirrup)
        {
            if (stirrup is IStirrupByRebar stirrupByRebar)
            {
                StirrupByRebarPrimitive stirrupByRebarPrimitive = new(stirrupByRebar, inclinedSection);
                primitives.Add(stirrupByRebarPrimitive);
            }
            else if (stirrup is IStirrupByDensity stirrupByDensity)
            {
                StirrupByDensityPrimitive stirrupByDensityPrimitive = new(stirrupByDensity, inclinedSection);
                primitives.Add(stirrupByDensityPrimitive);
            }
            else if (stirrup is IStirrupByInclinedRebar stirrupByInclinedRebar)
            {
                StirrupByInclinedRebarPrimitive stirrupByInclinedRebarPrimitive = new(stirrupByInclinedRebar, inclinedSection);
                primitives.Add(stirrupByInclinedRebarPrimitive);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(stirrup));
            }
        }
    }
}
