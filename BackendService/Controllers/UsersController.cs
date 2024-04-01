using BackendService.Application.MsUsers.Dtos;
using BackendService.Application.MsUsers.Service;
using BackendService.Helper.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BackendService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class UsersController(IMsUserService services) : ControllerBase
    {
        /// <summary>
        /// Get All Users
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<MsUserReadDto>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponse.BadRequest))]
        public async Task<List<MsUserReadDto>> GetMsUsersAsync(
            CancellationToken cancellationToken)
        {
            var result = await services.GetAllAsync(cancellationToken);

            return result;
        }

        /// <summary>
        /// Get Single User
        /// </summary>
        /// <remarks>
        /// Use id from Users data in path to get single data, example:
        /// 
        ///     {BaseUrl}Users/cdf76df8-d15c-4a0b-9c77-ec50dead8dac
        /// 
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(MsUserReadDto))]
        [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(DefaultErrorResponse.NotFound))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponse.BadRequest))]
        public async Task<MsUserReadDto> GetMsUserAsync(
            [FromRoute] Guid id, 
            CancellationToken cancellationToken)
        {
            var result = await services.GetSingleAsync(id, cancellationToken);

            return result;
        }
    }
}
