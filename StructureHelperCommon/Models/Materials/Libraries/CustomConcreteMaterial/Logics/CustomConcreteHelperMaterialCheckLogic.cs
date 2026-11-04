using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System.Collections.Generic;

namespace StructureHelperCommon.Models.Materials.Logics
{
    /// <summary>
    /// Checks a complete custom concrete helper material: all models, their options,
    /// and the rule that only one active model may exist for each state/term pair.
    /// </summary>
    public class CustomConcreteHelperMaterialCheckLogic
        : CheckEntityLogic<ICustomConcreteHelperMaterial>
    {
        private CustomConcreteModelCheckLogic modelCheckLogic;

        public override bool Check()
        {
            CheckResult = string.Empty;
            if (Entity is null)
            {
                TraceMessage("Custom concrete helper material is not assigned");
                return false;
            }
            if (Entity.MaterialModels is null || Entity.MaterialModels.Count == 0)
            {
                TraceMessage("Custom concrete material must contain at least one model");
                return false;
            }

            bool result = true;
            var activePairs = new HashSet<(LimitStates, CalcTerms)>();
            int activeCount = 0;

            foreach (ICustomConcreteModel model in Entity.MaterialModels)
            {
                if (model is null)
                {
                    TraceMessage("Custom concrete material contains a null model");
                    result = false;
                    continue;
                }

                modelCheckLogic ??= new CustomConcreteModelCheckLogic();
                modelCheckLogic.Entity = model;
                modelCheckLogic.TraceLogger = TraceLogger;
                if (!modelCheckLogic.Check())
                {
                    string modelName = string.IsNullOrWhiteSpace(model.Name) ? "<unnamed>" : model.Name;
                    TraceMessage($"Model '{modelName}' is not valid: {modelCheckLogic.CheckResult.Trim()}");
                    result = false;
                }

                if (model.IsActive && model.StateCalcTermPair is not null)
                {
                    activeCount++;
                    var pair = (
                        model.StateCalcTermPair.LimitState,
                        model.StateCalcTermPair.CalcTerm);
                    if (!activePairs.Add(pair))
                    {
                        TraceMessage(
                            $"More than one active model uses limit state {pair.LimitState} and calculation term {pair.CalcTerm}");
                        result = false;
                    }
                }
            }

            if (activeCount == 0)
            {
                TraceMessage("Custom concrete material must contain at least one active model");
                result = false;
            }

            return result;
        }
    }
}
