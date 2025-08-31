using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class InclinedSectionResultListLogic : IInclinedSectionResultListLogic
    {
        private IBeamShearSectionLogic beamShearSectionLogic;

        public InclinedSectionResultListLogic(IBeamShearSectionLogic beamShearSectionLogic)
        {
            this.beamShearSectionLogic = beamShearSectionLogic;
        }

        public List<IBeamShearSectionLogicResult> GetInclinedSectionResults(List<IBeamShearSectionLogicInputData> sectionInputDatas)
        {
            List<IBeamShearSectionLogicResult> sectionResults = new();
            foreach (var item in sectionInputDatas)
            {
                IBeamShearSectionLogicResult sectionResult = CalculateInclinedSectionResult(item);
                sectionResults.Add(sectionResult);
            }
            return sectionResults;
        }

        private IBeamShearSectionLogicResult CalculateInclinedSectionResult(IBeamShearSectionLogicInputData sectionInputData)
        {
            beamShearSectionLogic.InputData = sectionInputData;
            beamShearSectionLogic.Run();
            var sectionResult = beamShearSectionLogic.Result as IBeamShearSectionLogicResult;
            return sectionResult;
        }
    }
}
