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

        [DynamoDBProperty(Converter = typeof(DynamoDbEnumConverter<Title>))]
        public Title Title { get; set; }
        public string PreferredFirstname { get; set; }
        public string PreferredSurname { get; set; }
        public string Firstname { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }
        public string Ethnicity { get; set; }
        public string Nationality { get; set; }
        public string PlaceOfBirth { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbDateTimeConverter))]
        public DateTime DateOfBirth { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbEnumConverter<Gender>))]
        public Gender Gender { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbObjectListConverter<Identification>))]
        public List<Identification> Identifications { get; set; } = new List<Identification>();

        [DynamoDBProperty(Converter = typeof(DynamoDbObjectListConverter<Language>))]
        public List<Language> Languages { get; set; } = new List<Language>();

        [DynamoDBProperty(Converter = typeof(DynamoDbEnumListConverter<CommunicationRequirement>))]
        public List<CommunicationRequirement> CommunicationRequirements { get; set; } = new List<CommunicationRequirement>();

        [DynamoDBProperty(Converter = typeof(DynamoDbEnumListConverter<PersonType>))]
        public List<PersonType> PersonTypes { get; set; } = new List<PersonType>();
    }
}
