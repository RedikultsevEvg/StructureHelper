using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Exceptions
{
    public static class ErrorCommonProcessor
    {
        public static void ObjectTypeIsUnknown(Type expectedType, Type actualType)
        {
            throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + $"\n Expected: {expectedType},\n But was: {actualType}");
        }
    }
}
