using System.Text;

namespace PersonApi.V1.Domain
{
    public class Identification
    {
        public IdentificationType IdentificationType { get; set; }
        public string Value { get; set; }
        public bool IsOriginalDocumentSeen { get; set; }
        public string LinkToDocument { get; set; }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType()) return false;
            var otherObj = (Identification) obj;
            return IdentificationType.Equals(otherObj.IdentificationType)
                && Value.Equals(otherObj.Value)
                && IsOriginalDocumentSeen.Equals(otherObj.IsOriginalDocumentSeen)
                && LinkToDocument.Equals(otherObj.LinkToDocument);
        }

        public override int GetHashCode()
        {
            StringBuilder builder = new StringBuilder();
            return builder.Append(IdentificationType.ToString())
                          .Append(Value)
                          .Append(IsOriginalDocumentSeen.ToString())
                          .Append(LinkToDocument)
                          .ToString()
                          .GetHashCode();
        }
    }
}
