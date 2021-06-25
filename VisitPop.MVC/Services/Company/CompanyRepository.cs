using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Company;
using VisitPop.Application.Responses;
using VisitPop.Application.Wrappers;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.Company
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly string WebAPIUrl;
        private readonly Uri uri;

        public CompanyRepository()
        {
            WebAPIUrl = "https://localhost:5001/api/Companies/";
            uri = new Uri(WebAPIUrl);
        }

        public async Task<PagingResponse<CompanyDto>> GetCompaniesAsync(CompanyParametersDto companyParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = companyParameters.PageNumber.ToString(),
                ["pageSize"] = companyParameters.PageSize.ToString(),
                ["sortOrder"] = companyParameters.SortOrder.ToString(),
                ["filters"] = String.IsNullOrEmpty(companyParameters.Filters) ? "" : $"Name @=* {companyParameters.Filters}"
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uri.ToString(), queryStringParam)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var pagingResponse = new PagingResponse<CompanyDto>
                        {
                            Items = JsonConvert.DeserializeObject<PageListCompany>(content).Companies,
                            Metadata = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("x-pagination").First())
                        };

                        pagingResponse.Filters = companyParameters.Filters;
                        pagingResponse.SortOrder = companyParameters.SortOrder;
                        return pagingResponse;
                    }
                    return null;
                }
            }
        }

        public async Task<CompanyDto> GetCompany(int id)
        {
            CompanyDto company = new CompanyDto();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri.AbsoluteUri + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        company = JsonConvert.DeserializeObject<CompanyResponseDto>(apiResponse).Company;
                    }
                }
            }
            return company;
        }

        public async Task<CompanyDto> AddCompany(CompanyDto company)
        {
            CompanyDto receivedCompany = new CompanyDto();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri.AbsoluteUri, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedCompany = JsonConvert.DeserializeObject<CompanyResponseDto>(apiResponse).Company;
                }
            }
            return receivedCompany;
        }

        public async Task UpdateCompany(CompanyDto company)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(company), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(uri.AbsoluteUri + company.Id, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public async Task DeleteCompany(CompanyDto company)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(uri.AbsoluteUri + company.Id))
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
