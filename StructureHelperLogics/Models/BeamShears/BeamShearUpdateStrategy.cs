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
            CheckObject.IsNull(sourceObject, ErrorStrings.SourceObject);
            CheckObject.IsNull(targetObject, ErrorStrings.TargetObject);
            if (ReferenceEquals(targetObject, sourceObject)) { return; };
            CheckObject.IsNull(sourceObject.Repository);
            CheckObject.IsNull(targetObject.Repository);
            InitializeStrategies();
            clearStrategy.Process(targetObject.Repository);
            repositoryUpdateStrategy.Update(targetObject.Repository, sourceObject.Repository);
        }

        private void InitializeStrategies()
        {
            repositoryUpdateStrategy ??= new BeamShearRepositoryAddUpdateStrategy();
            clearStrategy ??= new BeamShearReporitoryClearStrategy();
        }
    }
}
