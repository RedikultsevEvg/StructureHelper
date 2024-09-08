using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Projects
{
    public class FileVersionUpdateStrategy : IUpdateStrategy<IFileVersion>
    {
        public void Update(IFileVersion targetObject, IFileVersion sourceObject)
        {
            CheckObject.IsNull(targetObject, sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            targetObject.VersionNumber = sourceObject.VersionNumber;
            targetObject.SubVersionNumber = sourceObject.SubVersionNumber;
        }
    }
}
