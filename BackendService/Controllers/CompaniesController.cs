using BackendService.Application.Cities.Dtos;
using BackendService.Application.Companies.Dtos;
using BackendService.Application.Companies.Service;
using BackendService.Helper.Api;
using BackendService.Helper.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace BackendService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CompaniesController(ICompaniesService service) : ControllerBase
    {
        /// <summary>
        /// Get All Companies
        /// </summary>
        /// <remarks>
        /// Use PaginationFilter to get paginated data with Parameters PageNumber and PageSize,
        /// example:
        /// 
        ///     {BaseUrl}Companies?PageNumber=1&amp;PageSize=10
        ///     
        /// Use Query to search data with Parameters query,
        /// example:
        /// 
        ///     {BaseUrl}Companies?PageNumber=1&amp;PageSize=10&amp;Query=mandiri
        ///     
        /// </remarks>
        /// <param name="filter"></param>
        /// <param name="Query"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(PaginatedList<CompaniesReadDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponse.BadRequest))]
        public async Task<ActionResult<PaginatedList<CompaniesReadDto>>> GetCompaniesAsync(
            [FromQuery] PaginationFilter filter,
            String? Query,
            CancellationToken cancellationToken)
        {
            //minimal 3 character
            if (!string.IsNullOrWhiteSpace(Query) && Query.Length < 3)
            {
                return Problem(statusCode: 400, detail: "Enter a minimum of 3 characters");
            }
            //End minimal 3 character
            var result = await service.GetAllAsync(filter, Query, cancellationToken);

            return result;
        }

        /// <summary>
        /// Get Single Company
        /// </summary>
        /// <remarks>
        /// Use id from Companies data in path to get single data using GET Method, example:
        /// 
        ///     {BaseUrl}Companies/cdf76df8-d15c-4a0b-9c77-ec50dead8dac
        ///     
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CompaniesReadDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(DefaultErrorResponse.NotFound))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponse.BadRequest))]
        public async Task<ActionResult<CompaniesReadDto>> GetSingleAsync(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await service.GetSingleAsync(id, cancellationToken);

            if (result is null)
            {
                return Problem(statusCode: (int) HttpStatusCode.NotFound, detail: "Data not found");
            }

            return result;
        }

        /// <summary>
        /// Post Company
        /// </summary>
        /// <remarks>
        /// Sample request Using POST Method:
        /// 
        ///     POST /Companies
        ///     {
        ///         "Name": "PT Perum Percetakan Uang Indonesia",
        ///         "Code": "PPUI",
        ///         "PIC": "Marsha Lenathea",
        ///         "PhoneNumber": "08123456789",
        ///         "Address": "Jl. Perum Percetakan Uang Indonesia No. 1"
        ///     }
        ///     
        ///  If the request is successful, the response will be as follows:
        ///  
        ///     {
        ///         "Id": "cdf76df8-d15c-4a0b-9c77-ec50dead8dac",
        ///         "Name": "PT Perum Percetakan Uang Indonesia",
        ///         "Code": "PPUI",
        ///         "PIC": "Marsha Lenathea",
        ///         "PhoneNumber": "08123456789",
        ///         "Address": "Jl. Perum Percetakan Uang Indonesia No. 1"
        ///     }
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CompaniesReadDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponse.BadRequest))]
        public async Task<ActionResult<CompaniesReadDto>> Post(
            [FromBody] CompaniesWriteDto input,
            CancellationToken cancellationToken)
        {
            var result = await service.Post(input, cancellationToken);

            if (result.IsError)
            {
                return Problem(statusCode: (int) result.StatusCode, detail: result.Message);
            }

            return Ok(result.Data!.First());
        }


        /// <summary>
        /// Put Company
        /// </summary>
        /// <remarks>
        /// Use id from Companies data in path to get single data using PUT Method, example:
        /// 
        ///     {BaseUrl}Companies/cdf76df8-d15c-4a0b-9c77-ec50dead8dac
        /// Body example :
        /// 
        ///     PUT /Companies/cdf76df8-d15c-4a0b-9c77-ec50dead8dac
        ///     {
        ///         "Name": "PT Perum Percetakan Uang Indonesia",
        ///         "Code": "PPUI",
        ///         "PIC": "Marsha Lenathea",
        ///         "PhoneNumber": "08123456789",
        ///         "Address": "Jl. Perum Percetakan Uang Indonesia No. 1"
        ///     }
        ///     
        /// If the request is successful, the response will be as follows:
        ///  
        ///     {
        ///         "Id": "cdf76df8-d15c-4a0b-9c77-ec50dead8dac",
        ///         "Name": "PT Perum Percetakan Uang Indonesia",
        ///         "Code": "PPUI",
        ///         "PIC": "Marsha Lenathea",
        ///         "PhoneNumber": "08123456789",
        ///         "Address": "Jl. Perum Percetakan Uang Indonesia No. 1"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CompaniesReadDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(DefaultErrorResponse.NotFound))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponse.BadRequest))]
        public async Task<ActionResult<CompaniesReadDto>> Put(
            [FromRoute] Guid id,
            [FromBody] CompaniesWriteDto input,
            CancellationToken cancellationToken)
        {
            var result = await service.Put(id, input, cancellationToken);

            if (result.IsError)
            {
                return Problem(statusCode: (int) result.StatusCode, detail: result.Message);
            }

            return Ok(result.Data!.First());
        }


        /// <summary>
        /// Delete Company
        /// </summary>
        /// <remarks>
        /// Use id from Companies data in path to get single data using DELETE Method, example:
        /// 
        ///     {BaseUrl}Companies/cdf76df8-d15c-4a0b-9c77-ec50dead8dac
        ///     
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(DefaultErrorResponse.NotFound))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponse.BadRequest))]
        public async Task<ActionResult> Delete(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await service.Delete(id, cancellationToken);

            if (result.IsError)
            {
                return Problem(statusCode: (int) result.StatusCode, detail: result.Message);
            }

            return Ok();
        }
    }
}
