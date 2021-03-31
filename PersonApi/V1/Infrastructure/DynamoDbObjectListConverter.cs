using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonApi.V1.Infrastructure
{
    // TODO: This should go in a common NuGet package...

    /// <summary>
    /// Converter for enum lists where the value stored should be the enum value name (not the numeric value)
    /// </summary>
    public class DynamoDbObjectListConverter<T> : IPropertyConverter
    {
        public DynamoDBEntry ToEntry(object value)
        {
            if (null == value) return new DynamoDBNull();

            var list = value as IEnumerable<T>;
            if (null == list)
                throw new ArgumentException($"Field value is not a list of {typeof(T).Name}. This attribute has been used on a property that is not a list of custom objects.");

            return new DynamoDBList(list.Select(x => Document.FromJson(JsonConvert.SerializeObject(x, new StringEnumConverter()))));
        }

        public object FromEntry(DynamoDBEntry entry)
        {
            if ((null == entry) || (null != entry.AsDynamoDBNull())) return null;

            var list = entry.AsDynamoDBList();
            if (null == list)
                throw new ArgumentException("Field value is not a DynamoDBList. This attribute has been used on a property that is not a list of custom objects.");

            return list.AsListOfDocument().Select(x => JsonConvert.DeserializeObject<T>(x.ToJson(), new StringEnumConverter())).ToList();
        }
    }
}
