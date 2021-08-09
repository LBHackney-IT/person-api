namespace PersonApi.V1.Boundary.Request.Validation
{
    public static class ErrorCodes
    {
        public const string DoBInvalid = "W9";
        public const string DoBInFuture = "W10";
        public const string PersonTypeMandatory = "W14";
        public const string FirstNameMandatory = "W15";
        public const string SurnameMandatory = "W16";
        public const string ReasonMandatory = "W17";
        public const string XssCheckFailure = "W42";
    }
}
