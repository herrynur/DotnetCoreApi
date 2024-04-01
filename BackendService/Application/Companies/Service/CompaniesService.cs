using BackendService.Application.Companies.Dtos;
using BackendService.Domain;
using BackendService.Helper.Api;
using BackendService.Helper.Model;
using BackendService.Infrastructure.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BackendService.Application.Companies.Service
{
    public class CompaniesService(ApplicationContext context) : ICompaniesService
    {
        private IQueryable<Company> CompaniesQuery()
        {
            return context.Companies
                .Where(e => e.IsDeleted == false)
                .AsQueryable();
        }
        public async Task<PaginatedList<CompaniesReadDto>> GetAllAsync(PaginationFilter filter, string? Query, CancellationToken cancellationToken)
        {
            var query = CompaniesQuery();

            //Apply filter
            if (!string.IsNullOrWhiteSpace(Query))
            {
                query = query.Where(e => EF.Functions.ILike(e.Name!, "%" + Query + "%")).AsQueryable();
            }

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var totalRecords = await query.CountAsync(cancellationToken);
            
            var companies = await query
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .OrderByDescending(e => e.CreatedAt)
                .Select(e => new CompaniesReadDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Code = e.Code,
                    PIC = e.PIC,
                    PhoneNumber = e.PhoneNumber,
                    Address = e.Address
                }).ToListAsync(cancellationToken);

            var hasNextPage = totalRecords > validFilter.PageSize * validFilter.PageNumber;
            var hasPreviousPage = validFilter.PageNumber > 1;

            if (companies.Count == 0)
            {
                return new PaginatedList<CompaniesReadDto>(validFilter.PageNumber, validFilter.PageSize, totalRecords, new List<CompaniesReadDto>(), hasPreviousPage, hasNextPage);
            }

            var result = new PaginatedList<CompaniesReadDto>(validFilter.PageNumber, validFilter.PageSize, totalRecords, companies, hasPreviousPage, hasNextPage);

            return result;
        }

        public async Task<ResponseBaseModel<Company>> Post(CompaniesWriteDto input, CancellationToken cancellationToken)
        {
            var response = new ResponseBaseModel<Company>();
            
            //Check is exist
            var isExist = await CompaniesQuery().AnyAsync(e => e.Code == input.Code, cancellationToken);
            if (isExist)
            {
                response.Message = "Company Code already exist";
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsError = true;

                return response;
            }

            //Post
            try
            {
                var company = new Company()
                {
                    Code = input.Code,
                    Name = input.Name,
                    PIC = input.PIC,
                    PhoneNumber = input.PhoneNumber,
                    Address = input.Address,
                };

                await context.Companies.AddAsync(company, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                response.StatusCode = HttpStatusCode.OK;
                response.IsError = false;
                response.Data = [company];

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsError = true;

                return response;
            }
        }

        public async Task<ResponseBaseModel<Company>> Put(Guid id, CompaniesWriteDto input, CancellationToken cancellationToken)
        {
            var response = new ResponseBaseModel<Company>();

            var company = await CompaniesQuery().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
            if (company is null)
            {
                response.Message = "Company not found";
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsError = true;

                return response;
            }

            try
            {
                company.Name = input.Name;
                company.Code = input.Code;
                company.Address = input.Address;
                company.PIC = input.PIC;
                company.PhoneNumber = input.PhoneNumber;
                company.UpdatedAt = DateTime.UtcNow;

                context.Companies.Update(company);
                await context.SaveChangesAsync(cancellationToken);

                response.StatusCode = HttpStatusCode.OK;
                response.IsError = false;
                response.Data = [company];
                
                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsError = true;
                
                return response;
            }
        }

        public async Task<CompaniesReadDto> GetSingleAsync(Guid id, CancellationToken cancellationToken)
        {
            var company = await CompaniesQuery().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (company is null)
            {
                return null!;
            }

            var result = company.Adapt<CompaniesReadDto>();

            return result;
        }

        public async Task<ResponseBaseModel<Company>> Delete(Guid id, CancellationToken cancellationToken)
        {
            var response = new ResponseBaseModel<Company>();

            var company = await CompaniesQuery().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

            if (company is null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.Message = "Company not found";
                response.IsError = true;

                return response;
            }

            try
            {
                company.IsDeleted = true;
                company.IsActive = false;
                company.UpdatedAt = DateTime.UtcNow;

                context.Companies.Update(company);
                await context.SaveChangesAsync(cancellationToken);

                response.StatusCode = HttpStatusCode.OK;
                response.IsError = false;
                response.Data = [company];

                return response;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsError = true;

                return response;
            }
        }
    }
}
