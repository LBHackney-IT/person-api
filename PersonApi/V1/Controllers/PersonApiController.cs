using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.Logging;
using PersonApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Infrastructure;
using PersonApi.V1.Infrastructure.JWT;

namespace PersonApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/persons")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class PersonApiController : BaseController
    {
        private readonly IGetByIdUseCase _getByIdUseCase;
        private readonly IPostNewPersonUseCase _newPersonUseCase;
        private readonly ITokenFactory _tokenFactory;
        private readonly IHttpContextWrapper _contextWrapper;

        public PersonApiController(IGetByIdUseCase getByIdUseCase, IPostNewPersonUseCase newPersonUseCase,
            ITokenFactory tokenFactory, IHttpContextWrapper contextWrapper)
        {
            _getByIdUseCase = getByIdUseCase;
            _newPersonUseCase = newPersonUseCase;
            _tokenFactory = tokenFactory;
            _contextWrapper = contextWrapper;
        }

        /// <summary>
        /// Retrives the person record corresponding to the supplied id
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid id value supplied</response>
        /// <response code="404">No person found for the specified id</response>
        /// <response code="500">Something went wrong</response>
        [ProducesResponseType(typeof(PersonResponseObject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Route("{id}")]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> GetPersonByIdAsync(Guid id)
        {
            var person = await _getByIdUseCase.ExecuteAsync(id).ConfigureAwait(false);
            if (null == person) return NotFound(id);

            return Ok(person);
        }

        [ProducesResponseType(typeof(PersonResponseObject), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> PostNewPerson([FromBody] PersonRequestObject personRequestObject)
        {
            var token = _tokenFactory.Create(_contextWrapper.GetContextRequestHeaders(HttpContext));

            var person = await _newPersonUseCase.ExecuteAsync(personRequestObject, token)
                .ConfigureAwait(false);

            return Created(new Uri("http://api/v1/persons"), person);
        }
    }
}
