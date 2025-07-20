using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public enum ShearSectionTemplateTypes
    {
        Rectangle,
        Circle
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
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(templateType));
            }
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
