using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.Models.BeamShears
{
    public enum ShearSectionTemplateTypes
    {
        Rectangle,
        Circle,
        TShape
    }
    public static class BeamShearTemplatesFactory
    {
        public static IBeamShearRepository GetTemplateRepository(ShearSectionTemplateTypes templateType)
        {
            if (templateType is ShearSectionTemplateTypes.Rectangle)
            {
                return GetRectangleSection();
            }
            if (templateType is ShearSectionTemplateTypes.Circle)
            {
                return GetCircleSection();
            }
            if (templateType is ShearSectionTemplateTypes.TShape)
            {
                return GetTShapeSection();
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(templateType));
            }
        }

        private static IBeamShearRepository GetTShapeSection()
        {
            BeamShearSection section = new(Guid.NewGuid())
            {
                Name = "New T-Shape section",
                Shape = new VerticalTShape(Guid.NewGuid()),
            };
            return BuildRepository(section);
        }

        private static IBeamShearRepository GetCircleSection()
        {
            BeamShearSection section = new(Guid.NewGuid())
            {
                Name = "New circle section",
                Shape = new CircleShape(Guid.NewGuid()) { Diameter = 0.6},
            };
            return BuildRepository(section);
        }

        private static IBeamShearRepository GetRectangleSection()
        {
            BeamShearSection section = new(Guid.NewGuid())
            {
                Name = "New rectangle section",
                Shape = new RectangleShape(Guid.NewGuid()) { Height = 0.6, Width = 0.4 },
            };
            return BuildRepository(section);
        }

        private static IBeamShearRepository BuildRepository(BeamShearSection section)
        {
            BeamShearRepository shearRepository = new(Guid.Empty);
            IBeamShearAction shearAction = BeamShearActionFactory.GetBeamShearAction(ShearActionTypes.DistributedLoad);
            shearAction.Name = "New shear action";
            shearRepository.Actions.Add(shearAction);
            shearRepository.Sections.Add(section);
            StirrupByRebar stirrupByUniformRebar = new(Guid.NewGuid()) { Name = "New uniform stirrup" };
            shearRepository.Stirrups.Add(stirrupByUniformRebar);
            BeamShearCalculator beamShearCalculator = new(Guid.NewGuid()) { Name = "New shear calculator" };
            beamShearCalculator.InputData.Sections.Add(section);
            beamShearCalculator.InputData.Stirrups.Add(stirrupByUniformRebar);
            beamShearCalculator.InputData.Actions.Add(shearAction);
            shearRepository.Calculators.Add(beamShearCalculator);
            return shearRepository;
        }
    }
}
