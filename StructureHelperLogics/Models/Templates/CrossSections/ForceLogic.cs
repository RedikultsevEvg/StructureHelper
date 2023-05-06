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
        public IEnumerable<IForceAction> GetCombinationList()
        {
            var combinations = new List<IForceAction>();
            var combination = new ForceCombinationList() { Name = "New Force Action"};
            combination.DesignForces.Clear();
            combination.DesignForces.AddRange(ForceCombinationListFactory.GetDesignForces(DesignForceType.Suit_1));
            combinations.Add(combination);
            var factorCombination = new ForceCombinationByFactor();
            combinations.Add(factorCombination);
            return combinations;
        }
    }
}
