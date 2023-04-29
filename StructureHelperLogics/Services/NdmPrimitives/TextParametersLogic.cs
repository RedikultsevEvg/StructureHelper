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
        public List<ITextParameter> GetTextParameters()
        {
            var parameters = new List<ITextParameter>();
            parameters.AddRange(GetGravityCenter(prefixInitial, ndms));
            parameters.AddRange(GetArea(prefixInitial, ndms));
            parameters.AddRange(GetSecondMomentOfArea(prefixInitial, ndms));
            parameters.AddRange(GetGravityCenter(prefixActual, ndms, strainMatrix));
            parameters.AddRange(GetArea(prefixActual, ndms, strainMatrix));
            parameters.AddRange(GetSecondMomentOfArea(prefixActual, ndms, strainMatrix));
            return parameters;
        }

        private IEnumerable<ITextParameter> GetSecondMomentOfArea(string prefix, IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix = null)
        {
            const string name = "Moment of inertia";
            var parameters = new List<ITextParameter>();
            var unitArea = CommonOperation.GetUnit(UnitTypes.Area, "mm2");
            var unitStress = CommonOperation.GetUnit(UnitTypes.Stress, "MPa");
            var unitName = $"{unitStress.Name} * {unitArea.Name} * {unitArea.Name}";
            var unitMultiPlayer = unitArea.Multiplyer * unitArea.Multiplyer * unitStress.Multiplyer;
            var firstParameter = new TextParameter()
            {
                IsValid = true,
                Name = $"{prefix} {name} {firstAxisName.ToUpper()}",
                ShortName = $"I{firstAxisName}",
                MeasurementUnit = unitName,
                Description = $"{prefix} {name} of cross-section arbitrary {firstAxisName}-axis multiplied by {prefix} modulus"
            };
            var secondParameter = new TextParameter()
            {
                IsValid = true,
                Name = $"{prefix} {name} {secondAxisName}",
                ShortName = $"I{secondAxisName}",
                MeasurementUnit = unitName,
                Description = $"{prefix} {name} of cross-section arbitrary {secondAxisName}-axis multiplied by {prefix} modulus"
            };
            try
            {
                var gravityCenter = GeometryOperations.GetReducedMomentsOfInertia(locNdms, locStrainMatrix);
                firstParameter.Value = gravityCenter.MomentX * unitMultiPlayer;
                secondParameter.Value = gravityCenter.MomentY * unitMultiPlayer;
            }
            catch (Exception ex)
            {
                firstParameter.IsValid = false;
                firstParameter.Value = double.NaN;
                firstParameter.Description += $": {ex}";
                secondParameter.IsValid = false;
                secondParameter.Value = double.NaN;
                secondParameter.Description += $": {ex}";
            }
            parameters.Add(firstParameter);
            parameters.Add(secondParameter);
            return parameters;
        }

        private IEnumerable<ITextParameter> GetArea(string prefix, IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix = null)
        {
            var parameters = new List<ITextParameter>();
            var unitArea = CommonOperation.GetUnit(UnitTypes.Area, "mm2");
            var unitStress = CommonOperation.GetUnit(UnitTypes.Stress, "MPa");
            var unitName = $"{unitStress.Name} * {unitArea.Name}" ;
            var unitMultiPlayer = unitArea.Multiplyer * unitStress.Multiplyer;
            var firstParameter = new TextParameter()
            {
                IsValid = true,
                Name = $"{prefix} Area",
                ShortName = "EA",
                MeasurementUnit = unitName,
                Description = $"{prefix} Area of cross-section multiplied by {prefix} modulus"
            };
            try
            {
                firstParameter.Value = GeometryOperations.GetReducedArea(locNdms, locStrainMatrix) * unitMultiPlayer;
            }
            catch (Exception ex)
            {
                firstParameter.IsValid = false;
                firstParameter.Value = double.NaN;
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

        private IEnumerable<ITextParameter> GetGravityCenter(string prefix, IEnumerable<INdm> locNdms, IStrainMatrix? locStrainMatrix = null)
        {
            var parameters = new List<ITextParameter>();
            var unitType = UnitTypes.Length;
            var unit = CommonOperation.GetUnit(unitType, "mm");
            var unitName = unit.Name;
            var unitMultiPlayer = unit.Multiplyer;
            var firstParameter = new TextParameter()
            {
                IsValid = true,
                Name = $"{prefix} Center{firstAxisName.ToUpper()}",
                ShortName = $"{firstAxisName.ToUpper()}c",
                MeasurementUnit = unitName,
                Description = $"{prefix} Displacement of center of gravity of cross-section along {firstAxisName}-axis"
            };
            var secondParameter = new TextParameter()
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
                firstParameter.Value = gravityCenter.CenterX * unitMultiPlayer;
                secondParameter.Value = gravityCenter.CenterY * unitMultiPlayer;
            }
            catch (Exception ex)
            {
                firstParameter.IsValid = false;
                firstParameter.Value = double.NaN;
                firstParameter.Description += $": {ex}";
                secondParameter.IsValid = false;
                secondParameter.Value = double.NaN;
                secondParameter.Description += $": {ex}";
            }
            parameters.Add(firstParameter);
            parameters.Add(secondParameter);
            return parameters;
        }
    }
}
