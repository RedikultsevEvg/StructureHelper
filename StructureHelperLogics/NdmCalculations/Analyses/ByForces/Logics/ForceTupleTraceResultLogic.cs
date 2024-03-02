using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.ResultData;
using LoaderCalculator.Logics;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Infrastructures.Exceptions;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceTupleTraceResultLogic : IForceTupleTraceResultLogic
    {
        private ILoaderResults calcResult;
        private IEnumerable<INdm> ndmCollection;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public ForceTupleTraceResultLogic(IEnumerable<INdm> ndmCollection)
        {
            this.ndmCollection = ndmCollection;
        }

        public void TraceResult(IResult result)
        {
            if (result is not IForcesTupleResult)
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(result));
            }
            calcResult = ((IForcesTupleResult)result).LoaderResults;
            TraceLogger?.AddMessage($"Analysis is done succsesfully");
            TraceLogger?.AddMessage($"Current accuracy {calcResult.AccuracyRate} has achieved in {calcResult.IterationCounter} iteration", TraceLogStatuses.Debug);
            var strainMatrix = calcResult.ForceStrainPair.StrainMatrix;
            var stiffness = new StiffnessLogic().GetStiffnessMatrix(ndmCollection, strainMatrix);
            TraceLogger?.AddMessage(string.Format("Next strain were obtained kx = {0}, ky = {1}, epsz = {2}", strainMatrix.Kx, strainMatrix.Ky, strainMatrix.EpsZ));
            TraceMinMaxStrain(ndmCollection, strainMatrix);
            TraceStrainAndStiffness(strainMatrix, stiffness);
        }

        private void TraceMinMaxStrain(IEnumerable<INdm> ndmCollection, IStrainMatrix strainMatrix)
        {
            var stressLogic = new StressLogic();
            double minStrain = double.PositiveInfinity, maxStrain = double.NegativeInfinity;
            Point2D minPoint = new(), maxPoint = new();
            foreach (var item in ndmCollection)
            {
                var strain = stressLogic.GetTotalStrain(strainMatrix, item);
                if (strain < minStrain)
                {
                    minStrain = strain;
                    minPoint = new () { X = item.CenterX, Y = item.CenterY };
                }

                if (strain > maxStrain)
                {
                    maxStrain = strain;
                    maxPoint = new () { X = item.CenterX, Y = item.CenterY };
                }
            }
            TraceLogger?.AddMessage(string.Format("Max strain EpsilonMax = {0}, at point x = {1}, y = {2}", maxStrain, maxPoint.X, maxPoint.Y), TraceLogStatuses.Debug);
            TraceLogger?.AddMessage(string.Format("Min strain EpsilonMin = {0}, at point x = {1}, y = {2}", minStrain, minPoint.X, minPoint.Y), TraceLogStatuses.Debug);
        }

        private void TraceStrainAndStiffness(IStrainMatrix strain, IStiffnessMatrix stiffness)
        {
            var exitMx = stiffness[0, 0] * strain.Kx + stiffness[0, 1] * strain.Ky + stiffness[0, 2] * strain.EpsZ;
            var exitMy = stiffness[1, 0] * strain.Kx + stiffness[1, 1] * strain.Ky + stiffness[1, 2] * strain.EpsZ;
            var exitNz = stiffness[2, 0] * strain.Kx + stiffness[2, 1] * strain.Ky + stiffness[2, 2] * strain.EpsZ;
            var PrestressMatrix = new ForceLogic()
                .GetPrestressMatrix(new StiffnessLogic(), ndmCollection, strain);
            double mx = exitMx + PrestressMatrix.Mx;
            double my = exitMy + PrestressMatrix.My;
            double nz = exitNz + PrestressMatrix.Nz;
            TraceStiffnessMatrix(strain, stiffness, exitMx, exitMy, exitNz);
            TracePrestressMatrix(exitMx, exitMy, exitNz, PrestressMatrix, mx, my, nz);
            TraceOutputForceCombination(mx, my, nz);
        }

        private void TraceOutputForceCombination(double mx, double my, double nz)
        {
            TraceLogger?.AddMessage($"Output force combination");
            var outputTuple = new ForceTuple()
            {
                Mx = mx,
                My = my,
                Nz = nz
            };
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(outputTuple));
        }

        private void TracePrestressMatrix(double exitMx, double exitMy, double exitNz, IForceMatrix PrestressMatrix, double mx, double my, double nz)
        {
            string prestressMatrix = string.Format("Prestress force matrix for current strain matrix: Mx = {0}, My = {1}, Nz = {2}",
                PrestressMatrix.Mx,
                PrestressMatrix.My,
                PrestressMatrix.Nz);
            TraceLogger?.AddMessage(prestressMatrix);
            string prestressForce = "Summary force matrix:";
            prestressForce += string.Format("\nMx = {0} + ({1}) = {2},\nMy = {3} + ({4}) = {5},\nNz = {6} + ({7}) = {8}",
                exitMx, PrestressMatrix.Mx, mx,
                exitMy, PrestressMatrix.My, my,
                exitNz, PrestressMatrix.Nz, nz
                );
            TraceLogger?.AddMessage(prestressForce, TraceLogStatuses.Debug);
        }

        private void TraceStiffnessMatrix(IStrainMatrix strain, IStiffnessMatrix stiffness, double exitMx, double exitMy, double exitNz)
        {
            TraceLogger?.AddMessage("Stiffness matrix");
            TraceLogger?.AddMessage(string.Format("D11 = {0}, D12 = {1}, D13 = {2}", stiffness[0, 0], stiffness[0, 1], stiffness[0, 2]));
            TraceLogger?.AddMessage(string.Format("D21 = {0}, D22 = {1}, D23 = {2}", stiffness[1, 0], stiffness[1, 1], stiffness[1, 2]));
            TraceLogger?.AddMessage(string.Format("D31 = {0}, D32 = {1}, D33 = {2}", stiffness[2, 0], stiffness[2, 1], stiffness[2, 2]));
            TraceLogger?.AddMessage("Checking equilibrium equations");
            TraceLogger?.AddMessage(string.Format("D11 * kx + D12 * ky + D13 * epsz =\n {0} * {1} + {2} * {3} + {4} * {5} = {6}",
                stiffness[0, 0], strain.Kx,
                stiffness[0, 1], strain.Ky,
                stiffness[0, 2], strain.EpsZ,
                exitMx
                ));
            TraceLogger?.AddMessage(string.Format("D12 * kx + D22 * ky + D23 * epsz =\n {0} * {1} + {2} * {3} + {4} * {5} = {6}",
                stiffness[1, 0], strain.Kx,
                stiffness[1, 1], strain.Ky,
                stiffness[1, 2], strain.EpsZ,
                exitMy
                ));
            TraceLogger?.AddMessage(string.Format("D31 * kx + D32 * ky + D33 * epsz =\n {0} * {1} + {2} * {3} + {4} * {5} = {6}",
                stiffness[2, 0], strain.Kx,
                stiffness[2, 1], strain.Ky,
                stiffness[2, 2], strain.EpsZ,
                exitNz
                ));
        }
    }
}
