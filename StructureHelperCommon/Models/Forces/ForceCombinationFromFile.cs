using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    internal class ForceCombinationFromFile : IForceCombinationFromFile
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<IForceFileProperty> ForceFiles { get; set; } = new();
        public bool SetInGravityCenter { get; set; } = true;
        public IPoint2D ForcePoint { get; set; } = new Point2D();


        public object Clone()
        {
            throw new NotImplementedException();
        }

        public IForceCombinationList GetCombination()
        {
            throw new NotImplementedException();
        }

        public List<IForceCombinationList> GetCombinations()
        {
            throw new NotImplementedException();
        }
    }
}
