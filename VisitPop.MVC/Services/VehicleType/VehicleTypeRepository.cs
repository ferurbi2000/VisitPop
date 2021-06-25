using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VehicleType;
using VisitPop.Application.Responses;
using VisitPop.Application.Wrappers;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.VehicleType
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly string WebAPIUrl;
        private readonly Uri uri;

        public VehicleTypeRepository()
        {
            WebAPIUrl = "https://localhost:5001/api/VehicleTypes/";
            uri = new Uri(WebAPIUrl);
        }

        public async Task<PagingResponse<VehicleTypeDto>> GetVehicleTypesAsync(VehicleTypeParametersDto vehicleTypeParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = vehicleTypeParameters.PageNumber.ToString(),
                ["pageSize"] = vehicleTypeParameters.PageSize.ToString(),
                ["sortOrder"] = vehicleTypeParameters.SortOrder.ToString(),
                ["filters"] = String.IsNullOrEmpty(vehicleTypeParameters.Filters) ? "" : $"Name @=* {vehicleTypeParameters.Filters}"
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uri.ToString(), queryStringParam)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var pagingResponse = new PagingResponse<VehicleTypeDto>
                        {
                            Items = JsonConvert.DeserializeObject<PageListVehicleType>(content).VehicleTypes,
                            Metadata = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("x-pagination").First())
                        };

                        pagingResponse.Filters = vehicleTypeParameters.Filters;
                        pagingResponse.SortOrder = vehicleTypeParameters.SortOrder;
                        return pagingResponse;
                    }
                    return null;
                }
            }
        }

        public async Task<VehicleTypeDto> GetVehicleType(int id)
        {
            VehicleTypeDto vehicleType = new VehicleTypeDto();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri.AbsoluteUri + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        vehicleType = JsonConvert.DeserializeObject<VehicleTypeResponseDto>(apiResponse).VehicleType;
                    }
                }
            }
            return vehicleType;
        }

        public async Task<VehicleTypeDto> AddVehicleType(VehicleTypeDto vehicleType)
        {
            VehicleTypeDto vehicleTypeCompany = new VehicleTypeDto();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(vehicleType), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri.AbsoluteUri, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    vehicleTypeCompany = JsonConvert.DeserializeObject<VehicleTypeResponseDto>(apiResponse).VehicleType;
                }
            }
            return vehicleTypeCompany;
        }

        public async Task UpdateVehicleType(VehicleTypeDto vehicleType)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(vehicleType), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(uri.AbsoluteUri + vehicleType.Id, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public async Task DeleteVehicleType(VehicleTypeDto vehicleType)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(uri.AbsoluteUri + vehicleType.Id))
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
