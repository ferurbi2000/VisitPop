using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.PersonType;
using VisitPop.Application.Responses;
using VisitPop.Application.Wrappers;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.PersonType
{
    public class PersonTypeRepository : IPersonTypeRepository
    {
        private readonly string WebAPIUrl;
        private readonly Uri uri;

        public PersonTypeRepository()
        {
            WebAPIUrl = "https://localhost:5001/api/PersonTypes/";
            uri = new Uri(WebAPIUrl);
        }

        public async Task<PagingResponse<PersonTypeDto>> GetPersonTypesAsync(PersonTypeParametersDto personTypeParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = personTypeParameters.PageNumber.ToString(),
                ["pageSize"] = personTypeParameters.PageSize.ToString(),
                ["sortOrder"] = personTypeParameters.SortOrder.ToString(),
                ["filters"] = String.IsNullOrEmpty(personTypeParameters.Filters) ? "" : $"Name @=* {personTypeParameters.Filters}"
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uri.ToString(), queryStringParam)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var pagingResponse = new PagingResponse<PersonTypeDto>
                        {
                            Items = JsonConvert.DeserializeObject<PageListPersonType>(content).PersonTypes,
                            Metadata = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("x-pagination").First())
                        };

                        pagingResponse.Filters = personTypeParameters.Filters;
                        pagingResponse.SortOrder = personTypeParameters.SortOrder;
                        return pagingResponse;
                    }
                    return null;
                }
            }
        }

        public async Task<PersonTypeDto> GetPersonType(int id)
        {
            PersonTypeDto personType = new PersonTypeDto();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri.AbsoluteUri + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        personType = JsonConvert.DeserializeObject<PersonTypeResponseDto>(apiResponse).PersonType;
                    }
                }
            }
            return personType;
        }

        public async Task<PersonTypeDto> AddPersonType(PersonTypeDto personType)
        {
            PersonTypeDto receivedPersonType = new PersonTypeDto();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(personType), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri.AbsoluteUri, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedPersonType = JsonConvert.DeserializeObject<PersonTypeResponseDto>(apiResponse).PersonType;
                }
            }
            return receivedPersonType;
        }

        public async Task UpdatePersonType(PersonTypeDto personType)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(personType), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(uri.AbsoluteUri + personType.Id, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public async Task DeletePersonType(PersonTypeDto personType)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(uri.AbsoluteUri + personType.Id))
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
