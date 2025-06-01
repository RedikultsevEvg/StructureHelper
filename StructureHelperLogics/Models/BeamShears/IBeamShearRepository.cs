using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IBeamShearRepository : ISaveable, IHasBeamShearActions, IHasCalculators, IHasBeamShearSections, IHasStirrups, ICloneable
    {
        
    }
}
