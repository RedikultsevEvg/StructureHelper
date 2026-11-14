using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Primitives
{
    public class LinePrimitive : IPrimitive
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IPoint2D Center { get; set; }
        public IShape Shape { get; }   

        public LinePrimitive()
            : this(Guid.NewGuid())
        {
        }

        public LinePrimitive(Guid id)
        {
            Id = id;
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

        public IPrimitive GetClone(IPrimitive sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            return CloneFallback.GetClone(sourceObject, cloneMode, cloneDepth);
        }
    }
}
