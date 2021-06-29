using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Infrastructure.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PersonApi.V1.UseCase.Interfaces
{
    public interface IUpdatePersonUseCase
    {
        Task<PersonResponseObject> ExecuteAsync(UpdatePersonRequestObject personRequestObject);
    }
}
