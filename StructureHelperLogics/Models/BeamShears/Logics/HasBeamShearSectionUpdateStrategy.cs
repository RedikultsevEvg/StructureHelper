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
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            CheckObject.ThrowIfNull(sourceObject.Sections);
            CheckObject.ThrowIfNull(targetObject.Sections);
            targetObject.Sections.Clear();
            targetObject.Sections.AddRange(sourceObject.Sections);
        }
    }
}
