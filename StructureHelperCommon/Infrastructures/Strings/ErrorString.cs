using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Strings
{
    public static class ErrorStrings
    {
        public static string UnknownError => "#0000: Unknown error";
        public static string ObjectTypeIsUnknown => "#0001: Object type is unknown";
        public static string MaterialTypeIsUnknown => "#0002: Material type is unknown";
        public static string DataIsInCorrect => "#0003: Data is not correct";
        public static string ShapeIsNotCorrect => "#0004: Shape is not valid";
        public static string LimitStatesIsNotValid => "#0005: Type of limite state is not valid";
        public static string LoadTermIsNotValid => "#0006: Load term is not valid";
        public static string IncorrectValue => "#0007: value is not valid";
        public static string FileCantBeDeleted => "#0008: File can't be deleted";
        public static string FileCantBeSaved => "#0009: File can't be saved";
    }
}
