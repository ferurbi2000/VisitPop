using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Application.Dtos.Office;
using VisitPop.Application.Dtos.RegisterControl;
using VisitPop.Application.Dtos.Visit;
using VisitPop.Application.Dtos.VisitState;
using VisitPop.Application.Dtos.VisitType;
using VisitPop.Application.Responses;
using VisitPop.Application.Wrappers;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.Visit
{
    public class VisitRepository : IVisitRepository
    {
        private readonly string WebAPIUrl;
        private readonly string WebAPIUrlVisitType;
        private readonly string WebAPIUrlEmployee;
        private readonly string WebAPIUrlOffice;
        private readonly string WebAPIUrlRegisterControl;
        private readonly string WebAPIUrlVisitState;

        private readonly Uri uri;
        private readonly Uri uriVisitType;
        private readonly Uri uriEmployee;
        private readonly Uri uriOffice;
        private readonly Uri uriRegisterControl;
        private readonly Uri uriVisitState;

        public VisitRepository()
        {
            WebAPIUrl = "https://localhost:5001/api/Visits/";
            uri = new Uri(WebAPIUrl);

            WebAPIUrlVisitType = "https://localhost:5001/api/VisitTypes/";
            uriVisitType = new Uri(WebAPIUrlVisitType);

            WebAPIUrlEmployee = "https://localhost:5001/api/Employees/";
            uriEmployee = new Uri(WebAPIUrlEmployee);

            WebAPIUrlOffice = "https://localhost:5001/api/Offices/";
            uriOffice = new Uri(WebAPIUrlOffice);

            WebAPIUrlRegisterControl = "https://localhost:5001/api/RegisterControls/";
            uriRegisterControl = new Uri(WebAPIUrlRegisterControl);

            WebAPIUrlVisitState = "https://localhost:5001/api/VisitStates/";
            uriVisitState = new Uri(WebAPIUrlVisitState);
        }


        public async Task<PagingResponse<VisitTypeDto>> GetVisitTypesAsync(VisitTypeParametersDto visitTypeParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = visitTypeParameters.PageNumber.ToString(),
                ["pageSize"] = visitTypeParameters.PageSize.ToString(),
                ["sortOrder"] = visitTypeParameters.SortOrder.ToString(),
                ["filters"] = visitTypeParameters.Filters.ToString()
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uriVisitType.ToString(), queryStringParam)))
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

        public async Task<PagingResponse<EmployeeDto>> GetEmployeesAsync(EmployeeParametersDto employeeParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = employeeParameters.PageNumber.ToString(),
                ["pageSize"] = employeeParameters.PageSize.ToString(),
                ["sortOrder"] = employeeParameters.SortOrder.ToString(),
                ["filters"] = employeeParameters.Filters.ToString()
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uriEmployee.ToString(), queryStringParam)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var pagingResponse = new PagingResponse<EmployeeDto>
                        {
                            Items = JsonConvert.DeserializeObject<PageListEmployee>(content).Employees,
                            Metadata = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("x-pagination").First())
                        };

                        pagingResponse.Filters = employeeParameters.Filters;
                        pagingResponse.SortOrder = employeeParameters.SortOrder;
                        return pagingResponse;

                    }
                    return null;
                }
            }
        }

        public async Task<PagingResponse<OfficeDto>> GetOfficesAsync(OfficeParametersDto officeParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = officeParameters.PageNumber.ToString(),
                ["pageSize"] = officeParameters.PageSize.ToString(),
                ["sortOrder"] = officeParameters.SortOrder.ToString(),
                ["filters"] = officeParameters.Filters.ToString()
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uriOffice.ToString(), queryStringParam)))
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

        public async Task<PagingResponse<RegisterControlDto>> GetRegisterControlsAsync(RegisterControlParametersDto registerControlParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = registerControlParameters.PageNumber.ToString(),
                ["pageSize"] = registerControlParameters.PageSize.ToString(),
                ["sortOrder"] = registerControlParameters.SortOrder.ToString(),
                ["filters"] = registerControlParameters.Filters.ToString()
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uriRegisterControl.ToString(), queryStringParam)))
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

        public async Task<PagingResponse<VisitStateDto>> GetVisitStatesAsync(VisitStateParametersDto visitStateParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = visitStateParameters.PageNumber.ToString(),
                ["pageSize"] = visitStateParameters.PageSize.ToString(),
                ["sortOrder"] = visitStateParameters.SortOrder.ToString(),
                ["filters"] = visitStateParameters.Filters.ToString()
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uriVisitState.ToString(), queryStringParam)))
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


        public async Task<PagingResponse<VisitDto>> GetVisitsAsync(VisitParametersDto visitParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = visitParameters.PageNumber.ToString(),
                ["pageSize"] = visitParameters.PageSize.ToString(),
                ["sortOrder"] = visitParameters.SortOrder.ToString(),
                ["filters"] = String.IsNullOrEmpty(visitParameters.Filters) ? "" : $"(Reason|Company)@=* {visitParameters.Filters}"
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uri.ToString(), queryStringParam)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var pagingResponse = new PagingResponse<VisitDto>
                        {
                            Items = JsonConvert.DeserializeObject<PageListVisit>(content).Visits,
                            Metadata = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("x-pagination").First())
                        };

                        pagingResponse.Filters = visitParameters.Filters;
                        pagingResponse.SortOrder = visitParameters.SortOrder;
                        return pagingResponse;

                    }
                    return null;
                }
            }
        }

        public async Task<VisitDto> GetVisit(int id)
        {
            VisitDto visit = new VisitDto();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri.AbsoluteUri + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        visit = JsonConvert.DeserializeObject<VisitResponseDto>(apiResponse).Visit;
                    }
                }
            }

            return visit;
        }

        public async Task<VisitDto> AddVisit(VisitDto visit)
        {
            VisitDto receivedVisit = new VisitDto();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(visit), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri.AbsoluteUri, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedVisit = JsonConvert.DeserializeObject<VisitResponseDto>(apiResponse).Visit;
                }
            }
            return receivedVisit;
        }

        public async Task UpdateVisit(VisitDto visit)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(visit), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(uri.AbsoluteUri + visit.Id, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public async Task DeleteVisit(VisitDto visit)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(uri.AbsoluteUri + visit.Id))
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
