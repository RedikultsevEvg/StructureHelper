using StructureHelperCommon.Infrastructures.Enums;
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
        private IForceFactoredCombination factoredCombination;
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<IForceFileProperty> ForceFiles { get; set; } = new();
        public bool SetInGravityCenter { get; set; } = true;
        public IPoint2D ForcePoint { get; set; } = new Point2D();

        public IFactoredCombinationProperty CombinationProperty { get; } = new FactoredCombinationProperty();

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public List<IForceCombinationList> GetCombinations()
        {
            factoredCombination = new ForceFactoredList();
            return factoredCombination.GetCombinations();
        }
    }
}
