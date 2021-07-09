using Hackney.Core.Http;
using Hackney.Core.JWT;
using Hackney.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PersonApi.V1.Boundary.Request;
using PersonApi.V1.Boundary.Response;
using PersonApi.V1.UseCase.Interfaces;
using System;
using System.Threading.Tasks;

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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PersonApiController(IGetByIdUseCase getByIdUseCase, IPostNewPersonUseCase newPersonUseCase,
            IUpdatePersonUseCase updatePersonUseCase, ITokenFactory tokenFactory, IHttpContextWrapper contextWrapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _getByIdUseCase = getByIdUseCase;
            _newPersonUseCase = newPersonUseCase;
            _updatePersonUseCase = updatePersonUseCase;
            _tokenFactory = tokenFactory;
            _contextWrapper = contextWrapper;
            _httpContextAccessor = httpContextAccessor;
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
        public async Task<IActionResult> PostNewPerson([FromBody] CreatePersonRequestObject personRequestObject)
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
        public async Task<IActionResult> UpdatePersonByIdAsync([FromBody] UpdatePersonRequestObject personRequestObject,
            [FromRoute] PersonQueryObject query)
        {
            // This is only possible because the EnableRequestBodyRewind middleware is specified in the application startup.
            var bodyText = await _httpContextAccessor.HttpContext.Request.GetRawBodyStringAsync().ConfigureAwait(false);

            var token = _tokenFactory.Create(_contextWrapper.GetContextRequestHeaders(HttpContext));

            // We use a request object AND the raw request body text because the incoming request will only contain the fields that changed
            // whereas the request object has all possible updateable fields defined.
            // The implementation will use the raw body text to identify which fields to update and the request object is specified here so that its
            // associated validation will be executed by the MVC pipeline before we even get to this point.
            var person = await _updatePersonUseCase.ExecuteAsync(personRequestObject, bodyText, token, query).ConfigureAwait(false);
            if (person == null) return NotFound(query.Id);

            return NoContent();
        }
    }
}
