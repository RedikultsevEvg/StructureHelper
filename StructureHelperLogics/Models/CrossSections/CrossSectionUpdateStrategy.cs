using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionUpdateStrategy : IUpdateStrategy<ICrossSection>
    {

        public CrossSectionUpdateStrategy()
        {

        }

        public void Update(ICrossSection targetObject, ICrossSection sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            

        }
    }
}
