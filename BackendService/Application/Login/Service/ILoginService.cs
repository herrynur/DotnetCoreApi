using BackendService.Application.Login.Dtos;

namespace BackendService.Application.Login.Service
{
    public interface ILoginService
    {
        Task<LoginSuccessResponseReadDto> LoginAsync(LoginWriteDto input, CancellationToken cancellationToken);
    }
}
