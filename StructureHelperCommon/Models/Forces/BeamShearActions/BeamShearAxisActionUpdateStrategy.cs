using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class BeamShearAxisActionUpdateStrategy : IUpdateStrategy<IBeamShearAxisAction>
    {
        private IUpdateStrategy<IFactoredCombinationProperty> combinationUpdateStrategy;
        public void Update(IBeamShearAxisAction targetObject, IBeamShearAxisAction sourceObject)
        {
            CheckObject.IsNull(targetObject);
            CheckObject.IsNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeStrategies();
            targetObject.Name = sourceObject.Name;
            targetObject.SupportShearForce = sourceObject.SupportShearForce;
            CheckObject.IsNull(targetObject.FactoredCombinationProperty);
            CheckObject.IsNull(sourceObject.FactoredCombinationProperty);
            combinationUpdateStrategy.Update(targetObject.FactoredCombinationProperty, sourceObject.FactoredCombinationProperty);
            CheckObject.IsNull(targetObject.ShearLoads);
            targetObject.ShearLoads.Clear();
            CheckObject.IsNull(sourceObject.ShearLoads);
            foreach (var item in sourceObject.ShearLoads)
            {
                IBeamShearLoad beamShearLoad = item.Clone() as IBeamShearLoad;
                targetObject.ShearLoads.Add(beamShearLoad);
            }
        }

        private void InitializeStrategies()
        {
            combinationUpdateStrategy ??= new FactoredCombinationPropertyUpdateStrategy(); 
        }
    }
}
