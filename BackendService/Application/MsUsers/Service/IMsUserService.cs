using BackendService.Application.MsUsers.Dtos;

namespace BackendService.Application.MsUsers.Service
{
    public interface IMsUserService
    {
        Task<List<MsUserReadDto>> GetAllAsync(CancellationToken cancellationToken);
        Task<MsUserReadDto> GetSingleAsync(Guid id, CancellationToken cancellationToken);
    }
}
