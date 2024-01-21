using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces.Logics;
using StructureHelperLogics.NdmCalculations.Buckling;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculator : IForceCalculator, IHasActionByResult
    {
        static readonly ForceCalculatorUpdateStrategy updateStrategy = new();
        public string Name { get; set; }
        public List<LimitStates> LimitStatesList { get; }
        public List<CalcTerms> CalcTermsList { get; }
        public List<IForceAction> ForceActions { get; }
        public List<INdmPrimitive> Primitives { get; }
        public IResult Result { get; private set; }
        public ICompressedMember CompressedMember { get; }
        public IAccuracy Accuracy { get; set; }
        public List<IForceCombinationList> ForceCombinationLists { get; private set; }
        public Action<IResult> ActionToOutputResults { get; set; }
        public ITraceLogger? TraceLogger { get; set; }

        public void Run()
        {
            var checkResult = CheckInputData();
            if (checkResult != "")
            {
                Result = new ForcesResults() { IsValid = false, Description = checkResult };
                return;
            }
            else
            {
                GetCombinations();
                CalculateResult();
            }
        }

        private void CalculateResult()
        {
            var ndmResult = new ForcesResults() { IsValid = true };
            foreach (var combination in ForceCombinationLists)
            {
                foreach (var tuple in combination.DesignForces)
                {
                    var limitState = tuple.LimitState;
                    var calcTerm = tuple.CalcTerm;
                    if (LimitStatesList.Contains(limitState) & CalcTermsList.Contains(calcTerm))
                    {
                        var ndms = NdmPrimitivesService.GetNdms(Primitives, limitState, calcTerm);
                        IPoint2D point2D;
                        if (combination.SetInGravityCenter == true)
                        {
                            var loaderPoint = LoaderCalculator.Logics.Geometry.GeometryOperations.GetGravityCenter(ndms);
                            point2D = new Point2D() { X = loaderPoint.Cx, Y = loaderPoint.Cy };
                        }
                        else point2D = combination.ForcePoint;
                        var newTuple = ForceTupleService.MoveTupleIntoPoint(tuple.ForceTuple, point2D) as ForceTuple;
                        IForcesTupleResult result = GetPrimitiveStrainMatrix(ndms, newTuple);
                        if (CompressedMember.Buckling == true)
                        {
                            IForceTuple longTuple;
                            if (calcTerm == CalcTerms.LongTerm)
                            {
                                longTuple = newTuple;
                            }
                            else
                            {
                                longTuple = GetLongTuple(combination.DesignForces, limitState);
                            }
                            var bucklingCalculator = GetBucklingCalculator(CompressedMember, limitState, calcTerm, newTuple, longTuple);
                            try
                            {
                                bucklingCalculator.Run();
                                var bucklingResult = bucklingCalculator.Result as IConcreteBucklingResult;

                                if (bucklingResult.IsValid != true)
                                {
                                    result.IsValid = false;
                                    result.Description += $"Buckling result:\n{bucklingResult.Description}\n";
                                }
                                newTuple = CalculateBuckling(newTuple, bucklingResult);
                                result = GetPrimitiveStrainMatrix(ndms, newTuple);
                            }
                            catch (Exception ex)
                            {
                                result.IsValid = false;
                                result.Description = $"Buckling error:\n{ex}\n";
                            }
                        }
                        result.DesignForceTuple.LimitState = limitState;
                        result.DesignForceTuple.CalcTerm = calcTerm;
                        result.DesignForceTuple.ForceTuple = newTuple;
                        ndmResult.ForcesResultList.Add(result);
                        ActionToOutputResults?.Invoke(ndmResult);
                    }
                }
            }
            Result = ndmResult;
        }


        private void GetCombinations()
        {
            ForceCombinationLists = new List<IForceCombinationList>();
            foreach (var item in ForceActions)
            {
                ForceCombinationLists.Add(item.GetCombinations());
            }
        }

        private IForceTuple GetLongTuple(List<IDesignForceTuple> designForces, LimitStates limitState)
        {
            IForceTuple longTuple;
            try
            {
                longTuple = designForces.Where(x => x.LimitState == limitState & x.CalcTerm == CalcTerms.LongTerm).First().ForceTuple;
            }
            catch (Exception)
            {
                longTuple = new ForceTuple();
            }     
            return longTuple;
        }

        private IConcreteBucklingCalculator GetBucklingCalculator(ICompressedMember compressedMember, LimitStates limitStates, CalcTerms calcTerms, IForceTuple calcTuple, IForceTuple longTuple)
        {
            IConcreteBucklingOptions options = new ConcreteBucklingOptions()
            { CompressedMember = compressedMember,
                LimitState = limitStates,
                CalcTerm = calcTerms,
                CalcForceTuple = calcTuple,
                LongTermTuple = longTuple,
                Primitives = Primitives };
            IConcreteBucklingCalculator bucklingCalculator = new ConcreteBucklingCalculator(options, Accuracy);
            return bucklingCalculator;
        }

        private ForceTuple CalculateBuckling(ForceTuple calcTuple, IConcreteBucklingResult bucklingResult)
        {
            var newTuple = calcTuple.Clone() as ForceTuple;
            newTuple.Mx *= bucklingResult.EtaFactorAlongY;
            newTuple.My *= bucklingResult.EtaFactorAlongX;
            return newTuple;
        }


        private string CheckInputData()
        {
            string result = "";
            NdmPrimitivesService.CheckPrimitives(Primitives);
            if (ForceActions.Count == 0) { result += "Calculator does not contain any forces \n"; }
            if (LimitStatesList.Count == 0) { result += "Calculator does not contain any limit states \n"; }
            if (CalcTermsList.Count == 0) { result += "Calculator does not contain any duration \n"; }
            return result;
        }

        public ForceCalculator()
        {
            ForceActions = new List<IForceAction>();
            Primitives = new List<INdmPrimitive>();
            CompressedMember = new CompressedMember() { Buckling = false };
            Accuracy = new Accuracy() { IterationAccuracy = 0.001d, MaxIterationCount = 1000 };
            LimitStatesList = new List<LimitStates>() { LimitStates.ULS, LimitStates.SLS };
            CalcTermsList = new List<CalcTerms>() { CalcTerms.ShortTerm, CalcTerms.LongTerm };
        }

        private IForcesTupleResult GetPrimitiveStrainMatrix(IEnumerable<INdm> ndmCollection, IForceTuple tuple)
        {
            IForceTupleInputData inputData = new ForceTupleInputData() { NdmCollection = ndmCollection, Tuple = tuple, Accuracy = Accuracy };
            IForceTupleCalculator calculator = new ForceTupleCalculator(inputData);
            calculator.Run();
            return calculator.Result as IForcesTupleResult;
        }

        public object Clone()
        {
            var newCalculator = new ForceCalculator();
            updateStrategy.Update(newCalculator, this);
            return newCalculator;
        }
    }
}
