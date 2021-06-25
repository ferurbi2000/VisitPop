using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.Application.Interfaces.EmployeeDepartment;
using VisitPop.Domain.Entities;
using VisitPop.MVC.Components;
using VisitPop.MVC.Features;
using VisitPop.MVC.Models.ViewModels;
using VisitPop.MVC.Services.Employee;
//using VisitPop.Application.Interfaces.Employee;

namespace VisitPop.MVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class EmployeesController : Controller
    {
        private IEmployeeRepository _employeeRepo;

        private IEnumerable<EmployeeDepartmentDto> EmployeeDepartments => GetEmployeeDepartments();


        public EmployeesController(IEmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo ??
                throw new ArgumentNullException(nameof(employeeRepo));
        }

        private IEnumerable<EmployeeDepartmentDto> GetEmployeeDepartments()
        {
            var result = Task.Run(async () =>
            await _employeeRepo.GetEmployeeDepartmentsAsync(
                new EmployeeDepartmentParametersDto
                {
                    PageSize = 1000,
                    Filters = "isActive==true",
                    SortOrder = "Name"
                })).ConfigureAwait(true);

            var response = result.GetAwaiter().GetResult();

            return response.Items;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string filters = "", string sortOrder = "")
        {
            ViewBag.pageSize = pageSize;
            ViewBag.filter = filters;

            ViewData["IdSortParm"] = sortOrder == "Id" ? "-Id" : "Id";
            ViewData["FirstNameSortParm"] = sortOrder == "FirstName" ? "-FirstName" : "FirstName";
            ViewData["LastNameSortParm"] = sortOrder == "LastName" ? "-LastName" : "LastName";
            ViewData["EmailAddressSortParm"] = sortOrder == "EmailAddress" ? "-EmailAddress" : "EmailAddress";

            EmployeeParametersDto employeeParameters = new EmployeeParametersDto()
            {
                PageNumber = page,
                PageSize = pageSize,
                SortOrder = sortOrder,
                Filters = filters
            };

            var pagingResponse = await _employeeRepo.GetEmployeesAsync(employeeParameters);

            return View(pagingResponse);
        }

        public IActionResult Create(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            return View("Edit", EmployeeViewModelFactory.Create(new EmployeeDto(), returnUrl, EmployeeDepartments));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EmployeeViewModel employeeVm)
        {
            if (ModelState.IsValid)
            {
                employeeVm.EmployeeDepartment = default;


                var newEmployee = await _employeeRepo.AddEmployee(employeeVm.Employee);

                TempData["message"] = "Your data has been submitted successfully.";
                TempData["toasterType"] = ToasterType.success;

                //return RedirectToAction(nameof(Index));

                return RedirectToAction(nameof(Edit), new { id = newEmployee.Id, returnUrl = employeeVm.ReturnUrl });
            }

            TempData["message"] = "A problem has been ocurred while submitting your data.";
            TempData["toasterType"] = ToasterType.info;

            return View("Edit", EmployeeViewModelFactory.Create(employeeVm.Employee, employeeVm.ReturnUrl, EmployeeDepartments));
        }

        public async Task<IActionResult> Details(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var employee = await _employeeRepo.GetEmployee(id);
            EmployeeViewModel employeeVm = EmployeeViewModelFactory.Details(employee, returnUrl, EmployeeDepartments);

            return View("Edit", employeeVm);
        }

        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            var employee = await _employeeRepo.GetEmployee(id);
            EmployeeViewModel employeeVm = EmployeeViewModelFactory.Edit(employee, returnUrl, EmployeeDepartments);

            return View("Edit", employeeVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)
            {
                await _employeeRepo.UpdateEmployee(employeeVM.Employee);

                TempData["message"] = "Your data has been updated successfully.";
                TempData["toasterType"] = ToasterType.success;

                return RedirectToAction(nameof(Edit), new { id = employeeVM.Employee.Id, returnUrl = employeeVM.ReturnUrl });
            }
            else
            {
                TempData["message"] = "A problem has been ocurred while updating your data.";
                TempData["toasterType"] = ToasterType.info;
            }

            return View("Edit", EmployeeViewModelFactory.Edit(employeeVM.Employee, employeeVM.ReturnUrl, EmployeeDepartments));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var employee = await _employeeRepo.GetEmployee(id);
            EmployeeViewModel employeeVm = EmployeeViewModelFactory.Delete(employee, returnUrl, EmployeeDepartments);

            return View("Edit", employeeVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM)
        {
            await _employeeRepo.DeleteEmployee(employeeVM.Employee);

            TempData["message"] = "Your data has been deleted successfully.";
            TempData["toasterType"] = ToasterType.success;

            //return RedirectToAction(nameof(Index));

            return Redirect(employeeVM.ReturnUrl);
        }
    }
}
