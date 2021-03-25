using Amazon.DynamoDBv2.DataModel;
using PersonApi.V1.Domain;
using System;
using System.Collections.Generic;

namespace PersonApi.V1.Infrastructure
{
    [DynamoDBTable("Persons", LowerCamelCaseProperties = true)]
    public class PersonDbEntity
    {
        [DynamoDBHashKey]
        public Guid Id { get; set; }

        [DynamoDBProperty]
        public Title Title { get; set; }
        [DynamoDBProperty]
        public string PreferredFirstname { get; set; }
        [DynamoDBProperty]
        public string PreferredSurname { get; set; }
        [DynamoDBProperty]
        public string Firstname { get; set; }
        [DynamoDBProperty]
        public string MiddleName { get; set; }
        [DynamoDBProperty]
        public string Surname { get; set; }
        [DynamoDBProperty]
        public string Ethinicity { get; set; }
        [DynamoDBProperty]
        public string Nationality { get; set; }
        [DynamoDBProperty]
        public string PlaceOfBirth { get; set; }
        [DynamoDBProperty]
        public DateTime DateOfBirth { get; set; }
        [DynamoDBProperty]
        public Gender Gender { get; set; }
        [DynamoDBProperty]
        public IEnumerable<Identification> Identifications { get; set; }
        [DynamoDBProperty]
        public IEnumerable<Language> Languages { get; set; }
        [DynamoDBProperty]
        public IEnumerable<CommunicationRequirement> CommunicationRequirements { get; set; }
        [DynamoDBProperty]
        public IEnumerable<PersonType> PersonTypes { get; set; }
        [DynamoDBProperty]
        public IEnumerable<ApiLink> Links { get; set; }
    }
}
