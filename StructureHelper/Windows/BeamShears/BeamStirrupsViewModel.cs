using StructureHelper.Infrastructure.Enums;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;
using StructureHelperLogics.Models.BeamShears.Logics;
using StructureHelperLogics.Models.Materials;
using System;
using System.Windows;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamStirrupsViewModel : SelectItemVM<IStirrup>
    {
        private const string ErrorText = "Error of creating of stirrup";
        private IUpdateStrategy<IStirrup> updateStrategy;
        private readonly IBeamShearRepository shearRepository;
        private StirrupTypes stirrupType;

        public BeamStirrupsViewModel(IBeamShearRepository shearRepository) : base(shearRepository.Stirrups)
        {
            this.shearRepository = shearRepository;
        }

        public override void EditMethod(object parameter)
        {
            if (SelectedItem is null) { return; }
            SafetyProcessor.RunSafeProcess(EditStirrup, "Error of editing of stirrup");
            base.EditMethod(parameter);
        }

        private void EditStirrup()
        {
            Window window;
            IStirrup temporaryStirrup = SelectedItem.Clone() as IStirrup;
            if (SelectedItem is IStirrupByDensity stirrupByDensity)
            {
                window = new StirrupByDensityView(stirrupByDensity);
            }
            //else if (SelectedItem is IStirrupByUniformRebar stirrupByUniformRebar)
            //{
            //    window = ;
            //}
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(SelectedItem));
            }
            window.ShowDialog();
            if (window.DialogResult != true)
            {
                updateStrategy ??= new StirrupUpdateStrategy();
            }
        }

        public override void AddMethod(object parameter)
        {
            if (parameter is StirrupTypes stirrupParameter)
            {
                stirrupType = stirrupParameter;
                SafetyProcessor.RunSafeProcess(AddStirrup, ErrorText);
            }
            else
            {
                SafetyProcessor.ShowMessage(ErrorText, $"parameter type is {parameter.GetType()}, not valid type of stirrup");
                return;
            }
        }

        private void AddStirrup()
        {
            if (stirrupType is StirrupTypes.Density)
            {
                AddStirrupByDensity();
            }
            if (stirrupType is StirrupTypes.UniformRebar)
            {
                AddUniformRebarStirrup();
            }
            base.AddMethod(stirrupType);
        }

        private void AddUniformRebarStirrup()
        {
            IReinforcementLibMaterial reinforcement = new ReinforcementLibMaterial(Guid.NewGuid());
            NewItem = new StirrupByUniformRebar(Guid.NewGuid())
            {
                Name = "New stirrup by uniformly distributed rebar",
                Diameter = 0.008,
                LegCount = 2,
                Step = 0.1,
                Material = reinforcement
            };
        }

        private void AddStirrupByDensity()
        {
            NewItem = new StirrupByDensity(Guid.NewGuid())
            {
                Name = "New stirrup by density",
                StirrupDensity = 3e5
            };
        }

        public override void DeleteMethod(object parameter)
        {
            shearRepository.DeleteStirrup(SelectedItem);
            base.DeleteMethod(parameter);
        }
    }
}
