using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    internal class StirrupGroupStrengthLogic : IBeamShearStrenghLogic
    {
        private readonly IBeamShearSectionLogicInputData sourceInputData;
        private IBeamShearSectionLogicInputData localInputData;
        private IStirrupGroup stirrupGroup;
        private IUpdateStrategy<IBeamShearSectionLogicInputData> inputDataUpdateStrategy;

        public StirrupGroupStrengthLogic(IBeamShearSectionLogicInputData inputData, IStirrupGroup stirrupGroup, IShiftTraceLogger? traceLogger)
        {
            this.sourceInputData = inputData;
            this.stirrupGroup = stirrupGroup;
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public double GetShearStrength()
        {
            Check();
            if (!stirrupGroup.Stirrups.Any()) { return 0.0; }
            GetLocalInputData();
            double shearStrength = 0;
            foreach (var item in stirrupGroup.Stirrups)
            {
                localInputData.Stirrup = item;
                var stirrupSrengthLogic = new StirrupStrengthLogic(localInputData, TraceLogger?.GetSimilarTraceLogger(50));
                shearStrength += stirrupSrengthLogic.GetShearStrength();
            }
            TraceLogger?.AddMessage($"Total bearing capacity of group {stirrupGroup.Name} for shear Vtot = {shearStrength}(N)");
            return shearStrength;
        }

        private void Check()
        {
            if (stirrupGroup is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": Stirrup group");
            }
            if (stirrupGroup.Stirrups is null)
            {
                throw new StructureHelperException(ErrorStrings.ParameterIsNull + ": Stirrups");
            }
        }

        private void GetLocalInputData()
        {
            localInputData = new BeamShearSectionLogicInputData(Guid.Empty);
            inputDataUpdateStrategy ??= new BeamShearSectionLogicInputDataUpdateStrategy();
            inputDataUpdateStrategy.Update(localInputData, sourceInputData);
        }
    }
}
