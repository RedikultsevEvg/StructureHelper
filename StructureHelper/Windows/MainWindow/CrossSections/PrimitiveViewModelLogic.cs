using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Windows.PrimitivePropertiesWindow;
using StructureHelper.Windows.Services;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Analyses.Curvatures;
using StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

//Copyright (c) 2024 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.ViewModels.NdmCrossSections
{
    public class PrimitiveViewModelLogic : ViewModelBase, ICRUDViewModel<PrimitiveBase>, IRectangleShape, IObservable<PrimitiveBase>
    {
        private ICrossSection section;
        private ICrossSectionRepository repository => section.SectionRepository;
        private ICommand addCommand;
        private ICommand deleteCommand;
        private ICommand editCommand;
        private ICommand copyCommand;
        private ICommand setToFront;
        private ICommand setToBack;
        private ICommand copyToCommand;
        private RelayCommand setAsHostCommand;
        private RelayCommand setMaterialToPrimitivesCommand;
        private RelayCommand setHostToPrimitivesCommand;
        private RelayCommand deletAllCommand;

        public double Width { get; set; }
        public double Height { get; set; }

        public PrimitiveBase SelectedItem { get; set; }

        public ObservableCollection<PrimitiveBase> Items { get; private set; }

        public ICommand SetAsHostCommand => setAsHostCommand ??= new RelayCommand(SetPrimititveAsHost,
            o => SelectedItem != null
            && SelectedItem.NdmPrimitive is IHasDivisionSize);

        public ICommand SetHostToPrimitivesCommand => setHostToPrimitivesCommand ??= new RelayCommand(SetHostToPrimitives,
    o => SelectedItem != null
    && SelectedItem.NdmPrimitive is IHasHostPrimitive);

        private void SetHostToPrimitives(object obj)
        {
            if (SelectedItem is null) { return; }
            var newHost = (SelectedItem.NdmPrimitive as IHasHostPrimitive).HostPrimitive;
            SetNewHost(newHost);
        }

        public ICommand SetMaterialToPrimitivesCommand => setMaterialToPrimitivesCommand ??= new RelayCommand(SetMaterialToPrimitives,
    o => SelectedItem != null
    && SelectedItem
    .NdmPrimitive
    .NdmElement
    .HeadMaterial != null);

        public ICommand DeleteAllCommand => deletAllCommand ??= new RelayCommand(DeleteAll, o => Items.Count > 0);

        private void DeleteAll(object obj)
        {
            var dialogResult = MessageBox.Show("Delete all primitives?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                SafetyProcessor.RunSafeProcess(ClearRepository, "Error of deleting primitives");
                Refresh();
                OnPropertyChanged(nameof(Items));
                OnPropertyChanged(nameof(PrimitivesCount));
            }
        }

        private void ClearRepository()
        {
            repository.Operations.Primitives.RemoveAll();
            Items.Clear();
        }

        public ICommand DeleteSelectedCommand => deleteSelectedCommand ??= new RelayCommand(DeleteSelected, o => Items.Count > 0);

        private void DeleteSelected(object commandParameter)
        {
            var vm = new SelectPrimitivesViewModel(repository.Primitives);
            var wnd = new SelectPrimitivesView(vm);
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                var selectedNdmPrimitives = vm.Items.CollectionItems.Where(x => x.IsSelected == true).Select(x => x.Item.GetNdmPrimitive());
                var deletePrimitivesList = Items
                    .Where(x => selectedNdmPrimitives.Contains(x.NdmPrimitive))
                    .ToList();
                foreach (var item in deletePrimitivesList)
                {
                    RemoveFromRepository(item);
                }
                Refresh();
                OnPropertyChanged(nameof(Items));
                OnPropertyChanged(nameof(PrimitivesCount));
            }
        }

        private void RemoveFromRepository(PrimitiveBase item)
        {
            repository.Primitives.Remove(item.NdmPrimitive);
            Items.Remove(item);
        }

        private void SetMaterialToPrimitives(object obj)
        {
            if (SelectedItem is null) { return; }
            var material = SelectedItem.NdmPrimitive.NdmElement.HeadMaterial;
            if (material == null) { return; };
            var vm = new SelectPrimitivesViewModel(repository.Primitives);
            var wnd = new SelectPrimitivesView(vm);
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                var selectedNdmPrimitives = vm.Items.CollectionItems.Where(x => x.IsSelected == true).Select(x => x.Item.GetNdmPrimitive());
                foreach (var item in selectedNdmPrimitives)
                {
                    item.NdmElement.HeadMaterial = material;
                }
                Refresh();
            }
        }

        private void SetPrimititveAsHost(object obj)
        {
            if (SelectedItem is null) { return; }
            var newHost = SelectedItem.NdmPrimitive;
            SetNewHost(newHost);
        }

        private void SetNewHost(INdmPrimitive newHost)
        {
            var vm = new SelectPrimitivesViewModel(repository.Primitives.Where(x => x is IHasHostPrimitive));
            var wnd = new SelectPrimitivesView(vm);
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                var selectedNdmPrimitives = vm.Items.CollectionItems.Where(x => x.IsSelected == true).Select(x => x.Item.GetNdmPrimitive());
                var hasHostPrimitives = selectedNdmPrimitives.Where(x => x is IHasHostPrimitive);
                foreach (var item in hasHostPrimitives)
                {
                    IHasHostPrimitive hostPrimitive = item as IHasHostPrimitive;
                    hostPrimitive.HostPrimitive = newHost;
                }
                Refresh();
            }
        }

        public ICommand Add
        {
            get
            {
                return addCommand ??= new RelayCommand(o =>
                    {
                        if (o is not PrimitiveType primitiveType) return;
                        AddPrimitive(primitiveType);
                    }
                    );
            }
        }

        private void AddPrimitive(PrimitiveType primitiveType)
        {
            PrimitiveBase viewPrimitive;
            INdmPrimitive ndmPrimitive;
            if (primitiveType == PrimitiveType.Rectangle)
            {
                RectangleNdmPrimitive primitive = GetNewRectanglePrimitive();
                ndmPrimitive = primitive;
                viewPrimitive = new RectangleViewPrimitive(primitive);

            }
            else if (primitiveType == PrimitiveType.Reinforcement)
            {
                RebarNdmPrimitive primitive = GetNewReinforcementPrimitive();
                ndmPrimitive = primitive;
                viewPrimitive = new ReinforcementViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.Point)
            {
                PointNdmPrimitive primitive = GetNewPointPrimitive();
                ndmPrimitive = primitive;
                viewPrimitive = new PointViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.Circle)
            {
                EllipseNdmPrimitive primitive = GetNewCirclePrimitive();
                ndmPrimitive = primitive;
                viewPrimitive = new CircleViewPrimitive(primitive);
            }
            else if (primitiveType == PrimitiveType.Polygon)
            {
                ShapeNdmPrimitive primitive = GetNewPolygonPrimitive();
                ndmPrimitive = primitive;
                viewPrimitive = new ShapeViewPrimitive(primitive);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + nameof(primitiveType));
            }
            viewPrimitive.OnNext(this);
            repository.Primitives.Add(ndmPrimitive);
            ndmPrimitive.CrossSection = section;
            Items.Add(viewPrimitive);
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(PrimitivesCount));
        }

        private ShapeNdmPrimitive GetNewPolygonPrimitive()
        {
            LinePolygonShape polygon = new(Guid.NewGuid());
            polygon.AddVertex(new Vertex(-0.2, 0.3));
            polygon.AddVertex(new Vertex(0.2, 0.3));
            polygon.AddVertex(new Vertex(0.1, 0));
            polygon.AddVertex(new Vertex(0.2, -0.3));
            polygon.AddVertex(new Vertex(-0.2, -0.3));
            polygon.AddVertex(new Vertex(-0.1, 0));
            ShapeNdmPrimitive shapeNdmPrimitive = new(Guid.NewGuid())
            {
                Name = "New polygon primitive"
            };
            shapeNdmPrimitive.SetShape(polygon);
            return shapeNdmPrimitive;
        }

        private static EllipseNdmPrimitive GetNewCirclePrimitive()
        {
            return new EllipseNdmPrimitive
            {
                Width = 0.5d
            };
        }

        private static PointNdmPrimitive GetNewPointPrimitive()
        {
            return new PointNdmPrimitive
            {
                Area = 0.0005d
            };
        }

        private static RebarNdmPrimitive GetNewReinforcementPrimitive()
        {
            return new RebarNdmPrimitive
            {
                Area = 0.0005d
            };
        }

        private static RectangleNdmPrimitive GetNewRectanglePrimitive()
        {
            return new RectangleNdmPrimitive
            {
                Width = 0.4d,
                Height = 0.6d
            };
        }

        public ICommand Delete
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
                foreach (var calc in repository.Calculators)
                {
                    if (calc is IForceCalculator forceCalculator)
                    {
                        var forceCalc = forceCalculator.InputData as IHasPrimitives;
                        forceCalc.Primitives.Remove(ndmPrimitive);
                    }
                    else if (calc is ILimitCurvesCalculator calculator)
                    {
                        //to do
                        //var forceCalc = calculator.InputData as IHasPrimitives;
                        //forceCalc.Primitives.Remove(ndmPrimitive);
                    }
                    else if (calc is ICrackCalculator crackCalculator)
                    {
                        var forceCalc = crackCalculator.InputData as IHasPrimitives;
                        forceCalc.Primitives.Remove(ndmPrimitive);
                    }
                    else if (calc is IValueDiagramCalculator diagramCalculator)
                    {
                        var forceCalc = diagramCalculator.InputData as IHasPrimitives;
                        forceCalc.Primitives.Remove(ndmPrimitive);
                    }
                    else
                    {
                        throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(ICalculator), calc));
                    }
                }
                foreach (var primitive in repository.Primitives)
                {
                    if (primitive is IHasHostPrimitive sPrimitive)
                    {
                        if (sPrimitive.HostPrimitive == ndmPrimitive)
                        {
                            sPrimitive.HostPrimitive = null;
                        }
                    }
                }
                Items.Remove(SelectedItem);
            }
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(PrimitivesCount));
        }

        public ICommand Edit
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
            var ndmPrimitive = SelectedItem.GetNdmPrimitive();
            var primitiveCopy = ndmPrimitive.Clone() as INdmPrimitive;
            var wnd = new PrimitivePropertiesView(SelectedItem, repository);
            wnd.ShowDialog();
            if (wnd.DialogResult == true)
            {
                // to do save into repository
            }
            else
            {
                var updateStrategy = new NdmPrimitiveUpdateStrategy();
                updateStrategy.Update(ndmPrimitive, primitiveCopy);
                SelectedItem.Refresh();
            }

        }

        public ICommand Copy
        {
            get
            {
                return  copyCommand ??= new RelayCommand(
                        o => CopySelectedItem(SelectedItem.GetNdmPrimitive()),
                        o => SelectedItem != null
                    );
            }
        }

        public ICommand CopyTo
        {
            get
            {
                return copyToCommand ??
                    (
                    copyToCommand = new RelayCommand(
                        o => CopyToSelectedItem(SelectedItem.GetNdmPrimitive()),
                        o => SelectedItem != null
                    ));
            }
        }

        private void CopyToSelectedItem(INdmPrimitive ndmPrimitive)
        {
            var copyByParameterVM = new CopyByParameterViewModel(ndmPrimitive.Center);
            var wnd = new CopyByParameterView(copyByParameterVM);
            wnd.ShowDialog();
            if (wnd.DialogResult != true) { return;}
            var points = copyByParameterVM.GetNewItemCenters();
            foreach (var item in points)
            {
                var newPrimitive = CopySelectedItem(ndmPrimitive);
                newPrimitive.CenterX = item.X;
                newPrimitive.CenterY = item.Y;
            }
        }

        private PrimitiveBase CopySelectedItem(INdmPrimitive oldPrimitive)
        {
            var newPrimitive = oldPrimitive.Clone() as INdmPrimitive;
            newPrimitive.Name += " copy";
            repository.Primitives.Add(newPrimitive);
            PrimitiveBase primitiveBase;
            if (newPrimitive is IRectangleNdmPrimitive rectangle)
            {
                primitiveBase = new RectangleViewPrimitive(rectangle);
            }
            else if (newPrimitive is IEllipseNdmPrimitive ellipse)
            {
                primitiveBase = new CircleViewPrimitive(ellipse);
            }
            else if (newPrimitive is IShapeNdmPrimitive shapeNDMPrimitive)
            {
                primitiveBase = new ShapeViewPrimitive(shapeNDMPrimitive);
            }
            else if (newPrimitive is IPointNdmPrimitive)
            {
                if (newPrimitive is RebarNdmPrimitive rebar)
                {
                    primitiveBase = new ReinforcementViewPrimitive(rebar);
                }
                else
                {
                    primitiveBase = new PointViewPrimitive(newPrimitive as IPointNdmPrimitive);
                }

            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown);
            }
            primitiveBase.OnNext(this);
            Items.Add(primitiveBase);
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(PrimitivesCount));
            return primitiveBase;
        }

        public int PrimitivesCount => repository.Primitives.Count();

        public ICommand SetToFront
        {
            get
            {
                return setToFront ??= new RelayCommand(o=>
                    {
                        int maxZIndex = Items.Select(x => x.GetNdmPrimitive().VisualProperty.ZIndex).Max();
                        SelectedItem.ZIndex = maxZIndex + 1;
                    },o => CheckMaxIndex()
                    );
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

        public ICommand SetToBack
        {
            get
            {
                return setToBack ??= new RelayCommand(o =>
                    {
                        int minZIndex = Items.Select(x => x.GetNdmPrimitive().VisualProperty.ZIndex).Min();
                        SelectedItem.ZIndex = minZIndex - 1;
                    }, o => CheckMinIndex()
                    );
            }
        }

        public double Angle { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Guid Id => throw new NotImplementedException();

        public void AddItems(IEnumerable<PrimitiveBase> items)
        {
            foreach (var item in items)
            {
                Items.Add(item);
            }
        }

        public void Refresh()
        {
            NotifyObservers();
            OnPropertyChanged(nameof(PrimitivesCount));
        }

        public void NotifyObservers()
        {
            foreach (var item in Items)
            {
                item.OnNext(this);
            }
        }

        public IDisposable Subscribe(IObserver<PrimitiveBase> observer)
        {
            throw new NotImplementedException();
        }

        public PrimitiveViewModelLogic(ICrossSection section)
        {
            this.section = section;
            Items = new ObservableCollection<PrimitiveBase>();
            AddItems(PrimitiveOperations.ConvertNdmPrimitivesToPrimitiveBase(this.repository.Primitives));
        }

        private RelayCommand deleteSelectedCommand;
    }
}
