using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears.Logics
{
    public class StirrupByUniformRebarUpdateStrategy : IUpdateStrategy<IStirrupByUniformRebar>
    {
        private IUpdateStrategy<IStirrup>? baseUpdateStrategy;
        public void Update(IStirrupByUniformRebar targetObject, IStirrupByUniformRebar sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            baseUpdateStrategy ??= new StirrupBaseUpdateStrategy();
            baseUpdateStrategy.Update(targetObject, sourceObject);
            CheckObject.IsNull(sourceObject.Material);
            targetObject.Material = sourceObject.Material.Clone() as IReinforcementLibMaterial;
            targetObject.Diameter = sourceObject.Diameter;
            targetObject.LegCount = sourceObject.LegCount;
            targetObject.Step = sourceObject.Step;
        }
    }
}
