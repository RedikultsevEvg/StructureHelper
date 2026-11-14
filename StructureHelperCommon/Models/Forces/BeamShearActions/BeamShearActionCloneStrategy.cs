using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using System;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    internal class BeamShearActionCloneStrategy : ICloneStrategy<IBeamShearAction>
    {
        private IUpdateStrategy<IBeamShearAction> updateStrategy;
        private IUpdateStrategy<IBeamShearAction> UpdateStrategy => updateStrategy ??= new BeamShearActionUpdateStrategy();

        public IBeamShearAction GetClone(IBeamShearAction sourceObject, CloneMode cloneMode = CloneMode.NewEntity, CloneDepth cloneDepth = CloneDepth.Deep)
        {
            CheckObject.ThrowIfNull(sourceObject);
            if (cloneMode == CloneMode.Reference) { return sourceObject; }
            Guid id = GuidService.GetGuid(sourceObject, cloneMode);
            BeamShearAction targetObject = new(id);
            updateStrategy.Update(targetObject, sourceObject);
            return targetObject;
        }
    }
}
