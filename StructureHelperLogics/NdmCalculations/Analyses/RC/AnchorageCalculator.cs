using LoaderCalculator.Logics.ConcreteCalculations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.RC
{
    public class AnchorageCalculator : IAnchorageCalculator
    {
        private IAnchorageInputData inputData;
        private IAnchorage anchorage;
        
        public string Name { get; set; }
        public AnchorageCalculator(IAnchorageInputData inputData)
        {
            this.inputData = inputData;
            IAnchorageOptions anchorageOptions = GetAnchorageOptions();
            anchorage = new AnchorageSP632018Rev3(anchorageOptions);
        }

        public double GetBaseDevLength()
        {
            var val = anchorage.GetBaseDevLength();
            return val;
        }

        public double GetDevLength()
        {
            return anchorage.GetDevLength();
        }

        public double GetLapLength()
        {
            return anchorage.GetLapLength();
        }

        private IAnchorageOptions GetAnchorageOptions()
        {
            var anchorageOptions = new AnchorageOptionsSP63();
            anchorageOptions.ConcreteStrength = inputData.ConcreteStrength;
            anchorageOptions.ReinforcementStrength = inputData.ReinforcementStrength;
            anchorageOptions.FactorEta1 = inputData.FactorEta1;
            anchorageOptions.ReinforcementStress = inputData.ReinforcementStress;
            anchorageOptions.CrossSectionArea = inputData.CrossSectionArea;
            anchorageOptions.CrossSectionPerimeter = inputData.CrossSectionPerimeter;
            anchorageOptions.LappedCountRate = inputData.LappedCountRate;
            return anchorageOptions;
        }
    }
}
