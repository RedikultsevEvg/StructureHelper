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
using System.Windows.Forms;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelper.Windows.BeamShears
{
    /// <summary>
    /// Implements logic of CRUD operations with stirrups in beam shear calculations
    /// </summary>
    public class BeamStirrupsViewModel : SelectItemVM<IStirrup>
    {
        private const string ErrorText = "Error of creating of stirrup";
        private IUpdateStrategy<IStirrup> updateStrategy;
        private readonly IHasStirrups hasStirrup;
        private StirrupTypes stirrupType;

        public BeamStirrupsViewModel(IHasStirrups hasStirrup) : base(hasStirrup.Stirrups)
        {
            this.hasStirrup = hasStirrup;
        }
        /// <inheritdoc/>
        public override void EditMethod(object parameter)
        {
            if (SelectedItem is null) { return; }
            SafetyProcessor.RunSafeProcess(EditStirrup, "Error of editing of stirrup");
            base.EditMethod(parameter);
        }
        /// <inheritdoc/>
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
        private void EditStirrup()
        {
            Window window;
            IStirrup temporaryStirrup = SelectedItem.Clone() as IStirrup;
            if (SelectedItem is IStirrupGroup stirrupGroup)
            {
                window = new StirrupGroupView(stirrupGroup);
            }
            else if (SelectedItem is IStirrupByDensity stirrupByDensity)
            {
                window = new StirrupByDensityView(stirrupByDensity);
            }
            else if (SelectedItem is IStirrupByRebar stirrupByRebar)
            {
                window = new StirrupByRebarView(stirrupByRebar);
            }
            else if (SelectedItem is IStirrupByInclinedRebar stirrupByInclinedRebar)
            {
                window = new StirrupByInclinedReebarView(stirrupByInclinedRebar);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(SelectedItem));
            }
            window.ShowDialog();
            if (window.DialogResult != true)
            {
                updateStrategy ??= new StirrupUpdateStrategy();
                updateStrategy.Update(SelectedItem, temporaryStirrup);
            }
        }

        private void AddStirrup()
        {
            if (stirrupType is StirrupTypes.GroupOfStirrups)
            {
                AddStirrupGroup();
            }
            else if (stirrupType is StirrupTypes.Density)
            {
                AddStirrupByDensity();
            }
            else if (stirrupType is StirrupTypes.UniformRebar)
            {
                AddUniformRebarStirrup();
            }
            else if (stirrupType is StirrupTypes.InclinedRebar)
            {
                AddInclinedRebarStirrup();
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(stirrupType));
            }
            base.AddMethod(stirrupType);
        }

        private void AddInclinedRebarStirrup()
        {
            NewItem = new StirrupByInclinedRebar(Guid.NewGuid())
            {
                Name = "New inclined rebar"
            };
        }

        private void AddStirrupGroup()
        {
            NewItem = new StirrupGroup(Guid.NewGuid())
            {
                Name = "New group of stirrups"
            };
        }

        private void AddUniformRebarStirrup()
        {
            IReinforcementLibMaterial reinforcement = HeadMaterialFactory.GetHeadMaterial(HeadmaterialType.Reinforcement400).HelperMaterial as IReinforcementLibMaterial;
            NewItem = new StirrupByRebar(Guid.NewGuid())
            {
                Name = "New stirrup by uniformly distributed rebar",
                Diameter = 0.008,
                LegCount = 2,
                Spacing = 0.1,
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
            var dialogResult = System.Windows.Forms.MessageBox.Show("Delete stirrup?", "Please, confirm deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.Yes)
            {
                if (hasStirrup is IBeamShearRepository repository)
                { 
                    BeamShearRepositoryService.DeleteStirrup(repository, SelectedItem);
                }
                base.DeleteMethod(parameter);
            }
        }
    }
}
