using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperCommon.Infrastructures.Exceptions
{
    public class StructureHelperNullReferenceException : StructureHelperException
    {
        public StructureHelperNullReferenceException(string errorString) : base(errorString)
        {
        }

        public StructureHelperNullReferenceException(Exception ex) : base(ex)
        {
        }
    }
}
