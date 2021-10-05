using Hackney.Core.Http;
using Hackney.Core.JWT;
using Hackney.Core.Logging;
using Hackney.Shared.Person.Boundary.Request;
using Hackney.Shared.Person.Boundary.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonApi.V1.Controllers;
using PersonApi.V2.UseCase.Interfaces;
using System;
using System.Threading.Tasks;

namespace PersonApi.V2.Controllers
{
    [ApiController]
    [Route("api/v2/persons")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class PersonApiController : BaseController
    {
        private readonly IPostNewPersonUseCase _newPersonUseCase;
        private readonly ITokenFactory _tokenFactory;
        private readonly IHttpContextWrapper _contextWrapper;

        public PersonApiController(IPostNewPersonUseCase newPersonUseCase,
            ITokenFactory tokenFactory, IHttpContextWrapper contextWrapper)
        {
            _newPersonUseCase = newPersonUseCase;
            _tokenFactory = tokenFactory;
            _contextWrapper = contextWrapper;
        }

        [ProducesResponseType(typeof(PersonResponseObject), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> PostNewPerson([FromBody] CreatePersonRequestObject personRequestObject)
        {
            var token = _tokenFactory.Create(_contextWrapper.GetContextRequestHeaders(HttpContext));

            var person = await _newPersonUseCase.ExecuteAsync(personRequestObject, token)
                .ConfigureAwait(false);

            return Created(new Uri($"api/v2/persons/{person.Id}", UriKind.Relative), person);
        }
    }
}
