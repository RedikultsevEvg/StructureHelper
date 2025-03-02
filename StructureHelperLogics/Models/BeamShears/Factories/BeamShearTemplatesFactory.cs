using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public enum ShearSectionTemplateTypes
    {
        Rectangle,
    }
    public static class BeamShearTemplatesFactory
    {
        public static IBeamShearRepository GetTemplateRepository(ShearSectionTemplateTypes templateType)
        {
            if (templateType is ShearSectionTemplateTypes.Rectangle)
            {
                return GetRectangleSection();
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(templateType));
            }
        }

        private static IBeamShearRepository GetRectangleSection()
        {
            BeamShearRepository shearRepository = new(Guid.Empty);
            BeamShearSection section = new(Guid.Empty) { Name = "New shear section"};
            shearRepository.ShearSections.Add(section);
            StirrupByUniformRebar stirrupByUniformRebar = new(Guid.Empty) { Name = "New uniform stirrup"};
            shearRepository.Stirrups.Add(stirrupByUniformRebar);
            BeamShearCalculator beamShearCalculator = new(Guid.Empty) { Name = "New shear calculator"};
            beamShearCalculator.InputData.ShearSections.Add(section);
            beamShearCalculator.InputData.Stirrups.Add(stirrupByUniformRebar);
            shearRepository.Calculators.Add(beamShearCalculator);
            return shearRepository;
        }
    }
}
