using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public class GetDeflectionByCurvatureLogic : IGetDeflectionByCurvatureLogic
    {
        public GetDeflectionByCurvatureLogic(IShiftTraceLogger traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IShiftTraceLogger? TraceLogger { get; set; }

        public ICurvatureTermResult GetDeflection(IForceTuple curvature, IDeflectionFactor factor)
        {
            double L = factor.SpanLength;
            TraceLogger?.AddMessage($"Span length L = {L}(m)");

            var result = new CurvatureTermResult();

            result.DeflectionMx = ComputeDeflection(
                axisName: "X",
                curvature: curvature.Mx,
                k: factor.DeflectionFactors.Mx,
                ultimate: factor.MaxDeflections.Mx,
                spanLength: L,
                isQuadratic: true);

            result.DeflectionMy = ComputeDeflection(
                axisName: "Y",
                curvature: curvature.My,
                k: factor.DeflectionFactors.My,
                ultimate: factor.MaxDeflections.My,
                spanLength: L,
                isQuadratic: true);

            result.DeflectionNz = ComputeDeflection(
                axisName: "Z",
                curvature: curvature.Nz,
                k: factor.DeflectionFactors.Nz,
                ultimate: factor.MaxDeflections.Nz,
                spanLength: L,
                isQuadratic: false);

            return result;
        }

        private DeflectionResult ComputeDeflection(
            string axisName,
            double curvature,
            double k,
            double ultimate,
            double spanLength,
            bool isQuadratic)
        {
            double deflection = isQuadratic
                ? k * curvature * spanLength * spanLength
                : k * curvature * spanLength;

            string formula = isQuadratic
                ? $"{k} * {curvature} * ({spanLength})^2"
                : $"{k} * {curvature} * {spanLength}";

            TraceLogger?.AddMessage(
                $"Deflection along {axisName} axis = {formula} = {deflection}(m)");

            return new DeflectionResult
            {
                Curvature = curvature,
                UltimateDeflection = ultimate,
                Deflection = deflection
            };
        }

    }
}
