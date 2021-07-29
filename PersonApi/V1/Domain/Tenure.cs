using System;
using System.Text;
using System.Text.Json.Serialization;

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

        public string PaymentReference { get; set; }

        public string PropertyReference { get; set; }

        [JsonIgnore]
        public bool IsActive => TenureHelpers.IsTenureActive(EndDate);

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
                && Uprn.Equals(otherObj.Uprn)
                && PaymentReference.Equals(otherObj.PaymentReference)
                && PropertyReference.Equals(otherObj.PropertyReference);
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
                          .Append(PaymentReference)
                          .Append(PropertyReference)
                          .Append(IsActive)
                          .ToString()
                          .GetHashCode();
        }
    }
}
