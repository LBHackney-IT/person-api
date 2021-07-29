using System;

namespace PersonApi.V1.Boundary.Response
{
    public class TenureResponseObject
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

        public bool IsActive { get; set; }
    }
}
