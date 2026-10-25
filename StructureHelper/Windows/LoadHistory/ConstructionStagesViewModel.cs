using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.LoadHistory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StructureHelper.Windows.LoadHistory
{
    public class ConstructionStagesViewModel : SelectItemVM<IConstructionStage>
    {
        private readonly ICrossSectionRepository repository;

        public override void AddMethod(object parameter)
        {
            NewItem = new ConstructionStage(Guid.NewGuid())
            {
                Name = $"Construction stage {Items.Count + 1}",
            };
            NewItem.InputData.SubStepNumber = 10;
            NewItem.InputData.Duration = 1d;
            if (repository.ForceActions.Count > 0)
            {
                NewItem.InputData.ForceAction = repository.ForceActions[0];
            }
            NewItem.InputData.Primitives.AddRange(repository.Primitives);
            base.AddMethod(parameter);
        }

        public override void EditMethod(object parameter)
        {
            if (SelectedItem is null) { return; }
            SafetyProcessor.RunSafeProcess(EditSelectedItem, "Error of editing of costruction stage");
            base.EditMethod(parameter);
        }

        private void EditSelectedItem()
        {
            var clone = SelectedItem.Clone() as IConstructionStage;
            ConstructionStageViewModel vm = new(SelectedItem, repository.Primitives, repository.ForceActions);
            ConstructionStageView wnd = new(vm);
            wnd.ShowDialog();
            if (wnd.DialogResult != true)
            {
                ConstructionStageUpdateStrategy updateStrategy = new() { UpdateChildren = true };
                updateStrategy.Update(SelectedItem, clone);
            }
        }

        public ConstructionStagesViewModel(List<IConstructionStage> collection, ICrossSectionRepository repository) : base(collection)
        {
            this.repository = repository;
        }
    }
}
