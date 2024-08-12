using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Triangulations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Loggers;
using LoaderCalculator.Data.Materials;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    internal class CrackWidthLogicInputDataFactory : ILogic
    {
        private const double longTermFactor = 1.4d;
        private const double shortTermFactor = 1d;
        private IStressStateFactorLogic stressStateFactorLogic;
        private ICrackSofteningLogic softeningLogic;

        public double RebarStrain { get; set; }
        public double ConcreteStrain { get; set;}

        public CalcTerms CalcTerm { get; set; }
        public IRebarCrackInputData InputData { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public CrackWidthLogicInputDataFactory(ICrackSofteningLogic softeningLogic)
        {
            this.softeningLogic = softeningLogic;
        }

        public ICrackWidthLogicInputData GetCrackWidthLogicInputData()
        {
            stressStateFactorLogic = new StressStateFactorLogic()
            {
                ForceTuple = InputData.ForceTuple,
                TraceLogger = TraceLogger?.GetSimilarTraceLogger(50)
            };
            CrackWidthLogicInputDataSP63 data = new();
            if (CalcTerm == CalcTerms.LongTerm)
            {
                data.TermFactor = longTermFactor;
            }
            else
            {
                data.TermFactor = shortTermFactor;
            }
            data.PsiSFactor = softeningLogic.GetSofteningFactor();
            data.StressStateFactor = stressStateFactorLogic.GetStressStateFactor();
            data.BondFactor = 0.5d;
            data.LengthBetweenCracks = InputData.LengthBeetwenCracks;
            data.ConcreteStrain = ConcreteStrain;
            data.RebarStrain = RebarStrain;
            return data;
        }
    }
}
