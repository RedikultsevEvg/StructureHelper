using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.Materials
{
    public class ReinforcementLibUpdateStrategy : IUpdateStrategy<IReinforcementLibMaterial>
    {
        private IUpdateStrategy<ILibMaterial> libUpdateStrategy;
        public ReinforcementLibUpdateStrategy(IUpdateStrategy<ILibMaterial> libUpdateStrategy)
        {
            this.libUpdateStrategy = libUpdateStrategy;
        }
        public ReinforcementLibUpdateStrategy() : this(new LibMaterialUpdateStrategy())
        {
            
        }
        public void Update(IReinforcementLibMaterial targetObject, IReinforcementLibMaterial sourceObject)
        {
            CheckObject.IsNull(sourceObject);
            CheckObject.IsNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            libUpdateStrategy.Update(targetObject, sourceObject);
        }
    }
}
