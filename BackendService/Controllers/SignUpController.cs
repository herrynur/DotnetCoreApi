using BackendService.Application.Login.Dtos;
using BackendService.Application.SignUp.Dtos;
using BackendService.Application.SignUp.Service;
using BackendService.Domain;
using BackendService.Helper.Model;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BackendService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class SignUpController(ISignUpService service) : ControllerBase
    {
        /// <summary>
        /// Register new user
        /// </summary>
        /// <remarks>
        /// Register new user with username, password and fullname.
        /// 
        /// example request:
        /// 
        ///     POST /SignUp
        ///     {
        ///         "Fullname": "Marsha Lenathea",
        ///         "Username": "marsha",
        ///         "Password": "password123"
        ///     }
        ///     
        /// if Register success, the response will be as follows:
        ///     
        ///     {
        ///         "Id": "cdf76df8-d15c-4a0b-9c77-ec50dead8dac",
        ///         "Fullname": "Marsha Lenathea",
        ///         "Username": "marsha"
        ///     }
        ///     
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(SignUpReadDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponse.BadRequest))]
        public async Task<ActionResult<SignUpReadDto>> SignUp(
            [FromBody] SignUpWriteDto input,
            CancellationToken cancellationToken)
        {
            var signUp = await service.SignUp(input, cancellationToken);

            if (signUp.IsError)
            {
                return Problem(statusCode: (int)signUp.StatusCode, detail: signUp.Message);
            }

            var result = signUp.Data!.First().Adapt<SignUpReadDto>();

            return Ok(result);
        }
    }
}
