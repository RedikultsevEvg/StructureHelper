using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StructureHelperCommon.Services
{
    /// <summary>
    /// Processor for checking objects
    /// </summary>
    public static class CheckObject
    {
        /// <summary>
        /// Compares types of two objects, throws exception if result is not valid
        /// </summary>
        /// <param name="targetObject"></param>
        /// <param name="sourceObject"></param>
        /// <exception cref="StructureHelperException"></exception>
        public static void CompareTypes(object targetObject, object sourceObject)
        {
            IsNull(targetObject, "target object");
            IsNull(targetObject, "source object");
            if (targetObject.GetType() != sourceObject.GetType())
            {
                throw new StructureHelperException
                    ($"{ErrorStrings.DataIsInCorrect}: target type is {targetObject.GetType()},\n does not coinside with source type {sourceObject.GetType()}");
            }
        }
        public static void IsNull(object item, string message = "")
        {
            if (item is null) { throw new StructureHelperException(ErrorStrings.ParameterIsNull + message); }
        }

        public static void CheckType(object sourceObject, Type targetType)
        {
            if (sourceObject.GetType() != targetType)
            {
                throw new StructureHelperException($"{ErrorStrings.ExpectedWas(targetType, sourceObject.GetType())}");
            }
        }

        public static void CheckMinMax (double value, double minValue, double maxValue)
        {
            if (value == null || minValue == null || maxValue == null) { throw new StructureHelperException(ErrorStrings.NullReference); }
            if (value < minValue) { throw new StructureHelperException($"{ErrorStrings.IncorrectValue}: Value must be greater than {minValue}"); }
            if (value > maxValue) { throw new StructureHelperException($"{ErrorStrings.IncorrectValue}: Value must be less than {maxValue}"); }
        }
    }
}
