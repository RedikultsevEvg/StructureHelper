using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.BeamShears.Logics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    public class GetSectionEffectivenessLogic : IGetSectionEffectivenessLogic
    {
        ISectionEffectivenessFactory sectionEffectivenessFactory;
        public IShiftTraceLogger? TraceLogger { get; set; }

        public ISectionEffectiveness GetSectionEffectiveness(ShearCodeTypes shearCodeTypes, IShape shape)
        {
            InitializeFactory(shearCodeTypes);
            ISectionEffectiveness sectionEffectiveness = GetEffectiveness(shape);
            return sectionEffectiveness;
        }

        private ISectionEffectiveness GetEffectiveness(IShape shape)
        {
            ISectionEffectiveness sectionEffectiveness;
            if (shape is IRectangleShape)
            {
                sectionEffectiveness = sectionEffectivenessFactory.GetShearEffectiveness(BeamShearSectionType.Rectangle);
            }
            else if (shape is ICircleShape)
            {
                sectionEffectiveness = sectionEffectivenessFactory.GetShearEffectiveness(BeamShearSectionType.Circle);
            }
            else
            {
                throw new StructureHelperException(ErrorStrings.ObjectTypeIsUnknownObj(shape));
            }

            return sectionEffectiveness;
        }

        private void InitializeFactory(ShearCodeTypes shearCodeTypes)
        {
            if (shearCodeTypes == ShearCodeTypes.SP_63_13330_2018_3)
            {
                sectionEffectivenessFactory = new SectionEffectivenessFactorySP2018();
            }
            else if (shearCodeTypes == ShearCodeTypes.StructureHelper_0)
            {
                sectionEffectivenessFactory = new SectionEffectivenessFactorySH();
            }
            else
            {
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(shearCodeTypes) + $": design code type {shearCodeTypes} for shear calculation is unknown";
                TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
                throw new StructureHelperException(errorString);
            }
        }
    }
}
