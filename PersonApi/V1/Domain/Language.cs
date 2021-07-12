using System.Text;
using System.Text.Json.Serialization;

namespace PersonApi.V1.Domain
{
    public class Language
    {
        [JsonPropertyName("language")]
        public string Name { get; set; }
        public bool IsPrimary { get; set; }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType()) return false;
            var otherObj = (Language) obj;
            return Name.Equals(otherObj.Name)
                && IsPrimary.Equals(otherObj.IsPrimary);
        }

        public override int GetHashCode()
        {
            StringBuilder builder = new StringBuilder();
            return builder.Append(Name)
                          .Append(IsPrimary.ToString())
                          .ToString()
                          .GetHashCode();
        }
    }
}
