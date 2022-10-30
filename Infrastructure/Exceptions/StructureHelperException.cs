using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Infrastructure.Exceptions
{
    internal class StructureHelperException : Exception
    {
        public StructureHelperException(string errorString) : base(errorString)
        {
        }
    }
}
