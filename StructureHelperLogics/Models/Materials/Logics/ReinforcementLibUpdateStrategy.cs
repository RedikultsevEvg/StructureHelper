using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Services;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

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
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            libUpdateStrategy.Update(targetObject, sourceObject);
        }
    }
}
