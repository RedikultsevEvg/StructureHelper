using StructureHelper.Infrastructure;
using StructureHelper.Infrastructure.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperLogics.Models.Analyses;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearViewModel : ViewModelBase
    {
        private IBeamShear beamShear;
        private IBeamShearRepository repository;
        private RelayCommand addTemplateCommand;

        public BeamShearActionsViewModel Actions {get; private set;}
        public BeamShearSectionsViewModel Sections { get; private set; }
        public BeamStirrupsViewModel Stirrups { get; private set; }
        public BeamShearCalculatorsViewModel Calculators { get; private set; }

        public ICommand AddTemplate
        {
            get
            {
                return addTemplateCommand ??
                    (
                    addTemplateCommand = new RelayCommand(param =>
                    {
                        AddTemplateMethod(param);
                    }
                    ));
            }
        }

        private void AddTemplateMethod(object parameter)
        {
            if (parameter is PrimitiveType.Rectangle)
            {
                var templateRepository = BeamShearTemplatesFactory.GetTemplateRepository(ShearSectionTemplateTypes.Rectangle);
                var updateStrategy = new BeamShearRepositoryAddUpdateStrategy();
                updateStrategy.Update(repository, templateRepository);
                Refresh();
            }
            else if (parameter is PrimitiveType.Circle)
            {
                var templateRepository = BeamShearTemplatesFactory.GetTemplateRepository(ShearSectionTemplateTypes.Circle);
                var updateStrategy = new BeamShearRepositoryAddUpdateStrategy();
                updateStrategy.Update(repository, templateRepository);
                Refresh();
            }
            else if (parameter is PrimitiveType.TShape)
            {
                var templateRepository = BeamShearTemplatesFactory.GetTemplateRepository(ShearSectionTemplateTypes.TShape);
                var updateStrategy = new BeamShearRepositoryAddUpdateStrategy();
                updateStrategy.Update(repository, templateRepository);
                Refresh();
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(parameter));
            }
        }

        private void Refresh()
        {
            Actions.Refresh();
            Sections.Refresh();
            Stirrups.Refresh();
            Calculators.Refresh();
        }

        public BeamShearViewModel(IBeamShear beamShear)
        {
            this.beamShear = beamShear;
            repository = beamShear.Repository;
            InitializeSubModels();
        }

        private void InitializeSubModels()
        {
            Actions = new (repository);
            Sections = new (repository);
            Stirrups = new (repository);
            Calculators = new (repository);
        }
    }
}
