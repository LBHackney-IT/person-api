using Hackney.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;
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
        private readonly IUpdatePersonUseCase _updatePersonUseCase;

        public PersonApiController(IGetByIdUseCase getByIdUseCase, IPostNewPersonUseCase newPersonUseCase,
            IUpdatePersonUseCase updatePersonUseCase, ITokenFactory tokenFactory, IHttpContextWrapper contextWrapper)
        {
            _getByIdUseCase = getByIdUseCase;
            _newPersonUseCase = newPersonUseCase;
            _updatePersonUseCase = updatePersonUseCase;
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
        public async Task<IActionResult> GetPersonByIdAsync([FromRoute] PersonQueryObject query)
        {
            var person = await _getByIdUseCase.ExecuteAsync(query).ConfigureAwait(false);
            if (null == person) return NotFound(query.Id);

            return Ok(person);
        }

        [ProducesResponseType(typeof(PersonResponseObject), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> PostNewPerson([FromBody] PersonRequestObject personRequestObject)
        {
            var token = _tokenFactory.Create(_contextWrapper.GetContextRequestHeaders(HttpContext));

            var person = await _newPersonUseCase.ExecuteAsync(personRequestObject, token)
                .ConfigureAwait(false);

            return Created(new Uri($"api/v1/persons/{person.Id}", UriKind.Relative), person);
        }

        /// <summary>
        /// Updates the person record corresponding to the supplied id
        /// </summary>
        /// <response code="204">Successfully updated person record</response>
        /// <response code="400">Invalid id value supplied</response>
        /// <response code="404">No person found for the specified id</response>
        /// <response code="500">Something went wrong</response>
        [ProducesResponseType(typeof(PersonResponseObject), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPatch]
        [Route("{id}")]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> UpdatePersonByIdAsync([FromBody] PersonRequestObject personRequestObject)
        {
            var person = await _updatePersonUseCase.ExecuteAsync(personRequestObject).ConfigureAwait(false);
            if (person == null) return NotFound(personRequestObject.Id);

            return NoContent();
        }
    }
}
