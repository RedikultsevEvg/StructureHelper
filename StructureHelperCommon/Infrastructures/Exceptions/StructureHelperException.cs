using System;

namespace StructureHelperCommon.Infrastructures.Exceptions
{
    public class StructureHelperException : Exception
    {
        public StructureHelperException(string errorString) : base(errorString)
        {
        }
    }
}
