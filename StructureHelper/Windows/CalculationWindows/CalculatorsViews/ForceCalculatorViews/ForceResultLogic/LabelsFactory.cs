using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews
{
    public static class LabelsFactory
    {
        static IConvertUnitLogic operationLogic = new ConvertUnitLogic();
        static IGetUnitLogic unitLogic = new GetUnitLogic();
        private static IUnit unitForce = unitLogic.GetUnit(UnitTypes.Force);
        private static IUnit unitMoment = unitLogic.GetUnit(UnitTypes.Moment);
        private static IUnit unitCurvature = unitLogic.GetUnit(UnitTypes.Curvature);
        private static GeometryNames GeometryNames => ProgramSetting.GeometryNames;
        public static List<string> GetCommonLabels()
        {
            var labels = new List<string>
            {
                $"{GeometryNames.MomFstName}, {unitMoment.Name}",
                $"{GeometryNames.MomSndName}, {unitMoment.Name}",
                $"{GeometryNames.LongForceName}, {unitForce.Name}",
                $"{GeometryNames.CurvFstName}, {unitCurvature.Name}",
                $"{GeometryNames.CurvSndName}, {unitCurvature.Name}",
                $"{GeometryNames.StrainTrdName}",
            };
            return labels;
        }
    }
}
