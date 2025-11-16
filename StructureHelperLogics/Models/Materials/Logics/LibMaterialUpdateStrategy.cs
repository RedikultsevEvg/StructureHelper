using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Services;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.Materials
{
    public class LibMaterialUpdateStrategy : IUpdateStrategy<ILibMaterial>
    {
        public void Update(ILibMaterial targetObject, ILibMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            targetObject.MaterialEntity = sourceObject.MaterialEntity;
            targetObject.MaterialLogic = sourceObject.MaterialLogic;

        }
    }
}
