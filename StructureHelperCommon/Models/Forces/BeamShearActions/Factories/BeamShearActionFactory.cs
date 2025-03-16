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
            SetAction(beamShearAction.XAxisShearAction, 0);
            SetFactors(beamShearAction.XAxisShearAction);
            beamShearAction.XAxisShearAction.ShearLoads.Clear();

            SetAction(beamShearAction.YAxisShearAction, 1e5);
            SetFactors(beamShearAction.YAxisShearAction);
            return beamShearAction;
        }

        private static void SetAction(IBeamShearAxisAction beamShearAxisAction, double supportForce)
        {
            beamShearAxisAction.SupportShearForce = 1e5;
            IBeamShearLoad distributedLoad = BeamShearLoadFactory.GetBeamShearLoad(ShearLoadTypes.DistributedLoad);
            beamShearAxisAction.ShearLoads.Add(distributedLoad);
        }

        private static void SetFactors(IBeamShearAxisAction beamShearAxisAction)
        {
            beamShearAxisAction.FactoredCombinationProperty.LimitState = Infrastructures.Enums.LimitStates.ULS;
            beamShearAxisAction.FactoredCombinationProperty.CalcTerm = Infrastructures.Enums.CalcTerms.ShortTerm;
            beamShearAxisAction.FactoredCombinationProperty.LongTermFactor = 0.95;
            beamShearAxisAction.FactoredCombinationProperty.ULSFactor = 1.2;
        }
    }
}
