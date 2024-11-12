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
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public class TextParametersLogic
    {
        const string prefixInitial = "Initial";
        const string prefixActual = "Actual";
        IConvertUnitLogic operationLogic = new ConvertUnitLogic();
        IGetUnitLogic unitLogic = new GetUnitLogic();

        static string firstAxisName => ProgramSetting.GeometryNames.FstAxisName;
        static string secondAxisName => ProgramSetting.GeometryNames.SndAxisName;
        static string thirdAxisName => ProgramSetting.GeometryNames.TrdAxisName;
        static IEnumerable<IUnit> units = UnitsFactory.GetUnitCollection();
        private IEnumerable<INdm> ndms;
        private IStrainMatrix strainMatrix;
        public List<IValueParameter<string>> GetTextParameters()
        {
            var parameters = new List<IValueParameter<string>>();
            parameters.AddRange(GetGravityCenter(prefixInitial, ndms));
            parameters.AddRange(GetSimpleArea(ndms));
            parameters.AddRange(GetArea(prefixInitial, ndms));
            parameters.AddRange(GetMomentOfInertia(prefixInitial, ndms));
            parameters.AddRange(GetGravityCenter(prefixActual, ndms, strainMatrix));
            parameters.AddRange(GetArea(prefixActual, ndms, strainMatrix));
            parameters.AddRange(GetMomentOfInertia(prefixActual, ndms, strainMatrix));
            parameters.AddRange(GetAreaRatio(ndms, strainMatrix));
            parameters.AddRange(GetMomentOfInertiaRatio(ndms, strainMatrix));
            parameters.AddRange(GetSummaryForces(ndms, strainMatrix));
            parameters.AddRange(GetSummaryMoments(ndms, strainMatrix));
            parameters.AddRange(GetForcesDistance(ndms, strainMatrix));
            parameters.AddRange(GetLiverArms(ndms, strainMatrix));
            return parameters;
        }
        private IEnumerable<IValueParameter<string>> GetSimpleArea(IEnumerable<INdm> ndms)
        {
            const string name = "Summary Area";
            const string shortName = "A";
            var parameters = new List<IValueParameter<string>>();
            var unitArea = unitLogic.GetUnit(UnitTypes.Area, "mm2");
            var unitName = $"{unitArea.Name}";
            var unitMultiPlayer = unitArea.Multiplyer;
            var firstParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{name}",
                ShortName = $"{shortName}",
                Text = unitName,
                Description = $"{name} of cross-section without reduction"
            };
            try
            {
                firstParameter.Value = (ndms.Sum(x => x.Area) * unitMultiPlayer).ToString();
            }
            catch (Exception ex)
            {
                firstParameter.IsValid = false;
                firstParameter.Value = (double.NaN).ToString();
                firstParameter.Description += $": {ex}";
            }
            parameters.Add(firstParameter);
            return parameters;
        }
        private IEnumerable<IValueParameter<string>> GetMomentOfInertia(string prefix, IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix = null)
        {
            const string name = "Bending stiffness";
            const string shortName = "EI";
            var parameters = new List<IValueParameter<string>>();
            var unitArea = unitLogic.GetUnit(UnitTypes.Area, "mm2");
            var unitStress = unitLogic.GetUnit(UnitTypes.Stress, "MPa");
            var unitName = $"{unitStress.Name} * {unitArea.Name} * {unitArea.Name}";
            var unitMultiPlayer = unitArea.Multiplyer * unitArea.Multiplyer * unitStress.Multiplyer;
            var firstParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefix} {name} {firstAxisName.ToUpper()}",
                ShortName = $"{shortName}{firstAxisName}",
                Text = unitName,
                Description = $"{prefix} {name} of cross-section arbitrary {firstAxisName}-axis multiplied by {prefix} modulus"
            };
            var secondParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefix} {name} {secondAxisName}",
                ShortName = $"{shortName}{secondAxisName}",
                Text = unitName,
                Description = $"{prefix} {name} of cross-section arbitrary {secondAxisName}-axis multiplied by {prefix} modulus"
            };
            try
            {
                var gravityCenter = GeometryOperations.GetReducedMomentsOfInertia(locNdms, locStrainMatrix);
                firstParameter.Value = (gravityCenter.EIx * unitMultiPlayer).ToString();
                secondParameter.Value = (gravityCenter.EIy * unitMultiPlayer).ToString();
            }
            catch (Exception ex)
            {
                firstParameter.IsValid = false;
                firstParameter.Value = (double.NaN).ToString();
                firstParameter.Description += $": {ex}";
                secondParameter.IsValid = false;
                secondParameter.Value = (double.NaN).ToString();
                secondParameter.Description += $": {ex}";
            }
            parameters.Add(firstParameter);
            parameters.Add(secondParameter);
            return parameters;
        }
        private IEnumerable<IValueParameter<string>> GetMomentOfInertiaRatio(IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix = null)
        {
            const string name = "Bending stiffness";
            const string shortName = "EI";
            var parameters = new List<IValueParameter<string>>();
            var firstParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefixActual}/{prefixInitial} {name} {firstAxisName.ToUpper()} ratio",
                ShortName = $"{shortName}{firstAxisName}-ratio",
                Text = "-",
                Description = $"{prefixActual}/{prefixInitial} {name} of cross-section arbitrary {firstAxisName}-axis ratio"
            };
            var secondParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefixActual}/{prefixInitial} {name} {secondAxisName} ratio",
                ShortName = $"{shortName}{secondAxisName}-ratio",
                Text = "-",
                Description = $"{prefixActual}/{prefixInitial} {name} of cross-section arbitrary {secondAxisName}-axis ratio"
            };
            try
            {
                var actualMoments = GeometryOperations.GetSofteningsFactors(locNdms, locStrainMatrix);
                firstParameter.Value = actualMoments.EIxFactor.ToString();
                secondParameter.Value = actualMoments.EIy.ToString();
            }
            catch (Exception ex)
            {
                firstParameter.IsValid = false;
                firstParameter.Value = (double.NaN).ToString();
                firstParameter.Description += $": {ex}";
                secondParameter.IsValid = false;
                secondParameter.Value = (double.NaN).ToString();
                secondParameter.Description += $": {ex}";
            }
            parameters.Add(firstParameter);
            parameters.Add(secondParameter);
            return parameters;
        }
        private IEnumerable<IValueParameter<string>> GetArea(string prefix, IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix = null)
        {
            const string name = "Longitudinal stiffness";
            const string shortName = "EA";
            var parameters = new List<IValueParameter<string>>();
            var unitArea = unitLogic.GetUnit(UnitTypes.Area, "mm2");
            var unitStress = unitLogic.GetUnit(UnitTypes.Stress, "MPa");
            var unitName = $"{unitStress.Name} * {unitArea.Name}" ;
            var unitMultiPlayer = unitArea.Multiplyer * unitStress.Multiplyer;
            var firstParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefix} {name}",
                ShortName = $"{shortName}",
                Text = unitName,
                Description = $"{prefix} {name} of cross-section multiplied by {prefix} modulus"
            };
            try
            {
                firstParameter.Value = (GeometryOperations.GetReducedArea(locNdms, locStrainMatrix) * unitMultiPlayer).ToString();
            }
            catch (Exception ex)
            {
                firstParameter.IsValid = false;
                firstParameter.Value = (double.NaN).ToString();
                firstParameter.Description += $": {ex}";
            }
            parameters.Add(firstParameter);
            return parameters;
        }
        private IEnumerable<IValueParameter<string>> GetAreaRatio(IEnumerable<INdm> locNdms, IStrainMatrix locStrainMatrix)
        {
            const string name = "Longitudinal stiffness";
            const string shortName = "EA";
            var parameters = new List<IValueParameter<string>>();
            var firstParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefixActual}/{prefixInitial} {name} ratio",
                ShortName = $"{shortName}-ratio",
                Text = "-",
                Description = $"{prefixActual}/{prefixInitial} {name}-ratio of cross-section"
            };
            try
            {
                var actual = GeometryOperations.GetSofteningsFactors(locNdms, locStrainMatrix);
                firstParameter.Value = actual.EAFactor.ToString();
            }
            catch (Exception ex)
            {
                firstParameter.IsValid = false;
                firstParameter.Value = (double.NaN).ToString();
                firstParameter.Description += $": {ex}";
            }
            parameters.Add(firstParameter);
            return parameters;
        }
        public TextParametersLogic(IEnumerable<INdm> ndms, IStrainMatrix strainMatrix)
        {
            this.ndms = ndms;
            this.strainMatrix = strainMatrix;
        }
        private IEnumerable<IValueParameter<string>> GetGravityCenter(string prefix, IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix = null)
        {
            var parameters = new List<IValueParameter<string>>();
            var unitType = UnitTypes.Length;
            var unit = unitLogic.GetUnit(unitType, "mm");
            var unitName = unit.Name;
            var unitMultiPlayer = unit.Multiplyer;
            var firstParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefix} Center{firstAxisName.ToUpper()}",
                ShortName = $"{firstAxisName.ToUpper()}c",
                Text = unitName,
                Description = $"{prefix} Displacement of center of gravity of cross-section along {firstAxisName}-axis"
            };
            var secondParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefix} Center{secondAxisName.ToUpper()}",
                ShortName = $"{secondAxisName.ToUpper()}c",
                Text = unitName,
                Description = $"{prefix} Displacement of center of gravity of cross-section along {secondAxisName}-axis"
            };
            try
            {
                var gravityCenter = GeometryOperations.GetGravityCenter(locNdms, locStrainMatrix);
                firstParameter.Value = (gravityCenter.Cx * unitMultiPlayer).ToString();
                secondParameter.Value = (gravityCenter.Cy * unitMultiPlayer).ToString();
            }
            catch (Exception ex)
            {
                firstParameter.IsValid = false;
                firstParameter.Value = (double.NaN).ToString();
                firstParameter.Description += $": {ex}";
                secondParameter.IsValid = false;
                secondParameter.Value = (double.NaN).ToString();
                secondParameter.Description += $": {ex}";
            }
            parameters.Add(firstParameter);
            parameters.Add(secondParameter);
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

        private List<IValueParameter<string>> GetDistance(IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix, PosNegFlag flag, string excentricityName, Func<IEnumerable<INdm>, IStrainMatrix, PosNegFlag, (double dX, double dY, double dSum)> func)
        {
            var parameters = new List<IValueParameter<string>>();
            var unitType = UnitTypes.Length;
            var unit = unitLogic.GetUnit(unitType, "mm");
            var unitName = unit.Name;
            var unitMultiPlayer = unit.Multiplyer;
            var sumExcenticityX = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{excentricityName} excentricity",
                ShortName = $"e{firstAxisName.ToLower()},sum",
                Text = unitName,
                Description = $"{excentricityName} force excentricity along {firstAxisName.ToUpper()}-axis"
            };
            var sumExcenticityY = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{excentricityName} excentricity",
                ShortName = $"e{secondAxisName.ToLower()},sum",
                Text = unitName,
                Description = $"{excentricityName} force excentricity along {secondAxisName.ToUpper()}-axis"
            };
            var sumExcenticity = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{excentricityName} excentricity",
                ShortName = $"e,sum",
                Text = unitName,
                Description = $"{excentricityName} force excentricity"
            };
            try
            {
                var sumExcentricityValue = func.Invoke(locNdms, locStrainMatrix, flag);
                sumExcenticityX.Value = (sumExcentricityValue.dX * unitMultiPlayer).ToString();
                sumExcenticityY.Value = (sumExcentricityValue.dY * unitMultiPlayer).ToString();
                sumExcenticity.Value = (sumExcentricityValue.dSum * unitMultiPlayer).ToString();
            }
            catch (Exception ex)
            {
                sumExcenticityX.IsValid = false;
                sumExcenticityX.Value = (double.NaN).ToString();
                sumExcenticityX.Description += $": {ex}";

                sumExcenticityY.IsValid = false;
                sumExcenticityY.Value = (double.NaN).ToString();
                sumExcenticityY.Description += $": {ex}";

                sumExcenticity.IsValid = false;
                sumExcenticity.Value = (double.NaN).ToString();
                sumExcenticity.Description += $": {ex}";
            }

            parameters.Add(sumExcenticityX);
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

        private IEnumerable<IValueParameter<string>> GetLiverArms(IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix, string excentricityName, Func<IEnumerable<INdm>, IStrainMatrix, (double dX, double dY, double dSum)> func)
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
                Name = $"{excentricityName} {liverArm}",
                ShortName = $"z,{firstAxisName.ToLower()}",
                Text = unitName,
                Description = $"{excentricityName} {liverArm} along {firstAxisName.ToUpper()}-axis"
            };
            var sumLiverArmY = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{excentricityName} {liverArm}",
                ShortName = $"z,{secondAxisName.ToLower()},sum",
                Text = unitName,
                Description = $"{excentricityName} {liverArm} along {secondAxisName.ToUpper()}-axis"
            };
            var sumLiverArm = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{excentricityName} {liverArm}",
                ShortName = $"z,sum",
                Text = unitName,
                Description = $"{excentricityName} {liverArm}"
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
                sumLiverArmX.Value = (double.NaN).ToString();
                sumLiverArmX.Description += $": {ex}";

                sumLiverArmY.IsValid = false;
                sumLiverArmY.Value = (double.NaN).ToString();
                sumLiverArmY.Description += $": {ex}";

                sumLiverArm.IsValid = false;
                sumLiverArm.Value = (double.NaN).ToString();
                sumLiverArm.Description += $": {ex}";
            }

            parameters.Add(sumLiverArmX);
            parameters.Add(sumLiverArmY);
            parameters.Add(sumLiverArm);
            return parameters;
        }
    }
}
