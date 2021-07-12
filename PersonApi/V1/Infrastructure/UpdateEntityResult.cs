using System.Collections.Generic;

namespace PersonApi.V1.Infrastructure
{
    // TODO - move to common nuget package

    /// <summary>
    /// Class representing the response to the EntityUpdater.UpdateEntity() method.
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    public class UpdateEntityResult<T> where T : class
    {
        /// <summary>
        /// The updated entity
        /// </summary>
        public T UpdatedEntity { get; set; }

        /// <summary>
        /// A simple dictionary listing the previous value(s) of any entity properties actually updated by the call.
        /// The size and keys of this dictionary will match that of the NewValues property.
        /// </summary>
        public Dictionary<string, object> OldValues { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// A simple dictionary listing the new value(s) of any entity properties actually updated by the call.
        /// The size and keys of this dictionary will match that of the OldValues property.
        /// </summary>
        public Dictionary<string, object> NewValues { get; set; } = new Dictionary<string, object>();
    }
}
