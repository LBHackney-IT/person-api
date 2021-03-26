namespace PersonApi.V1.Domain
{
    public class Identification
    {
        public IdentificationType IdentificationType { get; set; }
        public string Value { get; set; }
        public bool IsOriginalDocumentSeen { get; set; }
        public string LinkToDocument { get; set; }
    }
}
