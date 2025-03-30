using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    internal interface IShearForceLogic : ILogic
    {
        IForceTuple GetShearForce();

    }
}
