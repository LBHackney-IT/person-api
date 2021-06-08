using System.Text.Json.Serialization;

namespace PersonApi.V1.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Title
    {
        Dr,
        Master,
        Miss,
        Mr,
        Mrs,
        Ms,
        Other,
        Rabbi,
        Reverend
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum PersonType
    {
        Tenant,
        HouseholdMember
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Gender
    {
        M,  // Male
        F,  // Female
        O   // Other?
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum IdentificationType
    {
        Passport,
        DrivingLicence
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CommunicationRequirement
    {
        SignLanguage,
        InterpreterRequired
    }
}
