using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.CrossSections
{
    internal class CrossSectionRepositoryUpdateStrategy : IUpdateStrategy<ICrossSectionRepository>
    {


        public void Update(ICrossSectionRepository targetObject, ICrossSectionRepository sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            
        }
    }
}
