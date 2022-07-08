using StructureHelperLogics.Data.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.Data.FieldValues
{
    public class ValueShape : IValueShape
    {
        public IShape Shape => throw new NotImplementedException();

        public ICenter Center => throw new NotImplementedException();

        public double Value { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Description { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
