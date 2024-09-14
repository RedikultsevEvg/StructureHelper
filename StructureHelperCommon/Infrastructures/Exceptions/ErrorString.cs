﻿namespace StructureHelperCommon.Infrastructures.Exceptions
{
    public static class ErrorStrings
    {
        public static string UnknownError => "#0000: Unknown error";
        public static string ObjectTypeIsUnknown => "#0001: Object type is unknown";
        public static string ObjectTypeIsUnknownObj(object obj) => $"{ObjectTypeIsUnknown}: {obj.GetType()}";
        public static string MaterialTypeIsUnknown => "#0002: Material type is unknown";
        public static string DataIsInCorrect => "#0003: Data is not correct";
        public static string ShapeIsNotCorrect => "#0004: Shape is not valid";
        public static string LimitStatesIsNotValid => "#0005: Type of limite state is not valid";
        public static string LoadTermIsNotValid => "#0006: Load term is not valid";
        public static string IncorrectValue => "#0007: Value is not valid";
        public static string FileCantBeDeleted => "#0008: File can't be deleted";
        public static string FileCantBeSaved => "#0009: File can't be saved";
        public static string VisualPropertyIsNotRight => "#0010: VisualPropertyIsNotRight";
        public static string FactorMustBeGraterThanZero => "#0011: Partial factor must not be less than zero";
        public static string LongitudinalForceMustBeLessThanZero => "#0012: Longitudinal force must be less than zero";
        public static string LongitudinalForceMustBeLessThanCriticalForce => "#0013: Absolute value of longitudinal force must be greater than critical force";
        public static string SizeMustBeGreaterThanZero => "#0014: Size must be greater than zero";
        public static string ParameterIsNull => "#0015: Parameter is null";
        public static string ResultIsNotValid => "#0016: Result is not valid";
        public static string ErrorOfExuting => "#0017: Error of executing";
        public static string ExpectedWas(System.Type expected, System.Type was) => $"{DataIsInCorrect}: Expected {expected}, but was {was}";
        public static string ExpectedWas(System.Type expected, object obj) => ExpectedWas(expected, obj.GetType());
        public static string NullReference => "#0018: Null reference";
        public static string ObjectNotFound => "#0018: Object not found";
        public static string ErrorDuring(string operation) => string.Format("Errors appeared during {0}, see detailed information", operation);
        public static string CalculationError => "#0019: Error of calculation";
        public static string SourceObject => "#0020: Source object";
        public static string TargetObject => "#0021: Target object";
    }
}
