using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.Reports.CalculationReports;
using StructureHelper.Windows.PrimitivePropertiesWindow;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    internal class ShowIsoFieldLogic
    {
        private List<INdmPrimitive> selectedNdmPrimitives;
        private List<INdmPrimitive> selectedContourPrimitives;
        private List<INdm> ndms;

        public List<INdmPrimitive> Primitives { get; set; }
        public IStateCalcTermPair StateCalcTermPair { get; set; }
        public IStrainMatrix StrainMatrix { get; set; }

        public void ShowStressStrainField2D()
        {
            if (SelectPrimitives() == true)
            {
                GetNdms();
                ShowIsoField();
            }
        }

        public void ShowPrestrainField2D()
        {
            if (SelectPrimitives() == true)
            {
                GetNdms();
                ShowPrestrainField();
            }
        }

        private void ShowPrestrainField()
        {
            SelectedPrimitiveSet selectedPrimitiveSet = GetPrimitiveSet();
            Contour2DNdmPrestrainReport report = new(selectedPrimitiveSet);
            report.Show();
        }

        private SelectedPrimitiveSet GetPrimitiveSet()
        {
            return new()
            {
                AllPrimitives = Primitives,
                SelectedPrimitives = selectedNdmPrimitives.ToList(),
                ContourPrimitives = selectedContourPrimitives.ToList(),
                StateCalcTermPair = StateCalcTermPair,
                Ndms = [.. ndms],
                StrainMatrix = StrainMatrix
            };
        }

        private void ShowIsoField()
        {
            SelectedPrimitiveSet selectedPrimitiveSet = GetPrimitiveSet();
            Contour2DNdmStressStrainReport report = new(selectedPrimitiveSet);
            report.Show();
        }

        private void GetNdms()
        {
            var logic = new GetNdmPartLogic()
            {
                LimitState = StateCalcTermPair.LimitState,
                CalcTerm = StateCalcTermPair.CalcTerm,
                Primitives = Primitives.ToList(),
                SelectedPrimitives = selectedNdmPrimitives.ToList(),
            };
            ndms = logic.GetNdms();
        }

        private bool SelectPrimitives()
        {
            var vm = new SelectPrimitivesForShowAndContoursViewModel(Primitives);
            vm.ShowItems.SelectAllCommand.Execute(true);
            vm.ContoursItems.UnSelectAllCommand.Execute(true);
            var wnd = new SelectPrimitivesForShowAndContoursView(vm);
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                selectedNdmPrimitives = vm.ShowItems
                    .CollectionItems
                    .Where(x => x.IsSelected == true)
                    .Select(x => x.Item.GetNdmPrimitive())
                    .ToList();
                selectedContourPrimitives = vm.ContoursItems
                    .CollectionItems
                    .Where(x => x.IsSelected == true)
                    .Select(x => x.Item.GetNdmPrimitive())
                    .ToList();
                return true;
            }
            return false;
        }
    }
}
