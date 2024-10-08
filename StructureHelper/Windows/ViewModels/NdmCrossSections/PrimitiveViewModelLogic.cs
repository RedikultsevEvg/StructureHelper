using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelper.Infrastructure.UI.DataContexts;
using StructureHelper.Services.Settings;
using StructureHelper.Windows.PrimitiveProperiesWindow;
using StructureHelper.Windows.PrimitiveTemplates.RCs.Beams;
using StructureHelper.Windows.PrimitiveTemplates.RCs.RectangleBeam;
using StructureHelper.Windows.Services;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Materials;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.Models.Templates.CrossSections.RCs;
using StructureHelperLogics.Models.Templates.RCs;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using StructureHelperLogics.NdmCalculations.Cracking;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;

//Copyright (c) 2023 Redikultsev Evgeny, Ekaterinburg, Russia
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


        public double Width { get; set; }
        public double Height { get; set; }

        public PrimitiveBase SelectedItem { get; set; }

        public ObservableCollection<PrimitiveBase> Items { get; private set; }

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
                var primitive = new RectanglePrimitive
                {
                    Width = 0.4d,
                    Height = 0.6d
                };
                ndmPrimitive = primitive;
                viewPrimitive = new RectangleViewPrimitive(primitive);

            }
            else if (primitiveType == PrimitiveType.Reinforcement)
            {
                var primitive = new RebarPrimitive
                {
                    Area = 0.0005d
                };
                ndmPrimitive = primitive;
                viewPrimitive = new ReinforcementViewPrimitive(primitive);
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
                var primitive = new EllipsePrimitive
                {
                    DiameterByX = 0.5d
                };
                ndmPrimitive = primitive;
                viewPrimitive = new CircleViewPrimitive(primitive);
            }
            else { throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknown + nameof(primitiveType)); }
            viewPrimitive.OnNext(this);
            repository.Primitives.Add(ndmPrimitive);
            ndmPrimitive.CrossSection = section;
            Items.Add(viewPrimitive);
            OnPropertyChanged(nameof(Items));
            OnPropertyChanged(nameof(PrimitivesCount));
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
                    if (calc is ForceCalculator forceCalculator)
                    {
                        var forceCalc = forceCalculator.InputData as IHasPrimitives;
                        forceCalc.Primitives.Remove(ndmPrimitive);
                    }
                    else if (calc is LimitCurvesCalculator calculator)
                    {
                        //to do
                        //var forceCalc = calculator.InputData as IHasPrimitives;
                        //forceCalc.Primitives.Remove(ndmPrimitive);
                    }
                    else if (calc is CrackCalculator crackCalculator)
                    {
                        var forceCalc = crackCalculator.InputData as IHasPrimitives;
                        forceCalc.Primitives.Remove(ndmPrimitive);
                    }
                    else
                    {
                        throw new StructureHelperException(ErrorStrings.ExpectedWas(typeof(ICalculator), calc));
                    }
                }
                foreach (var primitive in repository.Primitives)
                {
                    if (primitive is IHasHostPrimitive)
                    {
                        var sPrimitive = primitive as IHasHostPrimitive;
                        if (sPrimitive.HostPrimitive == ndmPrimitive) { sPrimitive.HostPrimitive = null; }
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
                return                     copyCommand ??= new RelayCommand(
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
            if (newPrimitive is IRectanglePrimitive)
            {
                primitiveBase = new RectangleViewPrimitive(newPrimitive as IRectanglePrimitive);
            }
            else if (newPrimitive is IEllipsePrimitive)
            {
                primitiveBase = new CircleViewPrimitive(newPrimitive as IEllipsePrimitive);
            }
            else if (newPrimitive is IPointPrimitive)
            {
                if (newPrimitive is RebarPrimitive)
                {
                    primitiveBase = new ReinforcementViewPrimitive(newPrimitive as RebarPrimitive);
                }
                else
                {
                    primitiveBase = new PointViewPrimitive(newPrimitive as IPointPrimitive);
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

    }
}
