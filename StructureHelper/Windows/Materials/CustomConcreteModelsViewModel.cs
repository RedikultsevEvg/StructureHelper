using StructureHelper.Windows.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace StructureHelper.Windows.ViewModels.Materials
{
    /// <summary>
    /// Collection view model for the custom concrete models. It provides the standard
    /// SelectItemVM CRUD commands and opens the model editor for editing.
    /// </summary>
    public class CustomConcreteModelsViewModel : SelectItemVM<ICustomConcreteModel>
    {
        public CustomConcreteModelsViewModel(List<ICustomConcreteModel> collection)
            : base(collection)
        {
        }

        public override void AddMethod(object parameter)
        {
            ICustomConcreteModel model = CreateDefaultModel();
            NewItem = model;
            base.AddMethod(parameter);
            SelectedItem = model;

            if (ShowEditor(model) != true)
            {
                Collection.Remove(model);
                Items.Remove(model);
                SelectedItem = Items.LastOrDefault();
            }
            else
            {
                base.EditMethod(parameter);
            }
        }

        public override void EditMethod(object parameter)
        {
            if (SelectedItem is null)
            {
                return;
            }

            ShowEditor(SelectedItem);
            base.EditMethod(parameter);
        }

        public override void CopyMethod(object parameter)
        {
            if (SelectedItem is not ICloneable cloneable
                || cloneable.Clone() is not ICustomConcreteModel copy)
            {
                return;
            }

            copy.Name = $"{copy.Name} copy";
            copy.IsActive = false;
            NewItem = copy;
            base.AddMethod(parameter);
            SelectedItem = copy;
            RefreshItemViews();
        }

        public override void DeleteMethod(object parameter)
        {
            if (SelectedItem is null)
            {
                return;
            }

            MessageBoxResult result = MessageBox.Show(
                "Delete the selected model?",
                "Delete model",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            int index = Items.IndexOf(SelectedItem);
            base.DeleteMethod(parameter);
            SelectedItem = Items.Count == 0
                ? null
                : Items[Math.Min(index, Items.Count - 1)];
            RefreshItemViews();
        }

        /// <summary>
        /// Reloads the visible items from the backing collection. This is used after
        /// the material editor restores its original models.
        /// </summary>
        public void Reload(Guid? selectedId = null)
        {
            Refresh();
            if (selectedId is not null)
            {
                SelectedItem = Items.FirstOrDefault(x => x.Id == selectedId) ?? Items.LastOrDefault();
            }
        }

        private bool? ShowEditor(ICustomConcreteModel model)
        {
            var viewModel = new CustomConcreteModelEditViewModel(model, Collection);
            var window = new CustomConcreteModelView(viewModel);
            return window.ShowDialog();
        }

        private void RefreshItemViews()
        {
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(SelectedItem));
        }

        internal static ICustomConcreteModel CreateDefaultModel()
        {
            var model = new CustomConcreteModel(Guid.NewGuid())
            {
                Name = "New model",
                IsActive = false
            };
            model.StateCalcTermPair.LimitState = LimitStates.ULS;
            model.StateCalcTermPair.CalcTerm = CalcTerms.ShortTerm;
            return model;
        }
    }
}
