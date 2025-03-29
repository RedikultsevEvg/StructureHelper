using StructureHelper.Infrastructure.Enums;
using StructureHelper.Windows.ViewModels;
using StructureHelper.Windows.ViewModels.Errors;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.Models.BeamShears;
using System;
using System.Windows;

namespace StructureHelper.Windows.BeamShears
{
    public class BeamShearSectionsViewModel : SelectItemVM<IBeamShearSection>
    {
        private const string ErrorText = "Error of creating of section";
        private IUpdateStrategy<IBeamShearSection> updateStrategy;
        private IBeamShearRepository shearRepository;
        private PrimitiveType sectionType;
        public BeamShearSectionsViewModel(IBeamShearRepository shearRepository) : base(shearRepository.Sections)
        {
            this.shearRepository = shearRepository;
        }
        public override void EditMethod(object parameter)
        {
            if (SelectedItem is null) { return; }
            SafetyProcessor.RunSafeProcess(EditSection, "Error of editing of section");
            base.EditMethod(parameter);
        }

        public override void AddMethod(object parameter)
        {
            if (parameter is PrimitiveType primitiveType)
            {
                sectionType = primitiveType;
                SafetyProcessor.RunSafeProcess(AddSection, ErrorText);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(parameter));
            }
            base.AddMethod(parameter);
        }

        private void AddSection()
        {
            if (sectionType is PrimitiveType.Rectangle)
            {
                NewItem = new BeamShearSection(Guid.NewGuid())
                {
                    Name = "New rectangle section"
                };
            }
        }

        private void EditSection()
        {
            Window window;
            IBeamShearSection temporarySection = SelectedItem.Clone() as IBeamShearSection;
            window = new SectionView(SelectedItem);
            window.ShowDialog();
            if (window.DialogResult != true)
            {
                updateStrategy ??= new BeamShearSectionUpdateStrategy();
                updateStrategy.Update(SelectedItem, temporarySection);
            }
        }

        public override void DeleteMethod(object parameter)
        {
            BeamShearRepositoryService.DeleteSection(shearRepository, SelectedItem);
            base.DeleteMethod(parameter);
        }
    }
}
