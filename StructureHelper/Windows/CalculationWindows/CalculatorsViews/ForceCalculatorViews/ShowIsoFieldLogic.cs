using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.Reports.CalculationReports;
using StructureHelper.Windows.PrimitivePropertiesWindow;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews
{
    internal class ShowIsoFieldLogic
    {
        private List<INdmPrimitive> selectedNdmPrimitives;
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
            SelectedPrimitiveSet selectedPrimitiveSet = new()
            {
                AllPrimitives = Primitives,
                SelectedPrimitives = selectedNdmPrimitives.ToList(),
                StateCalcTermPair = StateCalcTermPair,
                Ndms = [.. ndms],
                StrainMatrix = StrainMatrix
            };
            Contour2DNdmPrestrainReport report = new(selectedPrimitiveSet);
            report.Show();
        }

        private void ShowIsoField()
        {
            SelectedPrimitiveSet selectedPrimitiveSet = new()
            {
                AllPrimitives = Primitives,
                SelectedPrimitives = selectedNdmPrimitives.ToList(),
                StateCalcTermPair = StateCalcTermPair,
                Ndms = [.. ndms],
                StrainMatrix = StrainMatrix
            };
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
            var vm = new SelectPrimitivesViewModel(Primitives);
            var wnd = new SelectPrimitivesView(vm);
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                selectedNdmPrimitives = vm.Items
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
