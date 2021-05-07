using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.Application.Responses;
using VisitPop.Application.Wrappers;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.EmployeeDepartment
{
    public class EmployeeDepartmentRepository : IEmployeeDepartmentRepository
    {
        private readonly string WebAPIUrl;
        private readonly Uri uri;

        public EmployeeDepartmentRepository()
        {
            WebAPIUrl = "https://localhost:5001/api/EmployeeDepartments/";
            uri = new Uri(WebAPIUrl);
        }

        //public async Task<IEnumerable<DepartamentoEmpleadoDto>> GetDepartamentoEmpleadosAsync(int pageNumber, int pageSize)
        public async Task<PagingResponse<EmployeeDepartmentDto>> GetEmployeeDepartmentsAsync(EmployeeDepartmentParametersDto employeeDepartmentParameters)
        {

            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = employeeDepartmentParameters.PageNumber.ToString(),
                ["pageSize"] = employeeDepartmentParameters.PageSize.ToString(),
                ["sortOrder"] = employeeDepartmentParameters.SortOrder.ToString(),
                ["filters"] = String.IsNullOrEmpty(employeeDepartmentParameters.Filters) ? "" : $"Name @=* {employeeDepartmentParameters.Filters}"
            };



            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uri.ToString(), queryStringParam)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        var pagingResponse = new PagingResponse<EmployeeDepartmentDto>
                        {
                            Items = JsonConvert.DeserializeObject<PageListEmployeeDepartment>(content).EmployeeDepartments,
                            Metadata = JsonConvert.DeserializeObject<MetaData>(response.Headers.GetValues("x-pagination").First())
                        };

                        pagingResponse.Filters = employeeDepartmentParameters.Filters;
                        pagingResponse.SortOrder = employeeDepartmentParameters.SortOrder;
                        return pagingResponse;

                    }
                    return null;
                }
            }
        }

        public async Task<EmployeeDepartmentDto> GetEmployeeDepartment(int id)
        {

            EmployeeDepartmentDto departamento = new EmployeeDepartmentDto();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri.AbsoluteUri + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        departamento = JsonConvert.DeserializeObject<EmployeeDepartmentResponseDto>(apiResponse).EmployeeDepartment;
                    }
                }
            }

            return departamento;
        }

        public async Task<EmployeeDepartmentDto> AddEmployeeDepartment(EmployeeDepartmentDto employeeDepartment)
        {

            EmployeeDepartmentDto receivedEmployeeDepartment = new EmployeeDepartmentDto();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(employeeDepartment), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri.AbsoluteUri, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {

                        throw new Exception();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedEmployeeDepartment = JsonConvert.DeserializeObject<EmployeeDepartmentResponseDto>(apiResponse).EmployeeDepartment;
                }
            }

            return receivedEmployeeDepartment;
        }

        public async Task UpdateEmployeeDepartment(EmployeeDepartmentDto employeeDepartment)
        {

            using (var httpClient = new HttpClient())
            {

                StringContent content = new StringContent(JsonConvert.SerializeObject(employeeDepartment), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(uri.AbsoluteUri + employeeDepartment.Id, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                }
            }
        }



        public async Task DeleteEmployeeDepartment(EmployeeDepartmentDto employeeDepartment)
        {


            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(uri.AbsoluteUri + employeeDepartment.Id))
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
