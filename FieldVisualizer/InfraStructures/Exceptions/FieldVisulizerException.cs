using System;
using System.Collections.Generic;
using System.Text;

namespace FieldVisualizer.InfraStructures.Exceptions
{
    public class FieldVisulizerException : Exception
    {
        public FieldVisulizerException(string errorString) : base(errorString)
        {
        }
    }
}
