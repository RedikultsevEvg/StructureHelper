using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics;
using LoaderCalculator.Logics.Geometry;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.Materials;
using StructureHelperLogics.NdmCalculations.Analyses;
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
        public IShiftTraceLogger? TraceLogger { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        private (double EtaAlongX, double EtaAlongY) GetBucklingCoefficients()
        {
            var stiffness = GetStiffness();
            criticalForceLogic.LongitudinalForce = options.CalcForceTuple.Nz;
            criticalForceLogic.StiffnessEI = stiffness.DX;
            criticalForceLogic.DesignLength = options.CompressedMember.GeometryLength * options.CompressedMember.LengthFactorY;
            var etaAlongY = criticalForceLogic.GetEtaFactor();
            criticalForceLogic.StiffnessEI = stiffness.DY;
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
            if (forceTuple.Nz >= 0) { return (new ConstDeltaELogic(), new ConstDeltaELogic()); }
            var eccentricityAlongX = options.CalcForceTuple.My / forceTuple.Nz;
            var eccentricityAlongY = options.CalcForceTuple.Mx / forceTuple.Nz;
            var sizeAlongX = ndmCollection.Max(x => x.CenterX) - ndmCollection.Min(x => x.CenterX);
            var sizeAlongY = ndmCollection.Max(x => x.CenterY) - ndmCollection.Min(x => x.CenterY);
            var DeltaElogicAboutX = new DeltaELogicSP63(eccentricityAlongY, sizeAlongY);
            var DeltaElogicAboutY = new DeltaELogicSP63(eccentricityAlongX, sizeAlongX);
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

            var concreteInertia = GeometryOperations.GetReducedMomentsOfInertia(concreteNdms, gravityCenter);
            var otherInertia = GeometryOperations.GetReducedMomentsOfInertia(otherNdms, gravityCenter);

            var stiffnessX = stiffnessLogicX.GetStiffnessCoeffitients();
            var dX = stiffnessX.Kc * concreteInertia.EIx  + stiffnessX.Ks * otherInertia.EIx;

            var stiffnessY = stiffnessLogicY.GetStiffnessCoeffitients();
            var dY = stiffnessY.Kc * concreteInertia.EIy + stiffnessY.Ks * otherInertia.EIy;

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
            return point;
        }

        private IForceTupleCalculator GetForceCalculator()
        {
            var tuple = options.CalcForceTuple;
            IForceTupleInputData inputData = new ForceTupleInputData() { NdmCollection = ndmCollection, Tuple = tuple, Accuracy = Accuracy };
            IForceTupleCalculator calculator = new ForceTupleCalculator(inputData);
            return calculator;
        }

        public void Run()
        {
            var checkResult = CheckInputData();
            if (checkResult != "")
            {
                Result = new ConcreteBucklingResult() { IsValid = false, Description = checkResult };
                return;
            }
            else
            {
                IConcretePhiLLogic phiLLogic = GetPhiLogic();
                var (DeltaLogicAboutX, DeltaLogicAboutY) = GetDeltaLogics();
                stiffnessLogicX = new RCStiffnessLogicSP63(phiLLogic, DeltaLogicAboutX);
                stiffnessLogicY = new RCStiffnessLogicSP63(phiLLogic, DeltaLogicAboutY);
                criticalForceLogic = new EilerCriticalForceLogic();

                var (EtaFactorX, EtaFactorY) = GetBucklingCoefficients();
                Result = new ConcreteBucklingResult()
                {
                    IsValid = true,
                    EtaFactorAlongX = EtaFactorX,
                    EtaFactorAlongY = EtaFactorY
                };
            }
        }

        private string CheckInputData()
        {
            string result = "";
            IForceTupleCalculator calculator = GetForceCalculator();
            calculator.Run();
            forcesResults = calculator.Result as IForcesTupleResult;
            if (forcesResults.IsValid != true)
            {
                result += "Bearind capacity of crosssection is not enough for initial forces\n";
            }
            return result;
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
