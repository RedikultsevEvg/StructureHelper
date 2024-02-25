using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
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
        private readonly IForceTupleCalculator forceTupleCalculator;
        public string Name { get; set; }
        public List<LimitStates> LimitStatesList { get; private set; }
        public List<CalcTerms> CalcTermsList { get; private set; }
        public List<IForceAction> ForceActions { get; private set; }
        public List<INdmPrimitive> Primitives { get; private set; }
        public IResult Result { get; private set; }
        public ICompressedMember CompressedMember { get; private set; }
        public IAccuracy Accuracy { get; set; }
        public List<IForceCombinationList> ForceCombinationLists { get; private set; }
        public Action<IResult> ActionToOutputResults { get; set; }
        public IShiftTraceLogger? TraceLogger { get; set; }

        public void Run()
        {
            var checkResult = CheckInputData();
            if (checkResult != "")
            {
                Result = new ForcesResults()
                {
                    IsValid = false,
                    Description = checkResult
                };
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
                        ProcessNdmResult(ndmResult, combination, tuple, limitState, calcTerm);
                    }
                }
            }
            Result = ndmResult;
        }

        private void ProcessNdmResult(ForcesResults ndmResult, IForceCombinationList combination, IDesignForceTuple tuple, LimitStates limitState, CalcTerms calcTerm)
        {
            var ndms = NdmPrimitivesService.GetNdms(Primitives, limitState, calcTerm);
            IPoint2D point2D;
            if (combination.SetInGravityCenter == true)
            {
                var (Cx, Cy) = LoaderCalculator.Logics.Geometry.GeometryOperations.GetGravityCenter(ndms);
                point2D = new Point2D(){ X = Cx, Y = Cy };
            }
            else point2D = combination.ForcePoint;
            var newTuple = ForceTupleService.MoveTupleIntoPoint(tuple.ForceTuple, point2D);
            TraceLogger?.AddMessage($"Input force combination");
            TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(newTuple));
            if (CompressedMember.Buckling == true)
            {
                if (newTuple.Nz >= 0d)
                {
                    TraceLogger.AddMessage(string.Format("Second order effect is not considered as Nz={0} >= 0", newTuple.Nz));
                }
                else
                {
                    newTuple = ProcessAccEccentricity(ndms, newTuple);
                    newTuple = GetForceTupleByBuckling(ndmResult, combination, limitState, calcTerm, ndms, newTuple);
                }
                GetForceResult(ndmResult, limitState, calcTerm, ndms, newTuple);
            }
            else
            {
                if (newTuple.Nz < 0d)
                {
                    string message = string.Format("Second order effect is not considered, despite force Nz={0}", newTuple.Nz);
                    TraceLogger.AddMessage(message, TraceLogStatuses.Warning);
                }
                GetForceResult(ndmResult, limitState, calcTerm, ndms, newTuple);
            }
        }

        private IForceTuple GetForceTupleByBuckling(ForcesResults ndmResult, IForceCombinationList combination, LimitStates limitState, CalcTerms calcTerm, List<INdm> ndms, IForceTuple newTuple)
        {
            var inputData = new BucklingInputData()
            {
                Combination = combination,
                LimitState = limitState,
                CalcTerm = calcTerm,
                Ndms = ndms,
                ForceTuple = newTuple
            };
            var bucklingResult = ProcessBuckling(inputData);

            if (bucklingResult.IsValid != true)
            {
                TraceLogger?.AddMessage(bucklingResult.Description, TraceLogStatuses.Error);
                var result = new ForcesTupleResult
                {
                    IsValid = false,
                    Description = $"Buckling result:\n{bucklingResult.Description}\n",
                    DesignForceTuple = new DesignForceTuple()
                    {
                        ForceTuple = newTuple,
                        LimitState = limitState,
                        CalcTerm = calcTerm
                    }
                };
                ndmResult.ForcesResultList.Add(result);
            }
            else
            {
                newTuple = CalculateBuckling(newTuple, bucklingResult);
                TraceLogger?.AddMessage($"Force combination with considering of second order effects");
                TraceLogger?.AddEntry(new TraceTablesFactory().GetByForceTuple(newTuple));
            }

            return newTuple;
        }

        private void GetForceResult(ForcesResults ndmResult, LimitStates limitState, CalcTerms calcTerm, List<INdm> ndms, IForceTuple newTuple)
        {
            var result = GetPrimitiveStrainMatrix(ndms, newTuple, Accuracy);
            result.DesignForceTuple.LimitState = limitState;
            result.DesignForceTuple.CalcTerm = calcTerm;
            result.DesignForceTuple.ForceTuple = newTuple;
            ndmResult.ForcesResultList.Add(result);
            ActionToOutputResults?.Invoke(ndmResult);
        }

        private IForceTuple ProcessAccEccentricity(List<INdm> ndms, IForceTuple newTuple)
        {
            var accLogic = new AccidentalEccentricityLogic()
            {
                Length = CompressedMember.GeometryLength,
                SizeX = ndms.Max(x => x.CenterX) - ndms.Min(x => x.CenterX),
                SizeY = ndms.Max(x => x.CenterY) - ndms.Min(x => x.CenterY),
                InitialForceTuple = newTuple,
            };
            if (TraceLogger is not null) { accLogic.TraceLogger = TraceLogger.GetSimilarTraceLogger(50); }
            newTuple = accLogic.GetForceTuple();
            return newTuple;
        }

        private IConcreteBucklingResult ProcessBuckling(BucklingInputData inputData)
        {
            IForceTuple resultTuple;
            IForceTuple longTuple;
            if (inputData.CalcTerm == CalcTerms.LongTerm)
            {
                longTuple = inputData.ForceTuple;
            }
            else
            {
                longTuple = GetLongTuple(inputData.Combination.DesignForces, inputData.LimitState);
            }
            longTuple = ProcessAccEccentricity(inputData.Ndms, longTuple);
            var bucklingCalculator = GetBucklingCalculator(CompressedMember, inputData.LimitState, inputData.CalcTerm, inputData.ForceTuple, longTuple);
            if (TraceLogger is not null)
            {
                bucklingCalculator.TraceLogger = TraceLogger.GetSimilarTraceLogger(50);
            }
            bucklingCalculator.Run();
            var bucklingResult = bucklingCalculator.Result as IConcreteBucklingResult;
          
            return bucklingResult;
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
            var options = new ConcreteBucklingOptions()
            { 
                CompressedMember = compressedMember,
                LimitState = limitStates,
                CalcTerm = calcTerms,
                CalcForceTuple = calcTuple,
                LongTermTuple = longTuple,
                Primitives = Primitives
            };
            var bucklingCalculator = new ConcreteBucklingCalculator(options, Accuracy);
            return bucklingCalculator;
        }

        private ForceTuple CalculateBuckling(IForceTuple calcTuple, IConcreteBucklingResult bucklingResult)
        {
            var newTuple = calcTuple.Clone() as ForceTuple;
            newTuple.Mx *= bucklingResult.EtaFactorAlongY;
            newTuple.My *= bucklingResult.EtaFactorAlongX;
            return newTuple;
        }


        private string CheckInputData()
        {
            string result = "";
            try
            {
                NdmPrimitivesService.CheckPrimitives(Primitives);
            }
            catch (Exception ex)
            {
                result += ex;
            }
            if (ForceActions.Count == 0)
            {
                result += "Calculator does not contain any forces \n";
            }
            if (LimitStatesList.Count == 0)
            {
                result += "Calculator does not contain any limit states \n";
            }
            if (CalcTermsList.Count == 0)
            {
                result += "Calculator does not contain any duration \n";
            }
            return result;
        }

        public ForceCalculator(IForceTupleCalculator forceTupleCalculator)
        {
            this.forceTupleCalculator = forceTupleCalculator;
            SetDefaultProperties();
        }

        public ForceCalculator() : this(new ForceTupleCalculator())
        {           
        }

        private void SetDefaultProperties()
        {
            ForceActions = new List<IForceAction>();
            Primitives = new List<INdmPrimitive>();
            CompressedMember = new CompressedMember()
            {
                Buckling = false
            };
            Accuracy = new Accuracy()
            {
                IterationAccuracy = 0.001d,
                MaxIterationCount = 1000
            };
            LimitStatesList = new List<LimitStates>()
            {
                LimitStates.ULS,
                LimitStates.SLS
            };
            CalcTermsList = new List<CalcTerms>()
            {
                CalcTerms.ShortTerm,
                CalcTerms.LongTerm
            };
        }
        private void GetCombinations()
        {
            ForceCombinationLists = new List<IForceCombinationList>();
            foreach (var item in ForceActions)
            {
                ForceCombinationLists.Add(item.GetCombinations());
            }
        }

        private IForcesTupleResult GetPrimitiveStrainMatrix(IEnumerable<INdm> ndmCollection, IForceTuple tuple, IAccuracy accuracy)
        {
            var inputData = new ForceTupleInputData()
            {
                NdmCollection = ndmCollection,
                Tuple = tuple,
                Accuracy = accuracy
            };
            var calculator = forceTupleCalculator.Clone() as IForceTupleCalculator;
            calculator.InputData = inputData;
            if (TraceLogger is not null)
            {
                calculator.TraceLogger = TraceLogger.GetSimilarTraceLogger();
            }
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
