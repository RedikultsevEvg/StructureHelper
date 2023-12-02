using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class SurroundData
    {
        public double XMax { get; set; }
        public double XMin { get; set; }
        public double YMax { get; set; }
        public double YMin { get; set; }
        public double ConstZ { get; set; }
        public int PointCount { get; set; }
        public SurroundData()
        {
            XMax = 1e7d;
            XMin = -1e7d;
            YMax = 1e7d;
            YMin = -1e7d;
            PointCount = 80;
        }
    }
}
