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
        public static string VisualPropertyIsNotRight => "#0010: VisualPropertyIsNotRight";
        public static string FactorMustBeGraterThanZero => "#0011: Partial factor must not be less than zero";
        public static string LongitudinalForceMustBeLessThanZero => "#0012: Longitudinal force must be less than zero";
        public static string LongitudinalForceMustBeLessThanCriticalForce => "#0013: Absolute value of longitudinal force must be greater than critical force";
        public static string SizeMustBeGreaterThanZero => "#0014: Size must be greater than zero";
    }
}
