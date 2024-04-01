using BackendService.Application.Common.Settings;
using BackendService.Application.Login.Dtos;
using BackendService.Application.MsUsers.Dtos;
using BackendService.Domain;
using BackendService.Helper.Security;
using BackendService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendService.Application.Login.Service
{
    public class LoginService(ApplicationContext dbContext,
        IOptions<JwtAuthSetting> settings) : ILoginService
    {
        private IQueryable<MsUser> MsUserQuery()
        {
            return dbContext.MsUsers
                .Where(e => e.IsDeleted == false)
                .AsQueryable();
        }
        private async Task<MsUserReadDto> GetSingleUserAsync(string username, CancellationToken cancellationToken)
        {
            var user = await MsUserQuery()
                .Where(e => e.Username == username)
                .Select(e => new MsUserReadDto
                {
                    Id = e.Id,
                    Fullname = e.Fullname,
                    Username = e.Username,
                    Password = e.Password
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                return null!;
            }

            return user;
        }
        private static Task<bool> VerifyLoginAsync(MsUserReadDto existing, LoginWriteDto input, CancellationToken cancellationToken)
        {
            var isPasswordValid = Security.VerifyPassword(input.Password, existing.Password!);
            if (!isPasswordValid)
            {
                return Task.FromResult(false);
            }

            return Task.FromResult(true);
        }
        public async Task<LoginSuccessResponseReadDto> LoginAsync(LoginWriteDto input, CancellationToken cancellationToken)
        {
            //Get User
            var user = await GetSingleUserAsync(input.Username, cancellationToken);

            if (user is null)
            {
                return null!;
            }
            
            //Verify User
            var isValid = await VerifyLoginAsync(user, input, cancellationToken);

            if (!isValid)
            {
                return null!;
            }

            //Generate Token
            var token = GenerateJwtToken(user);

            return new LoginSuccessResponseReadDto
            {
                Id = user.Id,
                Fullname = user.Fullname,
                Username = user.Username,
                Token = token,
            };
        }

        private string GenerateJwtToken(MsUserReadDto user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(settings.Value.Secret!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim("id", user.Id.ToString()),
                        new Claim("username", user.Username!),
                        new Claim("role", "Admin"),
                    }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = settings.Value.Issuer
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
