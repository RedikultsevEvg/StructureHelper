using StructureHelperCommon.Infrastructures.Interfaces;
using System;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    internal class BeamShearActionCloneStrategy : ICloneStrategy<IBeamShearAction>
    {
        private BeamShearAction targetObject;

        public IBeamShearAction GetClone(IBeamShearAction sourceObject)
        {
            targetObject = new(Guid.NewGuid());
            return targetObject;
        }
    }
}
