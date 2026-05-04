using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    /// <summary>
    /// Provides logic for calculating long-term and short-term deflection terms
    /// based on curvature values and deflection factors. Uses an underlying
    /// <see cref="IGetDeflectionByCurvatureLogic"/> to compute the deflection
    /// for a given curvature state.
    /// </summary>
    public class GetTermDeflectionLogic : IGetTermDeflectionLogic
    {
        private IGetDeflectionByCurvatureLogic deflectionLogic;
        private IGetDeflectionByCurvatureLogic DeflectionLogic => deflectionLogic ??= new GetDeflectionByCurvatureLogic(TraceLogger);
        private IForceTupleServiceLogic forceTupleServiceLogic;
        private IForceTupleServiceLogic ForceTupleServiceLogic => forceTupleServiceLogic ??= new ForceTupleServiceLogic();


        public IForceTuple LongCurvature { get; set; }
        public IForceTuple ShortLongCurvature { get; set; }
        public IForceTuple ShortFullCurvature { get; set; }
        public IDeflectionFactor DeflectionFactor { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }
        public GetTermDeflectionLogic(IShiftTraceLogger traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public GetTermDeflectionLogic(IGetDeflectionByCurvatureLogic deflectionLogic, IForceTupleServiceLogic forceTupleServiceLogic, IShiftTraceLogger? traceLogger)
        {
            this.deflectionLogic = deflectionLogic;
            this.forceTupleServiceLogic = forceTupleServiceLogic;
            TraceLogger = traceLogger;
        }

        public ICurvatureTermResult GetLongResult()
        {
            TraceCurvature(LongCurvature);
            ICurvatureTermResult longTermDeflection = DeflectionLogic.GetDeflection(LongCurvature, DeflectionFactor);
            return longTermDeflection;
        }

        private void TraceCurvature(IForceTuple curvature)
        {
            TraceLogger?.AddMessage($"Curvature kx = {curvature.Mx}(1/m), ky = {curvature.My}(1/m), epsz = {curvature.Nz}(dimensionless)");
        }

        public ICurvatureTermResult GetShortResult()
        {
            TraceLogger?.AddMessage($"Curvature from full short load with short-term stiffness");
            TraceCurvature(ShortFullCurvature);
            TraceLogger?.AddMessage($"Curvature from long-term load with short-term stiffness");
            TraceCurvature(ShortLongCurvature);
            var deltaShortCurvature = ForceTupleServiceLogic.SumTuples(ShortFullCurvature, ShortLongCurvature, -1);
            TraceLogger?.AddMessage($"Extra curvature with short-term stiffness");
            TraceCurvature(deltaShortCurvature);
            var shortCurvature = ForceTupleServiceLogic.SumTuples(LongCurvature, deltaShortCurvature);
            TraceLogger?.AddMessage($"Full short-term curvature");
            TraceCurvature(shortCurvature);
            ICurvatureTermResult shortTermDeflection = DeflectionLogic.GetDeflection(shortCurvature, DeflectionFactor);
            return shortTermDeflection;
        }
    }
}
