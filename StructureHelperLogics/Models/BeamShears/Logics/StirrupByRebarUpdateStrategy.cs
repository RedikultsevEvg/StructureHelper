using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByRebarUpdateStrategy : IUpdateStrategy<IStirrupByRebar>
    {
        private IUpdateStrategy<IStirrup>? baseUpdateStrategy;
        public void Update(IStirrupByRebar targetObject, IStirrupByRebar sourceObject)
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
            targetObject.Spacing = sourceObject.Spacing;
        }
    }
}
