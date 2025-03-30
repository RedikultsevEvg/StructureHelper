using StructureHelperCommon.Infrastructures.Exceptions;
using System;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public enum ShearLoadTypes
    {
        DistributedLoad,
        ConcentratedForce
    }
    public static class BeamShearLoadFactory
    {
        public static IBeamSpanLoad GetBeamShearLoad(ShearLoadTypes loadType)
        {
            if (loadType == ShearLoadTypes.DistributedLoad)
            {
                return GetDistributedLoad();
            }
            else if (loadType == ShearLoadTypes.ConcentratedForce)
            {
                return GetConcentratedForce();
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(loadType));
            }
        }

        private static ConcentratedForce GetConcentratedForce()
        {
            ConcentratedForce concentratedForce = new(Guid.NewGuid())
            {
                Name = "Concentrated force",
                LoadRatio = 1,
                RelativeLoadLevel = 0.5,
                ForceCoordinate = 1
            };
            concentratedForce.ForceValue.Qy = -5e4;
            return concentratedForce;
        }

        private static DistributedLoad GetDistributedLoad()
        {
            DistributedLoad distributedLoad = new(Guid.NewGuid())
            {
                Name = "Distributed load",
                LoadRatio = 1,
                RelativeLoadLevel = 0.5,
                StartCoordinate = 0,
                EndCoordinate = 100
            };
            distributedLoad.LoadValue.Qy = -5e3;
            return distributedLoad;
        }
    }
}
