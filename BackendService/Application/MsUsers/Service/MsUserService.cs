using BackendService.Application.MsUsers.Dtos;
using BackendService.Domain;
using BackendService.Helper.Exceptions;
using BackendService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Application.MsUsers.Service
{
    public class MsUserService(ApplicationContext dbContext) : IMsUserService
    {
        private IQueryable<MsUser> MsUserQuery()
        {
            return dbContext.MsUsers
                .Where(e => e.IsDeleted == false)
                .AsQueryable();
        }
        public async Task<List<MsUserReadDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var users = await MsUserQuery()
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new MsUserReadDto
                {
                    Id = e.Id,
                    Fullname = e.Fullname,
                    Username = e.Username
                })
                .ToListAsync(cancellationToken);

            return users;
        }

        public async Task<MsUserReadDto> GetSingleAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await MsUserQuery()
                .Where(e => e.Id == id)
                .Select(e => new MsUserReadDto
                {
                    Id = e.Id,
                    Fullname = e.Fullname,
                    Username = e.Username
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (user is null)
            {
                throw new NotFoundException("User");
            }

            return user;
        }
    }
}
