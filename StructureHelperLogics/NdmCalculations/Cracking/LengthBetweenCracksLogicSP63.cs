using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class LengthBetweenCracksLogicSP63 : ILengthBetweenCracksLogic
    {
        const double minDiameterFactor = 10d;
        const double maxDiameterFactor = 40d;
        const double minLength = 0.1d;
        const double maxLength = 0.4d;

        readonly IAverageDiameterLogic diameterLogic;
        readonly ITensileAreaLogic tensileAreaLogic;
        public IEnumerable<INdm> NdmCollection { get; set; }
        public IStrainMatrix StrainMatrix { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public LengthBetweenCracksLogicSP63(IAverageDiameterLogic diameterLogic, ITensileAreaLogic tensileAreaLogic)
        {
            this.diameterLogic = diameterLogic;
            this.tensileAreaLogic = tensileAreaLogic;
        }
        public LengthBetweenCracksLogicSP63() :
            this
            (   new AverageDiameterLogic(),
                new TensileAreaLogicSP63())
        {      }
        public double GetLength()
        {
            var rebars = NdmCollection
                .Where(x => x is RebarNdm)
                .Select(x => x as RebarNdm);
            var rebarArea = rebars.Sum(x => x.Area * x.StressScale);
            diameterLogic.Rebars = rebars;
            var rebarDiameter = diameterLogic.GetAverageDiameter();
            tensileAreaLogic.NdmCollection = NdmCollection;
            tensileAreaLogic.StrainMatrix = StrainMatrix;
            var concreteArea = tensileAreaLogic.GetTensileArea();
            var length = concreteArea / rebarArea * rebarDiameter;
            length = new List<double> { length, minDiameterFactor * rebarDiameter, minLength }.Max();
            length = new List<double> { length, maxDiameterFactor * rebarDiameter, maxLength }.Min();
            return length;
        }
    }
}
