using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitType;
using VisitPop.Application.Responses;
using VisitPop.Application.Wrappers;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.VisitType
{
    public class VisitTypeRepository : IVisitTypeRepository
    {

        private readonly string WebAPIUrl;
        private readonly Uri uri;

        public VisitTypeRepository()
        {
            WebAPIUrl = "https://localhost:5001/api/VisitTypes/";
            uri = new Uri(WebAPIUrl);
        }


        public async Task<PagingResponse<VisitTypeDto>> GetVisitTypesAsync(VisitTypeParametersDto visitTypeParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = visitTypeParameters.PageNumber.ToString(),
                ["pageSize"] = visitTypeParameters.PageSize.ToString(),
                ["sortOrder"] = visitTypeParameters.SortOrder.ToString(),
                ["filters"] = String.IsNullOrEmpty(visitTypeParameters.Filters) ? "" : $"Name @=* {visitTypeParameters.Filters}"
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uri.ToString(), queryStringParam)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var pagingResponse = new PagingResponse<VisitTypeDto>
                        {
                            Items = JsonConvert.DeserializeObject<PageListVisitType>(content).VisitTypes,
                            Metadata = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("x-pagination").First())
                        };

                        pagingResponse.Filters = visitTypeParameters.Filters;
                        pagingResponse.SortOrder = visitTypeParameters.SortOrder;
                        return pagingResponse;
                    }
                    return null;
                }
            }
        }

        public async Task<VisitTypeDto> GetVisitType(int id)
        {
            VisitTypeDto visitType = new VisitTypeDto();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri.AbsoluteUri + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        visitType = JsonConvert.DeserializeObject<VisitTypeResponseDto>(apiResponse).VisitType;
                    }
                }
            }
            return visitType;
        }

        public async Task<VisitTypeDto> AddVisitType(VisitTypeDto visitType)
        {
            VisitTypeDto receivedVisitType = new VisitTypeDto();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(visitType), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri.AbsoluteUri, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedVisitType = JsonConvert.DeserializeObject<VisitTypeResponseDto>(apiResponse).VisitType;
                }
            }
            return receivedVisitType;
        }

        public async Task UpdateVisitType(VisitTypeDto visitType)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(visitType), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(uri.AbsoluteUri + visitType.Id, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public async Task DeleteVisitType(VisitTypeDto visitType)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(uri.AbsoluteUri + visitType.Id))
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
