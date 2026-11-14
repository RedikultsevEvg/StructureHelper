using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Services;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public class BeamShearAxisActionUpdateStrategy : IParentUpdateStrategy<IBeamShearAxisAction>
    {
        private IUpdateStrategy<IFactoredCombinationProperty> combinationUpdateStrategy;
        private IUpdateStrategy<IFactoredCombinationProperty> CombinationUpdateStrategy => combinationUpdateStrategy ??= new FactoredCombinationPropertyUpdateStrategy();

        public bool UpdateChildren { get; set; } = true;

        public void Update(IBeamShearAxisAction targetObject, IBeamShearAxisAction sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.Name = sourceObject.Name;
            if (UpdateChildren == true)
            {
                targetObject.SupportForce = sourceObject.SupportForce.GetClone(sourceObject.SupportForce) as IFactoredForceTuple;
                CheckObject.ThrowIfNull(targetObject.ShearLoads);
                CheckObject.ThrowIfNull(sourceObject.ShearLoads);
                targetObject.ShearLoads.Clear();
                foreach (var item in sourceObject.ShearLoads)
                {
                    IBeamSpanLoad beamShearLoad = item.GetClone(item, CloneMode.NewEntity, CloneDepth.Deep);
                    targetObject.ShearLoads.Add(beamShearLoad);
                }
            }
        }
    }
}
