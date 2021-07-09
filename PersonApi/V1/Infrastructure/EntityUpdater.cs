using Hackney.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PersonApi.V1.Infrastructure
{
    // TODO - move to common nuget package

    /// <summary>
    /// Interface describing generic methods for updating an instance of an entity from the suypplied request object and raw request json
    /// </summary>
    public interface IEntityUpdater
    {
        /// <summary>
        /// Updates the supplied entity with the updated property values described in the request object / json
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <typeparam name="TUpdateObject">The type of the update request object</typeparam>
        /// <param name="entityToUpdate">The entity to update</param>
        /// <param name="updateJson">The raw update request json from which the request object was deserialized</param>
        /// <param name="updateObject">The update request object</param>
        /// <returns>A response object</returns>
        UpdateEntityResult<TEntity> UpdateEntity<TEntity, TUpdateObject>(
            TEntity entityToUpdate,
            string updateJson,
            TUpdateObject updateObject)
                where TEntity : class
                where TUpdateObject : class;

        /// <summary>
        /// Updates the supplied entity with the updated property values described in the request object / json.
        /// * This method expects both a request object and the raw request json so that the appropriate request object validation
        /// can be executed by the MVC pipeline.
        /// * The raw request json should contain ONLY the properties to be updated.
        /// * The property names in the json / request object MUST MATCH the corresponing properties on the entity type (assuming the json uses camel casing).
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <typeparam name="TUpdateObject">The type of the update request object</typeparam>
        /// <param name="entityToUpdate">The entity to update</param>
        /// <param name="updateJson">The raw update request json from which the request object was deserialized</param>
        /// <param name="updateObject">The update request object</param>
        /// <param name="ignoreUnchangedProperties">Wehter or not to ignore property values set in the update request
        /// but that are actually the same as current entity value.</param>
        /// <returns>A response object</returns>
        UpdateEntityResult<TEntity> UpdateEntity<TEntity, TUpdateObject>(
            TEntity entityToUpdate,
            string updateJson,
            TUpdateObject updateObject,
            bool ignoreUnchangedProperties)
                where TEntity : class
                where TUpdateObject : class;
    }

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

    public class EntityUpdater : IEntityUpdater
    {
        private static JsonSerializerOptions CreateJsonOptions()
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return options;
        }

        /// <summary>
        /// Updates the supplied entity with the updated property values described in the request object / json
        /// Defaults the ignoreUnchangedProperties input value to true.
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <typeparam name="TUpdateObject">The type of the update request object</typeparam>
        /// <param name="entityToUpdate">The entity to update</param>
        /// <param name="updateJson">The raw update request json from which the request object was deserialized</param>
        /// <param name="updateObject">The update request object</param>
        /// <returns>A response object</returns>
        public UpdateEntityResult<TEntity> UpdateEntity<TEntity, TUpdateObject>(TEntity entityToUpdate,
            string updateJson,
            TUpdateObject updateObject)
                where TEntity : class
                where TUpdateObject : class
        {
            return UpdateEntity(entityToUpdate, updateJson, updateObject, true);
        }

        /// <summary>
        /// Updates the supplied entity with the updated property values described in the request object / json.
        /// * This method expects both a request object and the raw request json so that the appropriate request object validation
        /// can be executed by the MVC pipeline.
        /// * The inclusion of the request object also means that each updated property value has been deserialised correctly.
        /// * The raw request json should contain ONLY the properties to be updated.
        /// * The property names in the json / request object MUST MATCH the corresponing properties on the entity type (assuming the json uses camel casing).
        /// * For nested objects, those classes must override the Equals() mewthod so that the algorithm will correctly determine if a suboject has changed.
        /// </summary>
        /// <typeparam name="TEntity">The entity type</typeparam>
        /// <typeparam name="TUpdateObject">The type of the update request object</typeparam>
        /// <param name="entityToUpdate">The entity to update</param>
        /// <param name="updateJson">The raw update request json from which the request object was deserialized</param>
        /// <param name="updateObject">The update request object</param>
        /// <param name="ignoreUnchangedProperties">Whether or not to ignore property values set in the update request
        /// but that are actually the same as current entity value.</param>
        /// <returns>A response object</returns>
        public UpdateEntityResult<TEntity> UpdateEntity<TEntity, TUpdateObject>(TEntity entityToUpdate,
            string updateJson,
            TUpdateObject updateObject,
            bool ignoreUnchangedProperties)
            where TEntity : class
            where TUpdateObject : class
        {
            if (entityToUpdate is null) throw new ArgumentNullException(nameof(entityToUpdate));
            if (updateObject is null) throw new ArgumentNullException(nameof(updateObject));

            var result = new UpdateEntityResult<TEntity>() { UpdatedEntity = entityToUpdate };
            if (string.IsNullOrEmpty(updateJson)) return result;

            var updateDic = JsonSerializer.Deserialize<Dictionary<string, object>>(updateJson, CreateJsonOptions());
            var entityType = typeof(TEntity);
            var updateObjectType = typeof(TUpdateObject);

            var allEntityProperties = entityType.GetProperties();
            foreach (var propName in updateDic.Keys)
            {
                var prop = allEntityProperties.FirstOrDefault(x => x.Name.ToCamelCase() == propName);
                if (prop is null)
                    throw new ArgumentException($"Entity object (type: {entityType.Name}) does not contain a property called {propName}.");

                var requestObjectProperty = updateObjectType.GetProperty(prop.Name);
                if (requestObjectProperty is null)
                    throw new ArgumentException($"Request object (type: {updateObjectType.Name}) does not contain a property called {prop.Name} that is on the entity type ({entityType.Name}).");

                var updateValue = requestObjectProperty.GetValue(updateObject);
                var existingValue = prop.GetValue(entityToUpdate);

                // For sub-objects this Equals() check will only work if the Equals() method is overridden
                if (!ignoreUnchangedProperties || !updateValue.Equals(existingValue))
                {
                    result.OldValues.Add(propName, existingValue);
                    result.NewValues.Add(propName, updateValue);
                    prop.SetValue(entityToUpdate, updateValue);
                }
            }

            return result;
        }
    }

}
