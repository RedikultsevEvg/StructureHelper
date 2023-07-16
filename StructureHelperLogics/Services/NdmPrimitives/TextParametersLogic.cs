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
        static string firstAxisName => ProgramSetting.CrossSectionAxisNames.FirstAxis;
        static string secondAxisName => ProgramSetting.CrossSectionAxisNames.SecondAxis;
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
            return parameters;
        }
        private IEnumerable<IValueParameter<string>> GetSimpleArea(IEnumerable<INdm> ndms)
        {
            const string name = "Summary Area";
            const string shortName = "A";
            var parameters = new List<IValueParameter<string>>();
            var unitArea = CommonOperation.GetUnit(UnitTypes.Area, "mm2");
            var unitName = $"{unitArea.Name}";
            var unitMultiPlayer = unitArea.Multiplyer;
            var firstParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{name}",
                ShortName = $"{shortName}",
                MeasurementUnit = unitName,
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
            var unitArea = CommonOperation.GetUnit(UnitTypes.Area, "mm2");
            var unitStress = CommonOperation.GetUnit(UnitTypes.Stress, "MPa");
            var unitName = $"{unitStress.Name} * {unitArea.Name} * {unitArea.Name}";
            var unitMultiPlayer = unitArea.Multiplyer * unitArea.Multiplyer * unitStress.Multiplyer;
            var firstParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefix} {name} {firstAxisName.ToUpper()}",
                ShortName = $"{shortName}{firstAxisName}",
                MeasurementUnit = unitName,
                Description = $"{prefix} {name} of cross-section arbitrary {firstAxisName}-axis multiplied by {prefix} modulus"
            };
            var secondParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefix} {name} {secondAxisName}",
                ShortName = $"{shortName}{secondAxisName}",
                MeasurementUnit = unitName,
                Description = $"{prefix} {name} of cross-section arbitrary {secondAxisName}-axis multiplied by {prefix} modulus"
            };
            try
            {
                var gravityCenter = GeometryOperations.GetReducedMomentsOfInertia(locNdms, locStrainMatrix);
                firstParameter.Value = (gravityCenter.MomentX * unitMultiPlayer).ToString();
                secondParameter.Value = (gravityCenter.MomentY * unitMultiPlayer).ToString();
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
                MeasurementUnit = "-",
                Description = $"{prefixActual}/{prefixInitial} {name} of cross-section arbitrary {firstAxisName}-axis ratio"
            };
            var secondParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefixActual}/{prefixInitial} {name} {secondAxisName} ratio",
                ShortName = $"{shortName}{secondAxisName}-ratio",
                MeasurementUnit = "-",
                Description = $"{prefixActual}/{prefixInitial} {name} of cross-section arbitrary {secondAxisName}-axis ratio"
            };
            try
            {
                var initialMoments = GeometryOperations.GetReducedMomentsOfInertia(locNdms);
                var actualMoments = GeometryOperations.GetReducedMomentsOfInertia(locNdms, locStrainMatrix);
                firstParameter.Value = (actualMoments.MomentX / initialMoments.MomentX).ToString();
                secondParameter.Value = (actualMoments.MomentY / initialMoments.MomentY).ToString();
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
            var unitArea = CommonOperation.GetUnit(UnitTypes.Area, "mm2");
            var unitStress = CommonOperation.GetUnit(UnitTypes.Stress, "MPa");
            var unitName = $"{unitStress.Name} * {unitArea.Name}" ;
            var unitMultiPlayer = unitArea.Multiplyer * unitStress.Multiplyer;
            var firstParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefix} {name}",
                ShortName = $"{shortName}",
                MeasurementUnit = unitName,
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
                MeasurementUnit = "-",
                Description = $"{prefixActual}/{prefixInitial} {name}-ratio of cross-section"
            };
            try
            {
                var actual = GeometryOperations.GetReducedArea(locNdms, locStrainMatrix);
                var initial = GeometryOperations.GetReducedArea(locNdms);
                firstParameter.Value = (actual / initial).ToString();
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
            var unit = CommonOperation.GetUnit(unitType, "mm");
            var unitName = unit.Name;
            var unitMultiPlayer = unit.Multiplyer;
            var firstParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefix} Center{firstAxisName.ToUpper()}",
                ShortName = $"{firstAxisName.ToUpper()}c",
                MeasurementUnit = unitName,
                Description = $"{prefix} Displacement of center of gravity of cross-section along {firstAxisName}-axis"
            };
            var secondParameter = new ValueParameter<string>()
            {
                IsValid = true,
                Name = $"{prefix} Center{secondAxisName.ToUpper()}",
                ShortName = $"{secondAxisName.ToUpper()}c",
                MeasurementUnit = unitName,
                Description = $"{prefix} Displacement of center of gravity of cross-section along {secondAxisName}-axis"
            };
            try
            {
                var gravityCenter = GeometryOperations.GetGravityCenter(locNdms, locStrainMatrix);
                firstParameter.Value = (gravityCenter.CenterX * unitMultiPlayer).ToString();
                secondParameter.Value = (gravityCenter.CenterY * unitMultiPlayer).ToString();
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
    }
}
