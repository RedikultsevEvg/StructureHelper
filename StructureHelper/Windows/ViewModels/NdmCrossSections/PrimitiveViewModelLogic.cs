using FieldVisualizer.ViewModels;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureHelper.Services.Primitives;
using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.Models.Primitives;
using StructureHelperCommon.Infrastructures.Strings;
using ViewModelBase = StructureHelper.Infrastructure.ViewModelBase;
using System.Windows.Forms;
using System.Windows.Documents;
using StructureHelper.Windows.PrimitiveProperiesWindow;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;

namespace StructureHelper.Windows.ViewModels.NdmCrossSections
{
    public class PrimitiveViewModelLogic : ViewModelBase, IPrimitiveViewModelLogic
    {
        private readonly ICrossSectionRepository repository;
        private RelayCommand addCommand;
        private RelayCommand deleteCommand;
        private RelayCommand editCommand;
        private RelayCommand copyCommand;
        private RelayCommand setToFront;
        private RelayCommand setToBack;

        public double CanvasWidth { get; set; }
        public double CanvasHeight { get; set; }

        public PrimitiveBase SelectedItem { get; set; }

        public ObservableCollection<PrimitiveBase> Items { get; private set; }

        public RelayCommand Add
        {
            get
            {
                return addCommand ??
                    (
                    addCommand = new RelayCommand(o =>
                    {
                        if (!(o is PrimitiveType primitiveType)) return;
                        AddPrimitive(primitiveType);
                    }
                    ));
            }
        }

        private void AddPrimitive(PrimitiveType primitiveType)
        {
            PrimitiveBase viewPrimitive;
            INdmPrimitive ndmPrimitive;
            if (primitiveType == PrimitiveType.Rectangle)
            {
                var primitive = new RectanglePrimitive
                {
                    Width = 0.4d,
                    Height = 0.6d
                };
                ndmPrimitive = primitive;
                viewPrimitive = new RectangleViewPrimitive(primitive);

            }
            else if (primitiveType == PrimitiveType.Point)
            {
                var primitive = new PointPrimitive
                {
                    Area = 0.0005d
                };
                ndmPrimitive = primitive;
                viewPrimitive = new PointViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.Circle)
            {
                var primitive = new CirclePrimitive
                {
                    Diameter = 0.5d
                };
                ndmPrimitive = primitive;
                viewPrimitive = new CircleViewPrimitive(primitive);
            }
            else { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + nameof(primitiveType)); }
            viewPrimitive.RegisterDeltas(CanvasWidth / 2, CanvasHeight / 2);
            repository.Primitives.Add(ndmPrimitive);
            Items.Add(viewPrimitive);
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(PrimitivesCount));
        }

        public RelayCommand Delete
        {
            get
            {
                return deleteCommand ??
                    (
                    deleteCommand = new RelayCommand(o=>
                        DeleteSelectedPrimitive(),
                        o => SelectedItem != null
                    ));
            }
        }

        private void DeleteSelectedPrimitive()
        {
            var dialogResult = MessageBox.Show("Delete primitive?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                var ndmPrimitive = SelectedItem.GetNdmPrimitive();
                repository.Primitives.Remove(ndmPrimitive);
                foreach (var calc in repository.CalculatorsList)
                {
                    if (calc is IForceCalculator)
                    {
                        var forceCalc = calc as IForceCalculator;
                        forceCalc.Primitives.Remove(ndmPrimitive);
                    }
                }
                Items.Remove(SelectedItem);
            }
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(PrimitivesCount));
        }

        public RelayCommand Edit
        {
            get
            {
                return editCommand ??
                    (editCommand = new RelayCommand(
                        o => EditSelectedItem(),
                        o => SelectedItem != null
                    ));
            }
        }

        private void EditSelectedItem()
        {
            var wnd = new PrimitivePropertiesView(SelectedItem, repository);
            wnd.ShowDialog();
        }

        public RelayCommand Copy
        {
            get
            {
                return copyCommand ??
                    (
                    copyCommand = new RelayCommand(
                        o => CopySelectedItem(),
                        o => SelectedItem != null
                    ));
            }
        }

        private void CopySelectedItem()
        {
            var oldPrimitive = SelectedItem.GetNdmPrimitive();
            var newPrimitive = oldPrimitive.Clone() as INdmPrimitive;
            repository.Primitives.Add(newPrimitive);
            PrimitiveBase primitiveBase;
            if (newPrimitive is IRectanglePrimitive) { primitiveBase = new RectangleViewPrimitive(newPrimitive as IRectanglePrimitive); }
            else if (newPrimitive is ICirclePrimitive) { primitiveBase = new CircleViewPrimitive(newPrimitive as ICirclePrimitive); }
            else if (newPrimitive is IPointPrimitive) { primitiveBase = new PointViewPrimitive(newPrimitive as IPointPrimitive); }
            else throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
            primitiveBase.RegisterDeltas(CanvasWidth / 2, CanvasHeight / 2);
            Items.Add(primitiveBase);
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(PrimitivesCount));
        }

        public int PrimitivesCount => repository.Primitives.Count();

        public RelayCommand SetToFront
        {
            get
            {
                return setToFront ??
                    (setToFront = new RelayCommand(o=>
                    {
                        int maxZIndex = Items.Select(x => x.GetNdmPrimitive().VisualProperty.ZIndex).Max();
                        SelectedItem.ZIndex = maxZIndex + 1;
                    },o => CheckMaxIndex()
                    ));
            }
        }
        private bool CheckMaxIndex()
        {
            if (SelectedItem is null || Items.Count == 0) return false;
            int maxZIndex = Items.Select(x => x.GetNdmPrimitive().VisualProperty.ZIndex).Max();
            if (SelectedItem.ZIndex <= maxZIndex) return true;
            else return false;
        }

        private bool CheckMinIndex()
        {
            if (SelectedItem is null || Items.Count == 0) return false;
            int minZIndex = Items.Select(x => x.GetNdmPrimitive().VisualProperty.ZIndex).Min();
            if (SelectedItem.ZIndex >= minZIndex) return true;
            else return false;
        }

        public RelayCommand SetToBack
        {
            get
            {
                return setToBack ??
                    (setToBack = new RelayCommand(o =>
                    {
                        int minZIndex = Items.Select(x => x.GetNdmPrimitive().VisualProperty.ZIndex).Min();
                        SelectedItem.ZIndex = minZIndex - 1;
                    }, o => CheckMinIndex()
                    ));
            }
        }

        public void AddItems(IEnumerable<PrimitiveBase> items)
        {
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public void Refresh()
        {
            OnPropertyChanged(nameof(PrimitivesCount));
        }

        public PrimitiveViewModelLogic(ICrossSectionRepository repository)
        {
            this.repository = repository;
            Items = new ObservableCollection<PrimitiveBase>();
            AddItems(PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(this.repository.Primitives));
        }
    }
}
