using BackendService.Application.Cities.Dtos;
using BackendService.Helper.Api;
using BackendService.Helper.Model;

namespace BackendService.Application.Cities.Service
{
    public interface ICitiesService
    {
        Task<PaginatedList<CitiesReadDto>> GetCitiesAsync(PaginationFilter filter, string? query, CancellationToken cancellationToken);
        Task<CitiesReadDto> GetSingleAsync(Guid id, CancellationToken cancellationToken);
    }
}
