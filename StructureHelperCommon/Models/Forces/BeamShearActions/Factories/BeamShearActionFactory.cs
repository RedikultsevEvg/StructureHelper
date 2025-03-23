using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces.BeamShearActions
{
    public enum ShearActionTypes
    {
        DistributedLoad
    }
    /// <summary>
    /// Generates beam shear action with default settings
    /// </summary>
    public static class BeamShearActionFactory
    {
        public static IBeamShearAction GetBeamShearAction(ShearActionTypes actionType)
        {
            if (actionType == ShearActionTypes.DistributedLoad)
            {
                return GetDistributedLoad();
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(actionType));
            }
        }

        private static IBeamShearAction GetDistributedLoad()
        {
            BeamShearAction beamShearAction = new(Guid.NewGuid())
            {
                Name = "Beam shear action"
            };
            SetExternalForceFactors(beamShearAction.ExternalForce.CombinationProperty);

            SetAction(beamShearAction.SupportAction, 1e5);
            SetFactors(beamShearAction.SupportAction);
            return beamShearAction;
        }

        private static void SetExternalForceFactors(IFactoredCombinationProperty combinationProperty)
        {
            combinationProperty.CalcTerm = CalcTerms.ShortTerm;
            combinationProperty.LimitState = LimitStates.ULS;
            combinationProperty.LongTermFactor = 1;
            combinationProperty.ULSFactor = 1;
        }

        private static void SetAction(IBeamShearAxisAction beamShearAxisAction, double supportForce)
        {
            beamShearAxisAction.SupportForce.ForceTuple.Qy = supportForce;
            IBeamSpanLoad distributedLoad = BeamShearLoadFactory.GetBeamShearLoad(ShearLoadTypes.DistributedLoad);
            beamShearAxisAction.ShearLoads.Add(distributedLoad);
        }

        private static void SetFactors(IBeamShearAxisAction beamShearAxisAction)
        {
            beamShearAxisAction.SupportForce.CombinationProperty.LimitState = LimitStates.ULS;
            beamShearAxisAction.SupportForce.CombinationProperty.CalcTerm = CalcTerms.ShortTerm;
            beamShearAxisAction.SupportForce.CombinationProperty.LongTermFactor = 0.95;
            beamShearAxisAction.SupportForce.CombinationProperty.ULSFactor = 1.2;
        }
    }
}
