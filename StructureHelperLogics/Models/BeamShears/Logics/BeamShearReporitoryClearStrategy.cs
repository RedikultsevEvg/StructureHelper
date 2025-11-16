using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Services;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearReporitoryClearStrategy : IProcessStrategy<IBeamShearRepository>
    {
        public void Process(IBeamShearRepository entity)
        {
            CheckObject.ThrowIfNull(entity.Calculators);
            entity.Calculators.Clear();
            CheckObject.ThrowIfNull(entity.Actions);
            entity.Actions.Clear();
            CheckObject.ThrowIfNull(entity.Sections);
            entity.Sections.Clear();
            CheckObject.ThrowIfNull(entity.Stirrups);
            entity.Stirrups.Clear();
        }
    }
}
