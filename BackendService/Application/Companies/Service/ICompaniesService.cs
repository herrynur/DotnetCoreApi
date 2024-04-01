using BackendService.Application.Companies.Dtos;
using BackendService.Domain;
using BackendService.Helper.Api;
using BackendService.Helper.Model;

namespace BackendService.Application.Companies.Service
{
    public interface ICompaniesService
    {
        Task<PaginatedList<CompaniesReadDto>> GetAllAsync(PaginationFilter filter, string? Query, CancellationToken cancellationToken);
        Task<CompaniesReadDto> GetSingleAsync(Guid id, CancellationToken cancellationToken);
        Task<ResponseBaseModel<Company>> Post(CompaniesWriteDto input, CancellationToken cancellationToken);
        Task<ResponseBaseModel<Company>> Put(Guid id, CompaniesWriteDto input, CancellationToken cancellationToken);
        Task<ResponseBaseModel<Company>> Delete(Guid id, CancellationToken cancellationToken);
    }
}
