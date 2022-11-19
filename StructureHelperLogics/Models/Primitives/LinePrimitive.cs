using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Primitives
{
    public class LinePrimitive : IPrimitive
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICenter Center { get; set; }
        public IShape Shape { get; }   

        public LinePrimitive()
        {

        }

        public IEnumerable<INdmPrimitive> GetNdmPrimitives()
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
