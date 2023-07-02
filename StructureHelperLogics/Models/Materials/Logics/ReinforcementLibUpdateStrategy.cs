using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    internal class ReinforcementLibUpdateStrategy : IUpdateStrategy<IReinforcementLibMaterial>
    {
        LibMaterialUpdateStrategy libUpdateStrategy = new LibMaterialUpdateStrategy();
        public void Update(IReinforcementLibMaterial targetObject, IReinforcementLibMaterial sourceObject)
        {
            libUpdateStrategy.Update(targetObject, sourceObject);
        }
    }
}
