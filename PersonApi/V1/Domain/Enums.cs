using System.Text.Json.Serialization;

namespace PersonApi.V1.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Title
    {
        Mr,
        Mrs,
        Miss,
        Mx
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PersonType
    {
        HousingOfficer,
        Resident,
        Tenant
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        M,
        F
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum IdentificationType
    {
        NI,
        Passport,
        BirthCertificate
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CommunicationRequirement
    {
        SignLanguage,
        InterpreterRequired
    }
}
