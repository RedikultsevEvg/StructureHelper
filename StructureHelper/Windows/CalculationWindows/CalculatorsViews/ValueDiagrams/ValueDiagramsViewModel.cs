using StructureHelper.Windows.ViewModels;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ValueDiagrams
{
    public class ValueDiagramsViewModel : SelectItemVM<IValueDiagramEntity>
    {
        private IUpdateStrategy<IValueDiagramEntity> updateStrategy;

        public override void AddMethod(object parameter)
        {
            NewItem = new ValueDiagramEntity(Guid.NewGuid())
            {
                IsTaken = true,
                Name = "New Value Diagram"
            };
            NewItem.ValueDiagram.StepNumber = 50;
            NewItem.ValueDiagram.Point2DRange.StartPoint.Y = 0.25;
            NewItem.ValueDiagram.Point2DRange.EndPoint.Y = - 0.25;
            base.AddMethod(parameter);
        }
        public override void EditMethod(object parameter)
        {
            if (SelectedItem is null) { return; }
            IValueDiagramEntity cloneDiagram = (IValueDiagramEntity)SelectedItem.Clone();
            var vm = new ValueDiagramEntityViewModel(SelectedItem);
            var wnd = new ValueDiagramEntityView(vm);
            wnd.ShowDialog();
            if (wnd.DialogResult != true)
            {
                updateStrategy ??= new ValueDiagramEntityUpdateStrategy();
                updateStrategy.Update(SelectedItem, cloneDiagram);
            }
            base.EditMethod(parameter);
        }
        public override void DeleteMethod(object parameter)
        {
            var dialogResult = MessageBox.Show("Delete diagram?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                base.DeleteMethod(parameter);
            }
        }
        public ValueDiagramsViewModel(List<IValueDiagramEntity> collection) : base(collection)
        {
        }
    }
}
