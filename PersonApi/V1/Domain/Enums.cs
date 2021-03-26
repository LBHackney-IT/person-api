using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace PersonApi.V1.Domain
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Title
    {
        Mr,
        Mrs,
        Miss,
        Mx
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PersonType
    {
        HousingOfficer,
        Resident,
        Tenant
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Gender
    {
        M,
        F
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum IdentificationType
    {
        NI,
        Passport,
        BirthCertificate
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommunicationRequirement
    {
        SignLanguage,
        InterpreterRequired
    }
}
