using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Services;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class BeamShearAxisActionUpdateStrategy : IUpdateStrategy<IBeamShearAxisAction>
    {
        private IUpdateStrategy<IFactoredCombinationProperty> combinationUpdateStrategy;
        public void Update(IBeamShearAxisAction targetObject, IBeamShearAxisAction sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            InitializeStrategies();
            targetObject.Name = sourceObject.Name;
            targetObject.SupportForce = sourceObject.SupportForce.Clone() as IFactoredForceTuple;
            CheckObject.ThrowIfNull(targetObject.ShearLoads);
            targetObject.ShearLoads.Clear();
            CheckObject.ThrowIfNull(sourceObject.ShearLoads);
            foreach (var item in sourceObject.ShearLoads)
            {
                IBeamSpanLoad beamShearLoad = item.Clone() as IBeamSpanLoad;
                targetObject.ShearLoads.Add(beamShearLoad);
            }
        }

        private void InitializeStrategies()
        {
            combinationUpdateStrategy ??= new FactoredCombinationPropertyUpdateStrategy(); 
        }
    }
}
