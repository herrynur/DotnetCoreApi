using BackendService.Application.SignUp.Dtos;
using BackendService.Domain;
using BackendService.Helper.Model;
using BackendService.Helper.Security;
using BackendService.Infrastructure.Persistence;
using System.Net;

namespace BackendService.Application.SignUp.Service
{
    public class SignUpService(ApplicationContext context) : ISignUpService
    {
        public async Task<ResponseBaseModel<MsUser>> SignUp(SignUpWriteDto input, CancellationToken cancellationToken)
        {
			var response = new ResponseBaseModel<MsUser>();
			try
			{
				var user = new MsUser
				{
					Fullname = input.Fullname,
					Username = input.Username,
					Password = Security.CreateHash(input.Password)
				};

				await context.MsUsers.AddAsync(user, cancellationToken);
				await context.SaveChangesAsync(cancellationToken);

				response.Data = [user];
				return response;
			}
			catch (Exception ex)
			{
				response.IsError = true;
				response.Message = ex.Message;
				response.StatusCode = HttpStatusCode.BadRequest;

				return response;
			}
        }
    }
}
