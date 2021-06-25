using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.RegisterControl;
using VisitPop.Application.Responses;
using VisitPop.Application.Wrappers;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.RegisterControl
{
    public class RegisterControlRepository : IRegisterControlRepository
    {
        private readonly string WebAPIUrl;
        private readonly Uri uri;

        public RegisterControlRepository()
        {
            WebAPIUrl = "https://localhost:5001/api/RegisterControls/";
            uri = new Uri(WebAPIUrl);
        }

        public async Task<PagingResponse<RegisterControlDto>> GetRegisterControlsAsync(RegisterControlParametersDto registerControlParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = registerControlParameters.PageNumber.ToString(),
                ["pageSize"] = registerControlParameters.PageSize.ToString(),
                ["sortOrder"] = registerControlParameters.SortOrder.ToString(),
                ["filters"] = String.IsNullOrEmpty(registerControlParameters.Filters) ? "" : $"(Name|Location)@=* {registerControlParameters.Filters}"
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uri.ToString(), queryStringParam)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var pagingResponse = new PagingResponse<RegisterControlDto>
                        {
                            Items = JsonConvert.DeserializeObject<PageListRegisterControl>(content).RegisterControls,
                            Metadata = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("x-pagination").First())
                        };

                        pagingResponse.Filters = registerControlParameters.Filters;
                        pagingResponse.SortOrder = registerControlParameters.SortOrder;
                        return pagingResponse;

                    }
                    return null;
                }
            }
        }

        public async Task<RegisterControlDto> GetRegisterControl(int id)
        {
            RegisterControlDto registerControl = new RegisterControlDto();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri.AbsoluteUri + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        registerControl = JsonConvert.DeserializeObject<RegisterControlResponseDto>(apiResponse).RegisterControl;
                    }
                }
            }

            return registerControl;
        }

        public async Task<RegisterControlDto> AddRegisterControl(RegisterControlDto registerControl)
        {
            RegisterControlDto receivedRegisterControl = new RegisterControlDto();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(registerControl), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri.AbsoluteUri, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedRegisterControl = JsonConvert.DeserializeObject<RegisterControlResponseDto>(apiResponse).RegisterControl;
                }
            }
            return receivedRegisterControl;
        }

        public async Task UpdateRegisterControl(RegisterControlDto registerControl)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(registerControl), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(uri.AbsoluteUri + registerControl.Id, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public async Task DeleteRegisterControl(RegisterControlDto registerControl)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(uri.AbsoluteUri + registerControl.Id))
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
