using BackendService.Application.Cities.Dtos;
using BackendService.Application.Common.Settings;
using BackendService.Domain;
using BackendService.Helper.Api;
using BackendService.Helper.Exceptions;
using BackendService.Helper.Model;
using BackendService.Infrastructure.Persistence;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace BackendService.Application.Cities.Service
{
    public class CitiesService(IHttpClientFactory httpClientFactory,
        IOptions<HjexServiceSetting> setting,
        ApplicationContext context) : ICitiesService
    {
        private readonly HttpClient httpClient = httpClientFactory.CreateClient(nameof(CitiesService));
        public async Task<PaginatedList<CitiesReadDto>> GetCitiesAsync(PaginationFilter filter, string? query, CancellationToken cancellationToken)
        {
            //set query
            var queryRequest = HttpUtility.ParseQueryString(string.Empty);
            queryRequest[nameof(filter.PageNumber)] = filter.PageNumber.ToString();
            queryRequest[nameof(filter.PageSize)] = filter.PageSize.ToString();
            if (!string.IsNullOrEmpty(query))
            {
                queryRequest[nameof(query)] = query;
            }
            //End set queryRequest

            //set Url
            var uri = new Uri($"{setting.Value.Host}hjexbackend/v1/MsKota?{queryRequest}");


            //Request to server
            //Create request
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = uri,
                Content = new StringContent(JsonConvert.SerializeObject(queryRequest), Encoding.UTF8, "application/json")
            };

            //Request
            var response = await httpClient.SendAsync(request, cancellationToken);
            var responseContentAsString = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new AppException($"{response.StatusCode}");
            }

            //Result
            var resultConvert = JsonConvert.DeserializeObject<PaginatedList<CitiesResponseModel>>(responseContentAsString)!;

            //Add to db or update
            var input = resultConvert.Data.ToList();
            await CreatedOrUpdatedtoDb(input, cancellationToken);
            //End

            var result = resultConvert.Adapt<PaginatedList<CitiesReadDto>>();

            return result!;
        }

        public async Task<CitiesReadDto> GetSingleAsync(Guid id, CancellationToken cancellationToken)
        {
            var city = await context.Cities.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false, cancellationToken);
            var result = city.Adapt<CitiesReadDto>();
            
            return result;
        }

        private async Task<List<City>> CreatedOrUpdatedtoDb(List<CitiesResponseModel> input, CancellationToken cancellationToken)
        {
            try
            {
                //Check if id is exist in db
                var ids = input.Select(x => x.Id).ToList();
                var citiesExist = await context.Cities
                    .Where(x => ids.Contains(x.Id))
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);

                //Add if not exist
                var citiesToAdd = input
                    .Where(x => !citiesExist.Contains(x.Id))
                    .Select(x => x.Adapt<City>())
                    .ToList();

                //Add to db
                await context.Cities.AddRangeAsync(citiesToAdd, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                //Update if exist
                //soon
                //var citiesToUpdate = input
                //    .Where(x => citiesExist.Contains(x.Id))
                //    .Select(x => x.Adapt<City>())
                //    .ToList();

                return citiesToAdd;
            }
            catch (Exception)
            {
                return [];
            }
        }
        private class CitiesResponseModel
        {
            [JsonProperty("Guid")]
            public Guid Id { get; set; }
            [JsonProperty("Id")]
            public long ExternalId { get; set; }
            [JsonProperty("Nama")]
            public string? Name { get; set; }
            [JsonProperty("Kode")]
            public string? Code { get; set; }
            [JsonProperty("Provinsi")]
            public string? Province { get; set; }
        }
    }
}
