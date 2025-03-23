using StructureHelperCommon.Infrastructures.Interfaces;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    internal interface IShearForceLogic : ILogic
    {
        double GetShearForce();

    }
}
