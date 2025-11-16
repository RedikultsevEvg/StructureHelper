using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

//Copyright (c) 2026 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByDensityUpdateStrategy : IUpdateStrategy<IStirrupByDensity>
    {
        private IUpdateStrategy<IStirrup>? baseUpdateStrategy;
        public void Update(IStirrupByDensity targetObject, IStirrupByDensity sourceObject)
        {
            CheckObject.ThrowIfNull(targetObject);
            CheckObject.ThrowIfNull(sourceObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; }
            baseUpdateStrategy ??= new StirrupBaseUpdateStrategy();
            baseUpdateStrategy.Update(targetObject, sourceObject);
            targetObject.StirrupDensity = sourceObject.StirrupDensity;
            targetObject.StartCoordinate = sourceObject.StartCoordinate;
            targetObject.EndCoordinate = sourceObject.EndCoordinate;
        }
    }
}
