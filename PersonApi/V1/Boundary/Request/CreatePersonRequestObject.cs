using System;
using System.Collections.Generic;
using System.Linq;
using PersonApi.V1.Domain;
using PersonApi.V1.Infrastructure;

namespace PersonApi.V1.Boundary.Request
{
    public class CreatePersonRequestObject : PersonRequestObject
    {
        public string Reason { get; set; }

        public override PersonDbEntity ToDatabase()
        {
            var personEntity = base.ToDatabase();

            personEntity.Reason = Reason;

            return personEntity;
        }
    }
}
