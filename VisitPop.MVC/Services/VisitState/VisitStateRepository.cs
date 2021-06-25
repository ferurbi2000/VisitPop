using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitState;
using VisitPop.Application.Responses;
using VisitPop.Application.Wrappers;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.VisitState
{
    public class VisitStateRepository : IVisitStateRepository
    {
        private readonly string WebAPIUrl;
        private readonly Uri uri;

        public VisitStateRepository()
        {
            WebAPIUrl = "https://localhost:5001/api/VisitStates/";
            uri = new Uri(WebAPIUrl);
        }

        public async Task<PagingResponse<VisitStateDto>> GetVisitStatesAsync(VisitStateParametersDto visitStateParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = visitStateParameters.PageNumber.ToString(),
                ["pageSize"] = visitStateParameters.PageSize.ToString(),
                ["sortOrder"] = visitStateParameters.SortOrder.ToString(),
                ["filters"] = String.IsNullOrEmpty(visitStateParameters.Filters) ? "" : $"Name @=* {visitStateParameters.Filters}"
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uri.ToString(), queryStringParam)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var pagingResponse = new PagingResponse<VisitStateDto>
                        {
                            Items = JsonConvert.DeserializeObject<PageListVisitState>(content).VisitStates,
                            Metadata = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("x-pagination").First())
                        };

                        pagingResponse.Filters = visitStateParameters.Filters;
                        pagingResponse.SortOrder = visitStateParameters.SortOrder;
                        return pagingResponse;
                    }
                    return null;
                }
            }
        }

        public async Task<VisitStateDto> GetVisitState(int id)
        {
            VisitStateDto visitState = new VisitStateDto();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri.AbsoluteUri + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        visitState = JsonConvert.DeserializeObject<VisitStateResponseDto>(apiResponse).VisitState;
                    }
                }
            }
            return visitState;
        }

        public async Task<VisitStateDto> AddVisitState(VisitStateDto visitState)
        {
            VisitStateDto receivedVisitState = new VisitStateDto();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(visitState), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri.AbsoluteUri, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedVisitState = JsonConvert.DeserializeObject<VisitStateResponseDto>(apiResponse).VisitState;
                }
            }
            return receivedVisitState;
        }

        public async Task UpdateVisitState(VisitStateDto visitState)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(visitState), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(uri.AbsoluteUri + visitState.Id, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public async Task DeleteVisitState(VisitStateDto visitState)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(uri.AbsoluteUri + visitState.Id))
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
