using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.ResultViewers;
using StructureHelper.Windows.Forces;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Parameters;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.ForceResultLogic
{
    public class ValuePointDiagramLogic : IValuePointDiagramLogic
    {
        private ArrayParameter<double> arrayParameter;
        private List<(INamedAreaPoint areaPoint, INdmPrimitive ndmPrimitive)> pointCollection;
        private List<IForcesTupleResult> validTuplesList;
        private ArrayParameter<double> arrayOfValuesByPoint;
        private IEnumerable<ForceResultFunc> selectedDelegates;
        private string exceptionMessage;

        public IEnumerable<IForcesTupleResult> TupleList { get; set; }
        public ForceCalculator Calculator { get; set; }
        public PointPrimitiveLogic PrimitiveLogic { get; set; }
        public ValueDelegatesLogic ValueDelegatesLogic { get; set; }

        public GenericResult<ArrayParameter<double>> GetArrayParameter()
        {
            SetParameters();
            var checkResult = CheckParameters();
            if (checkResult != true)
            {
                return GetFalseResult();
            }
            PrepareArray();
            return GetValidResult();
        }

        private GenericResult<ArrayParameter<double>> GetValidResult()
        {
            int i = 0;
            foreach (var tuple in validTuplesList)
            {
                ProcessPointByTuple(tuple, i);
                i++;
            }
            arrayParameter.AddArray(arrayOfValuesByPoint);
            return new GenericResult<ArrayParameter<double>>()
            {
                IsValid = true,
                Value = arrayParameter
            };
        }
        private GenericResult<ArrayParameter<double>> GetFalseResult()
        {
            return new GenericResult<ArrayParameter<double>>()
            {
                IsValid = false,
                Description = exceptionMessage
            };
        }
        private void SetParameters()
        {
            GetPointCollection();
            selectedDelegates = ValueDelegatesLogic.ResultFuncs.SelectedItems;
            validTuplesList = TupleList
                .Where(x => x.IsValid == true)
                .ToList();
        }
        private bool CheckParameters()
        {
            var result = true;
            exceptionMessage = ErrorStrings.DataIsInCorrect;
            if (pointCollection.Any() == false)
            {
                exceptionMessage += ", point collection is null";
                result = false;
            }
            if (selectedDelegates.Any() == false)
            {
                exceptionMessage += ", value expression collection is null";
                result = false;
            }
            if (validTuplesList.Any() == false)
            {
                exceptionMessage += ", force list is empty";
                result = false;
            }
            return result;
        }
        private void GetPointCollection()
        {
            pointCollection = new();
            foreach (var primitiveValuePoint in PrimitiveLogic.Collection.CollectionItems)
            {
                foreach (var selectedPoint in primitiveValuePoint.Item.ValuePoints.SelectedItems)
                {
                    var newPoint = (selectedPoint, primitiveValuePoint.Item.PrimitiveBase.GetNdmPrimitive());
                    pointCollection.Add(newPoint);
                }
            }
        }
        private void ProcessPointByTuple(IForcesTupleResult tuple, int i)
        {
            var values = new List<double>();
            var strainMatrix = tuple.LoaderResults.ForceStrainPair.StrainMatrix;

            foreach (var valuePoint in pointCollection)
            {
                var ndm = GetMockNdm(valuePoint, tuple);

                foreach (var valDelegate in selectedDelegates)
                {
                    double val = valDelegate.ResultFunction.Invoke(strainMatrix, ndm) * valDelegate.UnitFactor;
                    values.Add(val);
                }
            }
            arrayOfValuesByPoint.AddRow(i, values);
        }
        private void PrepareArray()
        {
            var factory = new DiagramFactory()
            {
                TupleList = validTuplesList,
                //SetProgress = SetProgress,
            };
            arrayParameter = factory.GetCommonArray();

            var labels = GetValueLabels(selectedDelegates);
            arrayOfValuesByPoint = new ArrayParameter<double>(validTuplesList.Count(), labels);
        }
        private INdm GetMockNdm((INamedAreaPoint areaPoint, INdmPrimitive ndmPrimitive) valuePoint, IForcesTupleResult tuple)
        {
            var limitState = tuple.DesignForceTuple.LimitState;
            var calcTerm = tuple.DesignForceTuple.CalcTerm;
            var material = valuePoint.ndmPrimitive.NdmElement.HeadMaterial.GetLoaderMaterial(limitState, calcTerm);
            var userPrestrain = valuePoint.ndmPrimitive.NdmElement.UsersPrestrain;
            var autoPrestrain = valuePoint.ndmPrimitive.NdmElement.AutoPrestrain;
            var ndm = new Ndm()
            {
                Area = valuePoint.areaPoint.Area,
                CenterX = valuePoint.areaPoint.Point.X,
                CenterY = valuePoint.areaPoint.Point.Y,
                Material = material,
            };
            var prestrain = (userPrestrain.Mx + autoPrestrain.Mx) * valuePoint.areaPoint.Point.Y
                + (userPrestrain.My + autoPrestrain.My) * valuePoint.areaPoint.Point.X
                + userPrestrain.Nz + autoPrestrain.Nz;
            ndm.PrestrainLogic.Add(PrestrainTypes.Prestrain, prestrain);
            return ndm;
        }
        private List<string> GetValueLabels(IEnumerable<ForceResultFunc> selectedDelegates)
        {
            List<string> strings = new();
            foreach (var valuePoint in pointCollection)
            {
                foreach (var deleg in selectedDelegates)
                {
                    string s = valuePoint.ndmPrimitive.Name;
                    s += "_" + valuePoint.areaPoint.Name;
                    s += "_" + deleg.Name + ", " + deleg.UnitName;
                    strings.Add(s);
                }
            }
            return strings;
        }
    }
}
