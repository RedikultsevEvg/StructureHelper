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
            CheckObject.IsNull(entity.Calculators);
            entity.Calculators.Clear();
            CheckObject.IsNull(entity.Actions);
            entity.Actions.Clear();
            CheckObject.IsNull(entity.Sections);
            entity.Sections.Clear();
            CheckObject.IsNull(entity.Stirrups);
            entity.Stirrups.Clear();
        }
    }
}
