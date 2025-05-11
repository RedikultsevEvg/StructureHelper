using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperCommon.Models.Forces.Logics
{
    public class GetCombinationsByFactoredTupleLogic : IGetCombinationsByFactoredTupleLogic
    {
        private ForceCombinationList? result;
        private IForceTuple? fullSLSTuple;
        private List<LimitStates> limitStates = new() { LimitStates.ULS, LimitStates.SLS };
        private List<CalcTerms> calcTerms = new() { CalcTerms.ShortTerm, CalcTerms.LongTerm };

        public IForceTuple? SourceForceTuple { get; set; }
        public IFactoredCombinationProperty? CombinationProperty { get; set; }
        public IForceActionProperty? ForceActionProperty { get; set; } = new ForceActionProperty() { ForcePoint = new Point2D(), SetInGravityCenter = false };
        public IForceCombinationList GetCombinationList()
        {
            Check();
            GetFullSLSTuple();
            GetNewResult();
            ProcessResult();
            return result;
        }

        private void GetFullSLSTuple()
        {
            double factor = 1d;
            if (CombinationProperty.CalcTerm == CalcTerms.LongTerm)
            {
                factor /= CombinationProperty.LongTermFactor;
            }
            if (CombinationProperty.LimitState == LimitStates.ULS)
            {
                factor /= CombinationProperty.ULSFactor;
            }
            fullSLSTuple = ForceTupleService.MultiplyTupleByFactor(SourceForceTuple, factor);
        }

        private void Check()
        {
            if (SourceForceTuple is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + $": {nameof(SourceForceTuple)}");
            }
            if (CombinationProperty is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + $": {nameof(CombinationProperty)}");
            }
            if (ForceActionProperty is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + $": {nameof(ForceActionProperty)}");
            }
        }

        private void ProcessResult()
        {
            foreach (var limitState in limitStates)
            {
                ProcessLimitState(limitState);
            }
        }

        private void ProcessLimitState(LimitStates limitState)
        {
            var factor = limitState is LimitStates.SLS ? 1d : CombinationProperty.ULSFactor;
            foreach (var calcTerm in calcTerms)
            {
                ProcessCalcTerm(limitState, factor, calcTerm);
            }
        }

        private void ProcessCalcTerm(LimitStates limitState, double stateFactor, CalcTerms calcTerm)
        {
            var factor = calcTerm is CalcTerms.ShortTerm ? 1d : CombinationProperty.LongTermFactor;
            IForceTuple forceTuple = ForceTupleService.MultiplyTupleByFactor(fullSLSTuple, stateFactor * factor);
            var designForceTuple = new DesignForceTuple
            {
                LimitState = limitState,
                CalcTerm = calcTerm,
                ForceTuple = forceTuple
            };
            result.DesignForces.Add(designForceTuple);
        }

        private void GetNewResult()
        {
            result = new()
            {
                SetInGravityCenter = ForceActionProperty.SetInGravityCenter,
                ForcePoint = ForceActionProperty.ForcePoint
            };
            result.DesignForces.Clear();
        }


    }
}
