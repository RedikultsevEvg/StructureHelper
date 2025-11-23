using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    internal interface ICurvatureCalcualtorLogic : ILogic
    {
        ICurvatureCalculatorInputData InputData {get;set;}
        IResult Result { get; }
        void Run();
    }
}
