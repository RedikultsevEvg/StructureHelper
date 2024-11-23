using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using LoaderCalculator.Logics.Geometry;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Settings;
using StructureHelperCommon.Models.Parameters;
using StructureHelperCommon.Services.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public class ForcesParametersLogic : IParametersLogic
    {
        const string prefixInitial = "Initial";
        const string prefixActual = "Actual";
        private const string errorValue = "----";
        IConvertUnitLogic operationLogic = new ConvertUnitLogic();
        IGetUnitLogic unitLogic = new GetUnitLogic();

        static string firstAxisName => ProgramSetting.GeometryNames.FstAxisName;
        static string secondAxisName => ProgramSetting.GeometryNames.SndAxisName;
        static string thirdAxisName => ProgramSetting.GeometryNames.TrdAxisName;
        static IEnumerable<IUnit> units = UnitsFactory.GetUnitCollection();
        private IEnumerable<INdm> ndms;
        private IStrainMatrix strainMatrix;

        public ForcesParametersLogic(IEnumerable<INdm> ndms, IStrainMatrix strainMatrix)
        {
            this.ndms = ndms;
            this.strainMatrix = strainMatrix;
        }

        public List<IValueParameter<string>> GetTextParameters()
        {
            var parameters = new List<IValueParameter<string>>();
            parameters.AddRange(GetSummaryForces(ndms, strainMatrix));
            parameters.AddRange(GetSummaryMoments(ndms, strainMatrix));
            parameters.AddRange(GetForcesDistance(ndms, strainMatrix));
            parameters.AddRange(GetLiverArms(ndms, strainMatrix));
            return parameters;
        }
        private List<IValueParameter<string>> GetSummaryForces(IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix)
        {
            var parameters = new List<IValueParameter<string>>();
            var unitType = UnitTypes.Force;
            var unit = unitLogic.GetUnit(unitType, "kN");
            var unitName = unit.Name;
            var unitMultiPlayer = unit.Multiplyer;
            var forceSum = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"Summary force",
                ShortName = $"N{thirdAxisName.ToLower()},sum",
                Text = unitName,
                Description = $"Summary longitudinal force along {thirdAxisName.ToUpper()}-axis"
            };
            FillForceParameters(locNdms, locStrainMatrix, unitMultiPlayer, PosNegFlag.Both, forceSum);
            var forcePositive = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"Positive force",
                ShortName = $"N{thirdAxisName.ToLower()},pos",
                Text = unitName,
                Description = $"Summary of positive longitudinal force along {thirdAxisName.ToUpper()}-axis"
            };
            FillForceParameters(locNdms, locStrainMatrix, unitMultiPlayer, PosNegFlag.Positive, forcePositive);
            var forceNegative = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"Negative force",
                ShortName = $"N{thirdAxisName.ToLower()},neg",
                Text = unitName,
                Description = $"Summary of negative longitudinal force along {thirdAxisName.ToUpper()}-axis"
            };
            FillForceParameters(locNdms, locStrainMatrix, unitMultiPlayer, PosNegFlag.Negative, forceNegative);

            parameters.Add(forceSum);
            parameters.Add(forcePositive);
            parameters.Add(forceNegative);
            return parameters;
        }

        private static void FillForceParameters(IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix, double unitMultiPlayer, PosNegFlag posNegFlag, ValueParameter<string> forceSum)
        {
            try
            {
                var sumForceNz = GeometryOperations.GetSummaryForce(locNdms, locStrainMatrix, posNegFlag);
                forceSum.Value = (sumForceNz * unitMultiPlayer).ToString();
            }
            catch (Exception ex)
            {
                forceSum.IsValid = false;
                forceSum.Value = (double.NaN).ToString();
                forceSum.Description += $": {ex}";
            }
        }

        private List<IValueParameter<string>> GetSummaryMoments(IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix)
        {
            var parameters = new List<IValueParameter<string>>();
            var unitType = UnitTypes.Moment;
            var unit = unitLogic.GetUnit(unitType, "kNm");
            var unitName = unit.Name;
            var unitMultiPlayer = unit.Multiplyer;
            var momentSumValue = GeometryOperations.GetSummaryMoment(locNdms, locStrainMatrix, PosNegFlag.Both);
            var momentSumX = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"Summary moment",
                ShortName = $"M{firstAxisName.ToLower()},sum",
                Text = unitName,
                Value = (momentSumValue.dX * unitMultiPlayer).ToString(),
                Description = $"Summary moment arbitrary {firstAxisName.ToUpper()}-axis"
            };
            var momentSumY = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"Summary moment",
                ShortName = $"M{secondAxisName.ToLower()},sum",
                Text = unitName,
                Value = (momentSumValue.dY * unitMultiPlayer).ToString(),
                Description = $"Summary moment arbitrary {secondAxisName.ToUpper()}-axis"
            };
            parameters.Add(momentSumX);
            parameters.Add(momentSumY);
            momentSumValue = GeometryOperations.GetSummaryMoment(locNdms, locStrainMatrix, PosNegFlag.Positive);
            momentSumX = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"Summary positive moment",
                ShortName = $"M{firstAxisName.ToLower()},sum",
                Text = unitName,
                Value = (momentSumValue.dX * unitMultiPlayer).ToString(),
                Description = $"Summary moment of positive forces arbitrary {firstAxisName.ToUpper()}-axis"
            };
            momentSumY = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"Summary positive moment",
                ShortName = $"M{secondAxisName.ToLower()},sum",
                Text = unitName,
                Value = (momentSumValue.dY * unitMultiPlayer).ToString(),
                Description = $"Summary moment of positive forces arbitrary {secondAxisName.ToUpper()}-axis"
            };
            parameters.Add(momentSumX);
            parameters.Add(momentSumY);
            momentSumValue = GeometryOperations.GetSummaryMoment(locNdms, locStrainMatrix, PosNegFlag.Negative);
            momentSumX = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"Summary negative moment",
                ShortName = $"M{firstAxisName.ToLower()},sum",
                Text = unitName,
                Value = (momentSumValue.dX * unitMultiPlayer).ToString(),
                Description = $"Summary moment of negative forces arbitrary {firstAxisName.ToUpper()}-axis"
            };
            momentSumY = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"Summary negative moment",
                ShortName = $"M{secondAxisName.ToLower()},sum",
                Text = unitName,
                Value = (momentSumValue.dY * unitMultiPlayer).ToString(),
                Description = $"Summary moment of negative forces arbitrary {secondAxisName.ToUpper()}-axis"
            };
            parameters.Add(momentSumX);
            parameters.Add(momentSumY);
            return parameters;
        }
        private List<IValueParameter<string>> GetForcesDistance(IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix)
        {
            PosNegFlag flag = PosNegFlag.Both;
            string excentricityName = "Summary";
            Func<IEnumerable<INdm>, IStrainMatrix, PosNegFlag, (double dX, double dY, double dSum)> func = GeometryOperations.GetCenterOfForces;
            var parameters = new List<IValueParameter<string>>();
            parameters.AddRange(GetDistance(locNdms, locStrainMatrix, flag, excentricityName, func));
            flag = PosNegFlag.Positive;
            excentricityName = "Summary positive";
            parameters.AddRange(GetDistance(locNdms, locStrainMatrix, flag, excentricityName, func));
            flag = PosNegFlag.Negative;
            excentricityName = "Summary negative";
            parameters.AddRange(GetDistance(locNdms, locStrainMatrix, flag, excentricityName, func));
            return parameters;
        }

        private List<IValueParameter<string>> GetDistance(IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix, PosNegFlag flag, string eccentricityName, Func<IEnumerable<INdm>, IStrainMatrix, PosNegFlag, (double dX, double dY, double dSum)> func)
        {
            var parameters = new List<IValueParameter<string>>();
            var unitType = UnitTypes.Length;
            var unit = unitLogic.GetUnit(unitType, "mm");
            var unitName = unit.Name;
            var unitMultiPlayer = unit.Multiplyer;
            var sumEccenticityX = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{eccentricityName} eccentricity",
                ShortName = $"e{firstAxisName.ToLower()},sum",
                Text = unitName,
                Description = $"{eccentricityName} force excentricity along {firstAxisName.ToUpper()}-axis"
            };
            var sumExcenticityY = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{eccentricityName} eccentricity",
                ShortName = $"e{secondAxisName.ToLower()},sum",
                Text = unitName,
                Description = $"{eccentricityName} force eccentricity along {secondAxisName.ToUpper()}-axis"
            };
            var sumExcenticity = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{eccentricityName} eccentricity",
                ShortName = $"e,sum",
                Text = unitName,
                Description = $"{eccentricityName} force eccentricity"
            };
            try
            {
                var sumEccentricityValue = func.Invoke(locNdms, locStrainMatrix, flag);
                sumEccenticityX.Value = (sumEccentricityValue.dX * unitMultiPlayer).ToString();
                sumExcenticityY.Value = (sumEccentricityValue.dY * unitMultiPlayer).ToString();
                sumExcenticity.Value = (sumEccentricityValue.dSum * unitMultiPlayer).ToString();
            }
            catch (Exception ex)
            {
                sumEccenticityX.IsValid = false;
                sumEccenticityX.Value = errorValue;
                sumEccenticityX.Description += $": {ex}";

                sumExcenticityY.IsValid = false;
                sumExcenticityY.Value = errorValue;
                sumExcenticityY.Description += $": {ex}";

                sumExcenticity.IsValid = false;
                sumExcenticity.Value = errorValue;
                sumExcenticity.Description += $": {ex}";
            }

            parameters.Add(sumEccenticityX);
            parameters.Add(sumExcenticityY);
            parameters.Add(sumExcenticity);
            return parameters;
        }

        private List<IValueParameter<string>> GetLiverArms(IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix)
        {
            string excentricityName = "Summary";
            Func<IEnumerable<INdm>, IStrainMatrix, (double dX, double dY, double dSum)> func = GeometryOperations.GetDistanceBetweenPosNegForces;
            var parameters = new List<IValueParameter<string>>();
            parameters.AddRange(GetLiverArms(locNdms, locStrainMatrix, excentricityName, func));
            return parameters;
        }

        private IEnumerable<IValueParameter<string>> GetLiverArms(IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix, string eccentricityName, Func<IEnumerable<INdm>, IStrainMatrix, (double dX, double dY, double dSum)> func)
        {
            const string liverArm = "liver arm";
            var parameters = new List<IValueParameter<string>>();
            var unitType = UnitTypes.Length;
            var unit = unitLogic.GetUnit(unitType, "mm");
            var unitName = unit.Name;
            var unitMultiPlayer = unit.Multiplyer;
            var sumLiverArmX = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{eccentricityName} {liverArm}",
                ShortName = $"z,{firstAxisName.ToLower()}",
                Text = unitName,
                Description = $"{eccentricityName} {liverArm} along {firstAxisName.ToUpper()}-axis"
            };
            var sumLiverArmY = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{eccentricityName} {liverArm}",
                ShortName = $"z,{secondAxisName.ToLower()}",
                Text = unitName,
                Description = $"{eccentricityName} {liverArm} along {secondAxisName.ToUpper()}-axis"
            };
            var sumLiverArm = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{eccentricityName} {liverArm}",
                ShortName = $"z,sum",
                Text = unitName,
                Description = $"{eccentricityName} {liverArm}"
            };
            try
            {
                var sumLiverArmValue = func.Invoke(locNdms, locStrainMatrix);
                sumLiverArmX.Value = (sumLiverArmValue.dX * unitMultiPlayer).ToString();
                sumLiverArmY.Value = (sumLiverArmValue.dY * unitMultiPlayer).ToString();
                sumLiverArm.Value = (sumLiverArmValue.dSum * unitMultiPlayer).ToString();
            }
            catch (Exception ex)
            {
                sumLiverArmX.IsValid = false;
                sumLiverArmX.Value = errorValue;
                sumLiverArmX.Description += $": {ex}";

                sumLiverArmY.IsValid = false;
                sumLiverArmY.Value = errorValue;
                sumLiverArmY.Description += $": {ex}";

                sumLiverArm.IsValid = false;
                sumLiverArm.Value = errorValue;
                sumLiverArm.Description += $": {ex}";
            }

            parameters.Add(sumLiverArmX);
            parameters.Add(sumLiverArmY);
            parameters.Add(sumLiverArm);
            return parameters;
        }
    }
}
