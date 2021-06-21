using System;
using Amazon.DynamoDBv2.Model;

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

        public bool IsActive
        {
            get
            {
                if (!string.IsNullOrEmpty(EndDate))
                {
                    var result = DateTime.TryParse(EndDate, out DateTime dt);
                    if (result == false)
                        return false;
                }

                return string.IsNullOrEmpty(EndDate) || EndDate == "1900-01-01" ||
                       DateTime.UtcNow <= DateTime.Parse(EndDate);
            }
        }
    }
}
