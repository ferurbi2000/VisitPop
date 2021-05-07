using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.EmployeeDepartment;
using VisitPop.MVC.Components;
using VisitPop.MVC.Models.ViewModels;
using VisitPop.MVC.Services.EmployeeDepartment;

namespace VisitPop.MVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class EmployeeDepartmentsController : Controller
    {
        private IEmployeeDepartmentRepository _context;

        public EmployeeDepartmentsController(IEmployeeDepartmentRepository context)
        {
            _context = context;

        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string filters = "", string sortOrder = "")
        {

            ViewBag.pageSize = pageSize;
            ViewBag.filter = filters;

            ViewData["IdSortParm"] = sortOrder == "Id" ? "-Id" : "Id";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "-Name" : "Name";

            EmployeeDepartmentParametersDto employeeDepartmentParameters = new EmployeeDepartmentParametersDto() { PageNumber = page, PageSize = pageSize, SortOrder = sortOrder, Filters = filters };
            var pagingResponse = await _context.GetEmployeeDepartmentsAsync(employeeDepartmentParameters);

            return View(pagingResponse);
        }

        public IActionResult Create()
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            return View("Edit", EmployeeDepartmentViewModelFactory.Create(new EmployeeDepartmentDto(), returnUrl));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] EmployeeDepartmentViewModel employeeDepartmentVM)
        {
            if (ModelState.IsValid)
            {
                var newEmployeeDepartment = await _context.AddEmployeeDepartment(employeeDepartmentVM.EmployeeDepartment);

                TempData["message"] = "Your data has been submitted successfully.";
                TempData["toasterType"] = ToasterType.success;

                //return RedirectToAction(nameof(Index));

                return RedirectToAction(nameof(Edit), new { id = newEmployeeDepartment.Id, returnUrl = employeeDepartmentVM.ReturnUrl });
            }

            TempData["message"] = "A problem has been ocurred while submitting your data.";
            TempData["toasterType"] = ToasterType.info;

            return View("Edit", EmployeeDepartmentViewModelFactory.Create(employeeDepartmentVM.EmployeeDepartment, employeeDepartmentVM.ReturnUrl));
        }

        public async Task<IActionResult> Details(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var employeeDepartment = await _context.GetEmployeeDepartment(id);
            EmployeeDepartmentViewModel department = EmployeeDepartmentViewModelFactory.Details(employeeDepartment, returnUrl);

            return View("Edit", department);
        }

        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            var employeeDepartment = await _context.GetEmployeeDepartment(id);
            EmployeeDepartmentViewModel deparment = EmployeeDepartmentViewModelFactory.Edit(employeeDepartment, returnUrl);

            return View("Edit", deparment);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] EmployeeDepartmentViewModel employeeDepartmentVM)
        {
            if (ModelState.IsValid)
            {
                await _context.UpdateEmployeeDepartment(employeeDepartmentVM.EmployeeDepartment);

                TempData["message"] = "Your data has been updated successfully.";
                TempData["toasterType"] = ToasterType.success;
            }
            else
            {
                TempData["message"] = "A problem has been ocurred while updating your data.";
                TempData["toasterType"] = ToasterType.info;
            }            

            return View("Edit", EmployeeDepartmentViewModelFactory.Edit(employeeDepartmentVM.EmployeeDepartment, employeeDepartmentVM.ReturnUrl));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var employeeDepartment = await _context.GetEmployeeDepartment(id);
            EmployeeDepartmentViewModel deparment = EmployeeDepartmentViewModelFactory.Delete(employeeDepartment, returnUrl);

            return View("Edit", deparment);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EmployeeDepartmentViewModel employeeDepartmentVM)
        {
            await _context.DeleteEmployeeDepartment(employeeDepartmentVM.EmployeeDepartment);

            TempData["message"] = "Your data has been deleted successfully.";
            TempData["toasterType"] = ToasterType.success;

            //return RedirectToAction(nameof(Index));

            return Redirect(employeeDepartmentVM.ReturnUrl);
        }
    }
}
