using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public class NamedAreaPoint : INamedAreaPoint
    {
        public string Name { get; set; }
        public Point2D Point { get; set; }
        public double Area { get; set; }
    }
}
