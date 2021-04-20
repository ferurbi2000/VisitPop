using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.DepartamentoEmpleado;
using VisitPop.Application.Responses;
using VisitPop.Application.Wrappers;
using VisitPop.MVC.Features;
using VisitPop.MVC.Models.ViewModels;

namespace VisitPop.MVC.Services.DepartamentoEmpleado
{
    public class DepartamentoEmpleadoRepository : IDepartamentoEmpleadoRepository
    {
        private readonly string WebAPIUrl;
        private readonly Uri uri;

        public DepartamentoEmpleadoRepository()
        {
            WebAPIUrl = "https://localhost:5001/api/DepartamentoEmpleados/";
            uri = new Uri(WebAPIUrl);
        }

        //public async Task<IEnumerable<DepartamentoEmpleadoDto>> GetDepartamentoEmpleadosAsync(int pageNumber, int pageSize)
        public async Task<PagingResponse<DepartamentoEmpleadoDto>> GetDepartamentoEmpleadosAsync(DepartamentoEmpleadoParametersDto departamentoEmpleadoParameters)
        {

            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = departamentoEmpleadoParameters.PageNumber.ToString(),
                ["pageSize"] = departamentoEmpleadoParameters.PageSize.ToString(),
                ["sortOrder"] = departamentoEmpleadoParameters.SortOrder.ToString(),
                ["filters"] = String.IsNullOrEmpty(departamentoEmpleadoParameters.Filters) ? "" : $"Nombre @=* {departamentoEmpleadoParameters.Filters}"
            };

            

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uri.ToString(), queryStringParam)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var pagingResponse = new PagingResponse<DepartamentoEmpleadoDto>
                        {
                            //Items = JsonConvert.DeserializeObject<PageListDepartamentoEmpleado>(content).DepartamentoEmpleados,
                            Items = JsonConvert.DeserializeObject<PageListDepartamentoEmpleado>(content).DepartamentoEmpleados,
                            Metadata = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("x-pagination").First())
                        };

                        pagingResponse.Filters = departamentoEmpleadoParameters.Filters;
                        pagingResponse.SortOrder = departamentoEmpleadoParameters.SortOrder;
                        return pagingResponse;

                    }
                    return null;
                }
            }



            //List<DepartamentoEmpleadoDto> departamentoEmpleados = new List<DepartamentoEmpleadoDto>();

            //using (var httpClient = new HttpClient())
            //{
            //    try
            //    {
            //        using (var response = await httpClient.GetAsync($"{uri}?PageNumber={pageNumber}&PageSize={pageSize}"))
            //        {

            //            if (response.IsSuccessStatusCode)
            //            {
            //                string apiResponse = await response.Content.ReadAsStringAsync();
            //                //departamentoEmpleados = JsonConvert.DeserializeObject<PageListDepartamentoEmpleado>(apiResponse).DepartamentoEmpleados;

            //                departamentoEmpleados = JsonConvert.DeserializeObject<PageListDepartamentoEmpleado>(apiResponse).DepartamentoEmpleados;

            //                response.Headers.TryGetValues("x-pagination", out var headerInfo);

            //                foreach (var item in headerInfo)
            //                {

            //                }
            //                //var pagedInfo = JsonConvert.DeserializeObject<PagedList<DepartamentoEmpleadoDto>>(headerInfo);

            //                return departamentoEmpleados;
            //            }

            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug.WriteLine(ex.Message);
            //    }
            //    return null;

            //}
        }

        public Task AddDepartamentoEmpleado(DepartamentoEmpleadoDto departamentoEmpleado)
        {
            throw new NotImplementedException();
        }

        public Task DeleteDepartamentoEmpleado(DepartamentoEmpleadoDto departamentoEmpleado)
        {
            throw new NotImplementedException();
        }

        public Task<DepartamentoEmpleadoDto> GetDepartamentoEmpleado(int id)
        {
            throw new NotImplementedException();
        }



        public Task UpdateDepartamentoEmpleado(DepartamentoEmpleadoDto departamentoEmpleado)
        {
            throw new NotImplementedException();
        }
    }
}
