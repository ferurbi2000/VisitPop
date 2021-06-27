using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Company;
using VisitPop.Application.Dtos.Person;
using VisitPop.Application.Dtos.PersonType;
using VisitPop.Application.Responses;
using VisitPop.Application.Wrappers;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.Person
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string WebAPIUrl, WebAPIUrlPersonType, WebAPIUrlCompany;
        private readonly Uri uri, uriPersonType, uriCompany;

        public PersonRepository()
        {
            WebAPIUrl = "https://localhost:5001/api/Persons/";
            uri = new Uri(WebAPIUrl);

            WebAPIUrlPersonType = "https://localhost:5001/api/PersonTypes/";
            uriPersonType = new Uri(WebAPIUrlPersonType);

            WebAPIUrlCompany = "https://localhost:5001/api/Companies/";
            uriCompany = new Uri(WebAPIUrlCompany);
        }

        public async Task<PagingResponse<PersonTypeDto>> GetPersonTypesAsync(PersonTypeParametersDto PersonTypeParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = PersonTypeParameters.PageNumber.ToString(),
                ["pageSize"] = PersonTypeParameters.PageSize.ToString(),
                ["sortOrder"] = PersonTypeParameters.SortOrder.ToString(),
                ["filters"] = PersonTypeParameters.Filters.ToString()
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uriPersonType.ToString(), queryStringParam)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var pagingResponse = new PagingResponse<PersonTypeDto>
                        {
                            Items = JsonConvert.DeserializeObject<PageListPersonType>(content).PersonTypes,
                            Metadata = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("x-pagination").First())
                        };

                        pagingResponse.Filters = PersonTypeParameters.Filters;
                        pagingResponse.SortOrder = PersonTypeParameters.SortOrder;
                        return pagingResponse;

                    }
                    return null;
                }
            }
        }

        public async Task<PagingResponse<CompanyDto>> GetCompaniesAsync(CompanyParametersDto companyParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = companyParameters.PageNumber.ToString(),
                ["pageSize"] = companyParameters.PageSize.ToString(),
                ["sortOrder"] = companyParameters.SortOrder.ToString(),
                ["filters"] = companyParameters.Filters.ToString()
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uriCompany.ToString(), queryStringParam)))
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

        public async Task<PagingResponse<PersonDto>> GetPersonsAsync(PersonParametersDto personParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = personParameters.PageNumber.ToString(),
                ["pageSize"] = personParameters.PageSize.ToString(),
                ["sortOrder"] = personParameters.SortOrder.ToString(),
                ["filters"] = String.IsNullOrEmpty(personParameters.Filters) ? "" : $"(FirstName|LastName|EmailAddress)@=* {personParameters.Filters}"
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uri.ToString(), queryStringParam)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var pagingResponse = new PagingResponse<PersonDto>
                        {
                            Items = JsonConvert.DeserializeObject<PageListPerson>(content).Persons,
                            Metadata = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("x-pagination").First())
                        };

                        pagingResponse.Filters = personParameters.Filters;
                        pagingResponse.SortOrder = personParameters.SortOrder;
                        return pagingResponse;

                    }
                    return null;
                }
            }
        }

        public async Task<PersonDto> GetPerson(int id)
        {
            PersonDto person = new PersonDto();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri.AbsoluteUri + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        person = JsonConvert.DeserializeObject<PersonResponseDto>(apiResponse).Person;
                    }
                }
            }

            return person;
        }

        public async Task<PersonDto> AddPerson(PersonDto person)
        {
            PersonDto receivedPerson = new PersonDto();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri.AbsoluteUri, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedPerson = JsonConvert.DeserializeObject<PersonResponseDto>(apiResponse).Person;
                }
            }
            return receivedPerson;
        }

        public async Task UpdatePerson(PersonDto person)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(person), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(uri.AbsoluteUri + person.Id, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public async Task DeletePerson(PersonDto person)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(uri.AbsoluteUri + person.Id))
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