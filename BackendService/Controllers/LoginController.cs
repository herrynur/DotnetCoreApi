using BackendService.Application.Login.Dtos;
using BackendService.Application.Login.Service;
using BackendService.Helper.Model;
using BackendService.Helper.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BackendService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class LoginController(ILoginService service) : ControllerBase
    {
        /// <summary>
        /// Auth Login
        /// </summary>
        /// <remarks>
        /// Login with with username and password to get Access Token.
        /// 
        /// Sample request:
        /// 
        ///     POST /Login
        ///     {
        ///         "Username": "admin",
        ///         "Password": "P@ssw0rd"
        ///     }
        ///     
        ///  If the request is successful, the response will be as follows:
        ///  
        ///     {
        ///         "Id": "cdf76df8-d15c-4a0b-9c77-ec50dead8dac",
        ///         "Username": "admin",
        ///         "Fullname": "Admin",
        ///         "Token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6ImNkZjc2ZGY4LWQxNWMtNGEwYi05Yzc3LWVjNTBkZWFkOGRhYyIsInVzZXJuYW1l"
        ///     }
        ///     
        ///  You can use the Token value in the Authorization header as follows:
        ///  
        ///  Authorization: Token {token} (Token+space+{token})
        ///  
        ///  In Swagger you can use Token value in Authorize button and  fill the value :
        ///  Token {token} (Token+space+{token})
        ///  
        /// </remarks>
        /// <param name="input"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>This endpoint returns a user information and access token</returns>
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(LoginSuccessResponseReadDto))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(DefaultErrorResponse.BadRequestLogin))]
        public async Task<ActionResult<LoginSuccessResponseReadDto>> LoginAsync(
            [FromBody] LoginWriteDto input,
            CancellationToken cancellationToken)
        { 
            var login = await service.LoginAsync(input, cancellationToken);

            if (login is null)
            {
                return Problem(statusCode: 400, detail: ResponseMessageExtensions.Login.UsernameOrPasswordIncorrect);
            }

            return Ok(login);
        }
    }
}
