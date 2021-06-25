using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Office;
using VisitPop.Application.Responses;
using VisitPop.Application.Wrappers;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.Office
{
    public class OfficeRepository : IOfficeRepository
    {
        private readonly string WebAPIUrl;
        private readonly Uri uri;

        public OfficeRepository()
        {
            WebAPIUrl = "https://localhost:5001/api/Offices/";
            uri = new Uri(WebAPIUrl);
        }

        public async Task<PagingResponse<OfficeDto>> GetOfficesAsync(OfficeParametersDto officeParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = officeParameters.PageNumber.ToString(),
                ["pageSize"] = officeParameters.PageSize.ToString(),
                ["sortOrder"] = officeParameters.SortOrder.ToString(),
                ["filters"] = String.IsNullOrEmpty(officeParameters.Filters) ? "" : $"Name @=* {officeParameters.Filters}"
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uri.ToString(), queryStringParam)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var pagingResponse = new PagingResponse<OfficeDto>
                        {
                            Items = JsonConvert.DeserializeObject<PageListOffice>(content).Offices,
                            Metadata = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("x-pagination").First())
                        };

                        pagingResponse.Filters = officeParameters.Filters;
                        pagingResponse.SortOrder = officeParameters.SortOrder;
                        return pagingResponse;
                    }
                    return null;
                }
            }
        }

        public async Task<OfficeDto> GetOffice(int id)
        {
            OfficeDto office = new OfficeDto();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri.AbsoluteUri + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        office = JsonConvert.DeserializeObject<OfficeResponseDto>(apiResponse).Office;
                    }
                }
            }
            return office;
        }

        public async Task<OfficeDto> AddOffice(OfficeDto office)
        {
            OfficeDto receivedOffice = new OfficeDto();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(office), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri.AbsoluteUri, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedOffice = JsonConvert.DeserializeObject<OfficeResponseDto>(apiResponse).Office;
                }
            }
            return receivedOffice;
        }

        public async Task UpdateOffice(OfficeDto office)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(office), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(uri.AbsoluteUri + office.Id, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public async Task DeleteOffice(OfficeDto office)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(uri.AbsoluteUri + office.Id))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        //string apiResponse = await response.Content.ReadAsStringAsync();
                        throw new Exception();
                    }
                }
            }
        }

    }
}
