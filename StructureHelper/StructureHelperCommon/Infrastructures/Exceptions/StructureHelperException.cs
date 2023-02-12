using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Exceptions
{
    public class StructureHelperException : Exception
    {
        public StructureHelperException(string errorString) : base(errorString)
        {
        }
    }
}
