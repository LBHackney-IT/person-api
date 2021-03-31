using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace PersonApi.V1.Infrastructure
{
    // TODO: This should go in a common NuGet package...

    /// <summary>
    /// Converter for sub-objects
    /// Treats a custom sub-objects as straight Json, meaning any DynamoDb attributes it may have are not applied
    /// Will (de)serialise any enum properties as the name value (not the numeric value)
    /// </summary>
    public class DynamoDbObjectConverter<T> : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {
            if (null == value) return new DynamoDBNull();

            return Document.FromJson(JsonConvert.SerializeObject(value, new StringEnumConverter()));
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            if ((null == entry) || (null != entry.AsDynamoDBNull())) return null;

            var doc = entry.AsDocument();
            if (null == doc)
                throw new ArgumentException("Field value is not a Document. This attribute has been used on a property that is not a custom object.");

            return JsonConvert.DeserializeObject<T>(doc.ToJson(), new StringEnumConverter());
        }
    }
}
