using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class HasBeamShearSectionUpdateStrategy : IUpdateStrategy<IHasBeamShearSections>
    {
        public void Update(IHasBeamShearSections targetObject, IHasBeamShearSections sourceObject)
        {
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            CheckObject.IsNull(sourceObject.Sections);
            CheckObject.IsNull(targetObject.Sections);
            targetObject.Sections.Clear();
            targetObject.Sections.AddRange(sourceObject.Sections);
        }
    }
}
