using Amazon.DynamoDBv2.DataModel;
using System;
using System.Text;

namespace PersonApi.V1.Domain
{
    public class Tenure
    {
        public string AssetFullAddress { get; set; }

        public string AssetId { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public Guid Id { get; set; }

        public string Type { get; set; }

        public string Uprn { get; set; }

        [DynamoDBIgnore]
        public bool IsActive
        {
            get
            {
                if (!string.IsNullOrEmpty(EndDate))
                {
                    var result = DateTime.TryParse(EndDate, out DateTime dt);
                    if (!result)
                        return false;
                }

                return string.IsNullOrEmpty(EndDate) || EndDate == "1900-01-01" ||
                       DateTime.UtcNow <= DateTime.Parse(EndDate);
            }
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj.GetType()) return false;
            var otherObj = (Tenure) obj;
            return AssetFullAddress.Equals(otherObj.AssetFullAddress)
                && AssetId.Equals(otherObj.AssetId)
                && StartDate.Equals(otherObj.StartDate)
                && EndDate.Equals(otherObj.EndDate)
                && Id.Equals(otherObj.Id)
                && Type.Equals(otherObj.Type)
                && Uprn.Equals(otherObj.Uprn);
        }

        public override int GetHashCode()
        {
            StringBuilder builder = new StringBuilder();
            return builder.Append(AssetFullAddress)
                          .Append(AssetId)
                          .Append(StartDate)
                          .Append(EndDate)
                          .Append(Id.ToString())
                          .Append(Type)
                          .Append(Uprn)
                          .Append(IsActive)
                          .ToString()
                          .GetHashCode();
        }
    }
}
