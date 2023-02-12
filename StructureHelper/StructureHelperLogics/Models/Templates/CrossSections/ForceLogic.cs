using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Templates.CrossSections
{
    internal class ForceLogic : IForceLogic
    {
        public IEnumerable<IForceCombinationList> GetCombinationList()
        {
            var combinations = new List<IForceCombinationList>();
            var combination = new ForceCombinationList() { Name = "New Force Action"};
            combination.DesignForces.Clear();
            combination.DesignForces.AddRange(ForceCombinationListFactory.GetDesignForces(DesignForceType.Suit_1));
            combinations.Add(combination);
            return combinations;
        }
    }
}
