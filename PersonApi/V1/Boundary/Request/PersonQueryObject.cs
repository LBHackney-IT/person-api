using Microsoft.AspNetCore.Mvc;
using System;

namespace PersonApi.V1.Boundary.Request
{
    public class PersonQueryObject
    {
        [FromRoute(Name = "id")]
        public Guid Id { get; set; }
    }
}
