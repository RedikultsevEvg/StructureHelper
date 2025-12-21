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
        private IUpdateStrategy<ILibMaterial> LibUpdateStrategy => libUpdateStrategy ??= new LibMaterialUpdateStrategy();
        public ReinforcementLibUpdateStrategy(IUpdateStrategy<ILibMaterial> libUpdateStrategy)
        {
            this.libUpdateStrategy = libUpdateStrategy;
        }
        public ReinforcementLibUpdateStrategy()
        {
            
        }
        public void Update(IReinforcementLibMaterial targetObject, IReinforcementLibMaterial sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject);
            CheckObject.ThrowIfNull(targetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            LibUpdateStrategy.Update(targetObject, sourceObject);
        }
    }
}
