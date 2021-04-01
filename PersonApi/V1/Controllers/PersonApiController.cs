using PersonApi.V1.Boundary.Response;
using PersonApi.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public PersonApiController(IGetByIdUseCase getByIdUseCase)
        {
            _getByIdUseCase = getByIdUseCase;
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
        public async Task<IActionResult> GetPersonByIdAsync(Guid id)
        {
            var person = await _getByIdUseCase.ExecuteAsync(id).ConfigureAwait(false);
            if (null == person) return NotFound(id);

            return Ok(person);
        }
    }
}
