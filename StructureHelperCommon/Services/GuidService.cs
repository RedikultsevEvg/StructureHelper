using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Services
{
    public static class GuidService
    {
        public static Guid GetGuid(ISaveable sourceObject, CloneMode cloningMode)
        {
            CheckObject.ThrowIfNull(sourceObject);
            Guid guid;
            if (cloningMode == CloneMode.NewEntity)
            {
                return Guid.NewGuid();
            }
            else if (cloningMode == CloneMode.PreserveIdentity)
            {
                return sourceObject.Id;
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(cloningMode));
            }
        }
    }
}
