using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

namespace StructureHelperCommon.Models.Analyses
{
    public class DateVersionUpdateStrategy : IUpdateStrategy<IDateVersion>
    {
        public void Update(IDateVersion targetObject, IDateVersion sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            targetObject.DateTime = sourceObject.DateTime;
            targetObject.Comment = sourceObject.Comment;
        }
    }
}
