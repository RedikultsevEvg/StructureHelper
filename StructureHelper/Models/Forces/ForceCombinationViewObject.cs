using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Models.Forces
{
    internal class ForceCombinationViewObject
    {
        IForceCombinationList forceCombinationList;

        private IDesignForceTuple ulsShort;

        public IDesignForceTuple ULSShort
        {
            get { return ulsShort; }
        }

        public ForceCombinationViewObject(ForceCombinationList _forceCombinationList)
        {
            forceCombinationList = _forceCombinationList;
            ulsShort = forceCombinationList.DesignForces.Where(x => x.LimitState == LimitStates.ULS & x.CalcTerm == CalcTerms.ShortTerm).Single();
        }
    }
}
