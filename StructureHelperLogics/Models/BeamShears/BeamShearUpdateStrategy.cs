using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;
using StructureHelperLogics.Models.BeamShears.Logics;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearUpdateStrategy : IUpdateStrategy<IBeamShear>
    {
        IProcessStrategy<IBeamShearRepository> clearStrategy;
        IUpdateStrategy<IBeamShearRepository> repositoryUpdateStrategy;
        public void Update(IBeamShear targetObject, IBeamShear sourceObject)
        {
            CheckObject.ThrowIfNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.ThrowIfNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            CheckObject.ThrowIfNull(sourceObject.Repository);
            CheckObject.ThrowIfNull(targetObject.Repository);
            InitializeStrategies();
            clearStrategy.Process(targetObject.Repository);
            repositoryUpdateStrategy.Update(targetObject.Repository, sourceObject.Repository);
        }

        private void InitializeStrategies()
        {
            clearStrategy ??= new BeamShearReporitoryClearStrategy();
            repositoryUpdateStrategy ??= new BeamShearRepositoryAddUpdateStrategy();
        }
    }
}
