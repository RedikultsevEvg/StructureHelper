﻿using StructureHelperCommon.Infrastructures.Enums;
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
        private static IUnit unitForce = CommonOperation.GetUnit(UnitTypes.Force, "kN");
        private static IUnit unitMoment = CommonOperation.GetUnit(UnitTypes.Moment, "kNm");
        private static IUnit unitCurvature = CommonOperation.GetUnit(UnitTypes.Curvature, "1/m");
        private static GeometryNames GeometryNames => ProgramSetting.GeometryNames;
        public static List<string> GetLabels()
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
