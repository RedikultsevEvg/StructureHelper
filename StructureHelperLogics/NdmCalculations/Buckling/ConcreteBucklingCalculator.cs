using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using LoaderCalculator.Logics.Geometry;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.NdmCalculations.Buckling
{
    public class ConcreteBucklingCalculator : IConcreteBucklingCalculator
    {
        private IConcreteBucklingOptions options;
        private IEilerCriticalForceLogic criticalForceLogic;
        private IRCStiffnessLogic stiffnessLogicX, stiffnessLogicY;
        private List<INdm> ndmCollection;
        private List<INdm> concreteNdms;
        private List<INdm> otherNdms;
        IForcesTupleResult forcesResults;

        public string Name { get; set; }

        public IResult Result { get; private set; }

        public IAccuracy Accuracy { get; set; }
        public Action<IResult> ActionToOutputResults { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IShiftTraceLogger? TraceLogger { get; set; }

        private (double EtaAlongX, double EtaAlongY) GetBucklingCoefficients()
        {
            var (DX, DY) = GetStiffness();
            criticalForceLogic.LongitudinalForce = options.CalcForceTuple.Nz;
            criticalForceLogic.StiffnessEI = DX;
            criticalForceLogic.DesignLength = options.CompressedMember.GeometryLength * options.CompressedMember.LengthFactorY;
            var etaAlongY = criticalForceLogic.GetEtaFactor();
            criticalForceLogic.StiffnessEI = DY;
            criticalForceLogic.DesignLength = options.CompressedMember.GeometryLength * options.CompressedMember.LengthFactorX;
            var etaAlongX = criticalForceLogic.GetEtaFactor();
            return (etaAlongX, etaAlongY);
        }

        public ConcreteBucklingCalculator(IConcreteBucklingOptions options, IAccuracy accuracy)
        {
            this.options = options;
            Accuracy = accuracy;

            var allPrimitives = options.Primitives;
            var concretePrimitives = GetConcretePrimitives();
            var otherPrimitives = allPrimitives.Except(concretePrimitives);
            ndmCollection = NdmPrimitivesService.GetNdms(allPrimitives, options.LimitState, options.CalcTerm);
            concreteNdms = NdmPrimitivesService.GetNdms(concretePrimitives, options.LimitState, options.CalcTerm);
            otherNdms = NdmPrimitivesService.GetNdms(otherPrimitives, options.LimitState, options.CalcTerm);
        }

        private (IConcreteDeltaELogic DeltaLogicX, IConcreteDeltaELogic DeltaLogicY) GetDeltaLogics()
        {
            IForceTuple forceTuple = options.CalcForceTuple;
            if (forceTuple.Nz >= 0)
            {
                return (new ConstDeltaELogic(), new ConstDeltaELogic());
            }
            var eccentricityAlongX = options.CalcForceTuple.My / forceTuple.Nz;
            var eccentricityAlongY = options.CalcForceTuple.Mx / forceTuple.Nz;
            var sizeAlongX = ndmCollection.Max(x => x.CenterX) - ndmCollection.Min(x => x.CenterX);
            var sizeAlongY = ndmCollection.Max(x => x.CenterY) - ndmCollection.Min(x => x.CenterY);
            var DeltaElogicAboutX = new DeltaELogicSP63(eccentricityAlongY, sizeAlongY);
            var DeltaElogicAboutY = new DeltaELogicSP63(eccentricityAlongX, sizeAlongX);
            if (TraceLogger is not null)
            {
                DeltaElogicAboutX.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
                DeltaElogicAboutY.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
            }
            return (DeltaElogicAboutX, DeltaElogicAboutY);
        }

        private IEnumerable<INdmPrimitive> GetConcretePrimitives()
        {
            var primitives = options.Primitives.Where(x => x.HeadMaterial.HelperMaterial is IConcreteLibMaterial);
            return primitives;
        }

        private (double DX, double DY) GetStiffness()
        {
            var gravityCenter = GeometryOperations.GetGravityCenter(ndmCollection);
            string message = string.Format("Gravity center, x = {0}, y = {1}", gravityCenter.Cx, gravityCenter.Cy);
            TraceLogger?.AddMessage(message);
            var (EIx, EIy) = GeometryOperations.GetReducedMomentsOfInertia(concreteNdms, gravityCenter);
            TraceLogger.AddMessage(string.Format("Summary stiffness of concrete parts EIx,c = {0}", EIx));
            TraceLogger.AddMessage(string.Format("Summary stiffness of concrete parts EIy,c = {0}", EIy));
            var otherInertia = GeometryOperations.GetReducedMomentsOfInertia(otherNdms, gravityCenter);
            TraceLogger.AddMessage(string.Format("Summary stiffness of nonconcrete parts EIx,s = {0}", otherInertia.EIy));
            TraceLogger.AddMessage(string.Format("Summary stiffness of nonconcrete parts EIy,s = {0}", otherInertia.EIy));

            var (Kc, Ks) = stiffnessLogicX.GetStiffnessCoeffitients();
            var dX = Kc * EIx  + Ks * otherInertia.EIx;
            string mesDx = string.Format("Summary stiffness Dx = Kc * EIx,c + Ks * EIx,s = {0} * {1} + {2} * {3} = {4}",
                Kc, EIx, Ks, otherInertia.EIx, dX);
            TraceLogger.AddMessage(mesDx);

            var stiffnessY = stiffnessLogicY.GetStiffnessCoeffitients();
            var dY = stiffnessY.Kc * EIy + stiffnessY.Ks * otherInertia.EIy;
            string mesDy = string.Format("Summary stiffness Dy = Kc * EIy,c + Ks * EIy,s = {0} * {1} + {2} * {3} = {4}",
                stiffnessY.Kc, EIy, stiffnessY.Ks, otherInertia.EIy, dY);
            TraceLogger.AddMessage(mesDy);
            return (dX, dY);
        }

        private IConcretePhiLLogic GetPhiLogic()
        {
            IPoint2D point = GetMostTensionedPoint();
            var phiLogic = new PhiLogicSP63(options.CalcForceTuple, options.LongTermTuple, point);
            return phiLogic;
        }

        private IPoint2D GetMostTensionedPoint()
        {
            var strains = forcesResults.LoaderResults.StrainMatrix;
            double maxStrain = double.NegativeInfinity;
            IPoint2D point = new Point2D();
            var stressLogic = new StressLogic();
            foreach (var item in ndmCollection)
            {
                var strain = stressLogic.GetTotalStrain(strains, item); 
                if (strain > maxStrain)
                {
                    maxStrain = strain;
                    point = new Point2D() { X = item.CenterX, Y = item.CenterY };
                }
            }
            TraceLogger.AddMessage(string.Format("Most tensioned (minimum compressed) point: x = {0}, y = {1}", point.X, point.Y));
            TraceLogger.AddMessage(string.Format("Strain: epsilon = {0}", maxStrain), TraceLogStatuses.Debug);
            return point;
        }

        private IForceTupleCalculator GetForceCalculator()
        {
            var tuple = options.CalcForceTuple;
            IForceTupleInputData inputData = new ForceTupleInputData()
            {
                NdmCollection = ndmCollection,
                Tuple = tuple, Accuracy = Accuracy
            };
            IForceTupleCalculator calculator = new ForceTupleCalculator(inputData);
            return calculator;
        }

        public void Run()
        {
            TraceLogger?.AddMessage(LoggerStrings.CalculatorType(this), TraceLogStatuses.Service);
            TraceLogger?.AddMessage(LoggerStrings.MethodBasedOn + "SP63.13330.2018");
            var checkResult = CheckInputData();
            if (checkResult != "")
            {
                TraceLogger?.AddMessage(checkResult, TraceLogStatuses.Error);
                Result = new ConcreteBucklingResult()
                {
                    IsValid = false,
                    Description = checkResult,
                    EtaFactorAlongX = double.PositiveInfinity,
                    EtaFactorAlongY = double.PositiveInfinity
                };
                return;
            }
            else
            {
                var phiLLogic = GetPhiLogic();
                var (DeltaLogicAboutX, DeltaLogicAboutY) = GetDeltaLogics();
                stiffnessLogicX = new RCStiffnessLogicSP63(phiLLogic, DeltaLogicAboutX);
                stiffnessLogicY = new RCStiffnessLogicSP63(phiLLogic, DeltaLogicAboutY);
                if (TraceLogger is not null)
                {
                    stiffnessLogicX.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
                    stiffnessLogicY.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
                }
                criticalForceLogic = new EilerCriticalForceLogic();
                if (TraceLogger is not null)
                {
                    criticalForceLogic.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
                }

                var (EtaFactorX, EtaFactorY) = GetBucklingCoefficients();
                var messageString = "Eta factor orbitrary {0} axis, Etta{0} = {1} (dimensionless)";
                var messageStringX = string.Format(messageString, "X", EtaFactorX);
                var messageStringY = string.Format(messageString, "Y", EtaFactorY);
                TraceLogger?.AddMessage(messageStringX);
                TraceLogger?.AddMessage(messageStringY);
                Result = new ConcreteBucklingResult()
                {
                    IsValid = true,
                    EtaFactorAlongX = EtaFactorX,
                    EtaFactorAlongY = EtaFactorY
                };
            }
            TraceLogger?.AddMessage(LoggerStrings.CalculationHasDone);
        }

        private string CheckInputData()
        {
            string result = "";
            var tuple = options.CalcForceTuple;
            if (tuple.Nz >= 0d)
            {
                result += $"Force Nz = {tuple.Nz} must negative in compression";
                return result;
            }
            IForceTupleCalculator calculator = GetForceCalculator();
            calculator.Run();
            forcesResults = calculator.Result as IForcesTupleResult;
            if (forcesResults.IsValid != true)
            {
                result += "Bearind capacity of cross-section is not enough for initial forces\n";
                TraceLogger?.AddMessage("Initial forces", TraceLogStatuses.Error);
                TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(tuple));
            }
            return result;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
