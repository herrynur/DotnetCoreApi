using BackendService.Application.Cities.Dtos;
using BackendService.Application.Cities.Service;
using BackendService.Domain;
using BackendService.Helper.Api;
using BackendService.Helper.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BackendService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CitiesController(ICitiesService citiesService) : ControllerBase
    {
        /// <summary>
        /// Get All Cities
        /// </summary>
        /// <remarks>
        /// Use PaginationFilter to get paginated data with Parameters PageNumber and PageSize,
        /// example:
        /// 
        ///     {BaseUrl}Cities?PageNumber=1&amp;PageSize=10
        ///     
        /// Use Query to search data with Parameters query,
        /// example:
        /// 
        ///     {BaseUrl}Cities?PageNumber=1&amp;PageSize=10&amp;Query=Jakarta
        ///     
        /// </remarks>
        /// <param name="filter"></param>
        /// <param name="Query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]   
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<CitiesReadDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponse.BadRequest))]
        public async Task<ActionResult<PaginatedList<CitiesReadDto>>> GetCitiesAsync(
            [FromQuery] PaginationFilter filter,
            [FromQuery] string? Query,
            CancellationToken cancellationToken)
        {
            //minimal 3 character
            if (!string.IsNullOrWhiteSpace(Query) && Query.Length < 3)
            {
                return Problem(statusCode: 400, detail: "Enter a minimum of 3 characters");
            }
            //End minimal 3 character

            var result = await citiesService.GetCitiesAsync(filter, Query, cancellationToken);

            return result;
        }

        /// <summary>
        /// Get Single City
        /// </summary>
        /// <remarks>
        /// Use id from cities data in path to get single data, example:
        /// 
        ///     {BaseUrl}Cities/cdf76df8-d15c-4a0b-9c77-ec50dead8dac
        ///     
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CitiesReadDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(DefaultErrorResponse.NotFound))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponse.BadRequest))]
        public async Task<ActionResult<CitiesReadDto>> GetCityAsync(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await citiesService.GetSingleAsync(id, cancellationToken);

            if (result is null)
            {
                return Problem(statusCode: 404, detail: "City not found");
            }

            return result;
        }
    }
}
