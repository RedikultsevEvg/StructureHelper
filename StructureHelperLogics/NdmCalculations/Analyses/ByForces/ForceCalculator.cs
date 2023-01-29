using LoaderCalculator;
using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Data.SourceData;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Models.Shapes;
using StructureHelperCommon.Services.Calculations;
using StructureHelperCommon.Services.Forces;
using StructureHelperCommon.Services.Sections;
using StructureHelperLogics.NdmCalculations.Buckling;
using StructureHelperLogics.NdmCalculations.Primitives;
using StructureHelperLogics.Services.NdmPrimitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculator : IForceCalculator
    {
        public string Name { get; set; }
        public List<LimitStates> LimitStatesList { get; }
        public List<CalcTerms> CalcTermsList { get; }
        public List<IForceCombinationList> ForceCombinationLists { get; }
        public List<INdmPrimitive> Primitives { get; }
        public INdmResult Result { get; private set; }
        public ICompressedMember CompressedMember { get; }
        public IAccuracy Accuracy { get; set; }

        public void Run()
        {
            var checkResult = CheckInputData();
            if (checkResult != "")
            {
                Result = new ForcesResults() { IsValid = false, Desctription = checkResult };
                return;
            }
            else { CalculateResult(); }
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
                            point2D = new Point2D() { X = loaderPoint.CenterX, Y = loaderPoint.CenterY };
                        }
                        else point2D = combination.ForcePoint;
                        var newTuple = ForceTupleService.MoveTupleIntoPoint(tuple.ForceTuple, point2D);
                        var result = GetPrimitiveStrainMatrix(ndms, newTuple);
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
                                    result.Desctription += $"Buckling result:\n{bucklingResult.Desctription}\n";
                                }
                                newTuple = CalculateBuckling(newTuple, bucklingResult);
                            }
                            catch (Exception ex)
                            {
                                result.IsValid = false;
                                result.Desctription = $"Buckling error:\n{ex}\n";
                            }

                        }
                        
                        result.DesignForceTuple.LimitState = limitState;
                        result.DesignForceTuple.CalcTerm = calcTerm;
                        result.DesignForceTuple.ForceTuple = newTuple;
                        ndmResult.ForcesResultList.Add(result);
                    }
                }
            }
            Result = ndmResult;
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

        private IForceTuple CalculateBuckling(IForceTuple calcTuple, IConcreteBucklingResult bucklingResult)
        {
            var newTuple = calcTuple.Clone() as IForceTuple;
            newTuple.Mx *= bucklingResult.EtaFactorAlongY;
            newTuple.My *= bucklingResult.EtaFactorAlongX;
            return newTuple;
        }


        private string CheckInputData()
        {
            string result = "";
            if (Primitives.Count == 0) { result += "Calculator does not contain any primitives \n"; }
            if (ForceCombinationLists.Count == 0) { result += "Calculator does not contain any forces \n"; }
            if (LimitStatesList.Count == 0) { result += "Calculator does not contain any limit states \n"; }
            if (CalcTermsList.Count == 0) { result += "Calculator does not contain any duration \n"; }
            return result;
        }

        public ForceCalculator()
        {
            ForceCombinationLists = new List<IForceCombinationList>();
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
            IForceCalculator target = new ForceCalculator { Name = Name + " copy"};
            target.LimitStatesList.Clear();
            target.LimitStatesList.AddRange(LimitStatesList);
            target.CalcTermsList.Clear();
            target.CalcTermsList.AddRange(CalcTermsList);
            AccuracyService.CopyProperties(Accuracy, target.Accuracy);
            CompressedMemberServices.CopyProperties(CompressedMember, target.CompressedMember);
            target.Primitives.AddRange(Primitives);
            target.ForceCombinationLists.AddRange(ForceCombinationLists);
            return target;
        }
    }
}
