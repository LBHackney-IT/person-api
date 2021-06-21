using System;

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

        public bool IsActive => string.IsNullOrEmpty(EndDate) || EndDate == "1900-01-01" ||
                                DateTime.UtcNow <= DateTime.Parse(EndDate);
    }
}
