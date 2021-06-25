using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.Application.Responses;
using VisitPop.Application.Wrappers;
using VisitPop.MVC.Features;

namespace VisitPop.MVC.Services.Employee
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string WebAPIUrl, WebAPIUrlDepartment;
        private readonly Uri uri, uriDepartment;

        public EmployeeRepository()
        {
            WebAPIUrl = "https://localhost:5001/api/Employees/";
            uri = new Uri(WebAPIUrl);

            WebAPIUrlDepartment = "https://localhost:5001/api/EmployeeDepartments/";
            uriDepartment = new Uri(WebAPIUrlDepartment);
        }

        public async Task<PagingResponse<EmployeeDepartmentDto>> GetEmployeeDepartmentsAsync(EmployeeDepartmentParametersDto employeeDepartmentParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = employeeDepartmentParameters.PageNumber.ToString(),
                ["pageSize"] = employeeDepartmentParameters.PageSize.ToString(),
                ["sortOrder"] = employeeDepartmentParameters.SortOrder.ToString(),
                ["filters"] = employeeDepartmentParameters.Filters.ToString()                               
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uriDepartment.ToString(), queryStringParam)))
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

        public async Task<PagingResponse<EmployeeDto>> GetEmployeesAsync(EmployeeParametersDto employeeParameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = employeeParameters.PageNumber.ToString(),
                ["pageSize"] = employeeParameters.PageSize.ToString(),
                ["sortOrder"] = employeeParameters.SortOrder.ToString(),
                ["filters"] = String.IsNullOrEmpty(employeeParameters.Filters) ? "" : $"(FirstName|LastName|EmailAddress)@=* {employeeParameters.Filters}"
            };

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(QueryHelpers.AddQueryString(uri.ToString(), queryStringParam)))
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

        public async Task<EmployeeDto> GetEmployee(int id)
        {
            EmployeeDto employee = new EmployeeDto();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync(uri.AbsoluteUri + id))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        employee = JsonConvert.DeserializeObject<EmployeeResponseDto>(apiResponse).Employee;
                    }
                }
            }

            return employee;
        }

        public async Task<EmployeeDto> AddEmployee(EmployeeDto employee)
        {
            EmployeeDto receivedEmployee = new EmployeeDto();

            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync(uri.AbsoluteUri, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedEmployee = JsonConvert.DeserializeObject<EmployeeResponseDto>(apiResponse).Employee;
                }
            }
            return receivedEmployee;
        }

        public async Task UpdateEmployee(EmployeeDto employee)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(employee), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PutAsync(uri.AbsoluteUri + employee.Id, content))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception();
                    }
                }
            }
        }

        public async Task DeleteEmployee(EmployeeDto employee)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.DeleteAsync(uri.AbsoluteUri + employee.Id))
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
