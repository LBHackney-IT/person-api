using System;

namespace PersonApi.V1.Domain
{
    public static class TenureHelpers
    {
        private static readonly DateTime _defaultEndDate = DateTime.Parse("1900-01-01");

        public static bool IsTenureActive(string endDate)
        {
            if (string.IsNullOrEmpty(endDate) || !DateTime.TryParse(endDate, out DateTime dtEndDate))
                return true;

            return IsTenureActive(dtEndDate);
        }

        public static bool IsTenureActive(DateTime? endDate)
        {
            return !endDate.HasValue
                || endDate.Value.Date == _defaultEndDate.Date
                || DateTime.UtcNow <= endDate.Value;
        }
    }
}
