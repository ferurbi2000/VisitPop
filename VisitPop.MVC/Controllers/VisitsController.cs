using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Employee;
using VisitPop.Application.Dtos.Office;
using VisitPop.Application.Dtos.RegisterControl;
using VisitPop.Application.Dtos.Visit;
using VisitPop.Application.Dtos.VisitState;
using VisitPop.Application.Dtos.VisitType;
using VisitPop.MVC.Components;
using VisitPop.MVC.Models.ViewModels;
using VisitPop.MVC.Services.Visit;

namespace VisitPop.MVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class VisitsController : Controller
    {
        private IVisitRepository _visitRepo;

        private IEnumerable<VisitTypeDto> VisitTypes => GetVisitTypes();
        private IEnumerable<EmployeeDto> Employees => GetEmployees();
        private IEnumerable<OfficeDto> Offices => GetOffices();
        private IEnumerable<RegisterControlDto> RegisterControls => GetRegisterControls();
        private IEnumerable<VisitStateDto> VisitStates => GetVisitStates();


        public VisitsController(IVisitRepository visitRepo)
        {
            _visitRepo = visitRepo ??
                throw new ArgumentNullException(nameof(visitRepo));
        }

        private IEnumerable<VisitTypeDto> GetVisitTypes()
        {
            var result = Task.Run(async () =>
            await _visitRepo.GetVisitTypesAsync(
                new VisitTypeParametersDto
                {
                    PageSize = 1000,
                    Filters = "isActive==true",
                    SortOrder = "Name"
                })).ConfigureAwait(true);

            var response = result.GetAwaiter().GetResult();

            return response.Items;
        }

        private IEnumerable<EmployeeDto> GetEmployees()
        {
            var result = Task.Run(async () =>
            await _visitRepo.GetEmployeesAsync(
                new EmployeeParametersDto
                {
                    PageSize = 1000,
                    Filters = "isActive==true",
                    SortOrder = "FirstName"
                })).ConfigureAwait(true);

            var response = result.GetAwaiter().GetResult();

            return response.Items;
        }

        private IEnumerable<OfficeDto> GetOffices()
        {
            var result = Task.Run(async () =>
            await _visitRepo.GetOfficesAsync(
                new OfficeParametersDto
                {
                    PageSize = 1000,
                    Filters = "isActive==true",
                    SortOrder = "Name"
                })).ConfigureAwait(true);

            var response = result.GetAwaiter().GetResult();

            return response.Items;
        }

        private IEnumerable<RegisterControlDto> GetRegisterControls()
        {
            var result = Task.Run(async () =>
            await _visitRepo.GetRegisterControlsAsync(
                new RegisterControlParametersDto
                {
                    PageSize = 1000,
                    Filters = "isActive==true",
                    SortOrder = "Name"
                })).ConfigureAwait(true);

            var response = result.GetAwaiter().GetResult();

            return response.Items;
        }

        private IEnumerable<VisitStateDto> GetVisitStates()
        {
            var result = Task.Run(async () =>
            await _visitRepo.GetVisitStatesAsync(
                new VisitStateParametersDto
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
            ViewData["ReasonSortParm"] = sortOrder == "Reason" ? "-Reason" : "Reason";
            ViewData["CompanySortParm"] = sortOrder == "Company" ? "-Company" : "Company";
            //ViewData["EmailAddressSortParm"] = sortOrder == "EmailAddress" ? "-EmailAddress" : "EmailAddress";

            VisitParametersDto visitParameters = new VisitParametersDto()
            {
                PageNumber = page,
                PageSize = pageSize,
                SortOrder = sortOrder,
                Filters = filters
            };

            var pagingResponse = await _visitRepo.GetVisitsAsync(visitParameters);

            return View(pagingResponse);
        }

        public IActionResult Create(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            return View("Edit", VisitViewModelFactory.Create(new VisitDto(), returnUrl,
                VisitTypes,
                Employees,
                Offices,
                RegisterControls,
                VisitStates));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] VisitViewModel visitVm)
        {
            if (ModelState.IsValid)
            {
                visitVm.VisitTypes = default;
                visitVm.Employees = default;
                visitVm.Offices = default;
                visitVm.RegisterControls = default;
                visitVm.VisitStates = default;


                var newVisit = await _visitRepo.AddVisit(visitVm.Visit);

                TempData["message"] = "Your data has been submitted successfully.";
                TempData["toasterType"] = ToasterType.success;

                //return RedirectToAction(nameof(Index));

                return RedirectToAction(nameof(Edit), new { id = newVisit.Id, returnUrl = visitVm.ReturnUrl });
            }

            TempData["message"] = "A problem has been ocurred while submitting your data.";
            TempData["toasterType"] = ToasterType.info;

            return View("Edit", VisitViewModelFactory.Create(visitVm.Visit, visitVm.ReturnUrl,
                VisitTypes,
                Employees,
                Offices,
                RegisterControls,
                VisitStates));
        }

        public async Task<IActionResult> Details(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var visit = await _visitRepo.GetVisit(id);
            VisitViewModel visitVm = VisitViewModelFactory.Details(visit, returnUrl,
                VisitTypes,
                Employees,
                Offices,
                RegisterControls,
                VisitStates);

            return View("Edit", visitVm);
        }

        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            var visit = await _visitRepo.GetVisit(id);
            VisitViewModel visitVm = VisitViewModelFactory.Edit(visit, returnUrl,
                VisitTypes,
                Employees,
                Offices,
                RegisterControls,
                VisitStates);

            return View("Edit", visitVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] VisitViewModel visitVM)
        {
            if (ModelState.IsValid)
            {
                await _visitRepo.UpdateVisit(visitVM.Visit);

                TempData["message"] = "Your data has been updated successfully.";
                TempData["toasterType"] = ToasterType.success;

                return RedirectToAction(nameof(Edit), new { id = visitVM.Visit.Id, returnUrl = visitVM.ReturnUrl });
            }
            else
            {
                TempData["message"] = "A problem has been ocurred while updating your data.";
                TempData["toasterType"] = ToasterType.info;
            }

            return View("Edit", VisitViewModelFactory.Edit(visitVM.Visit, visitVM.ReturnUrl,
                VisitTypes,
                Employees,
                Offices,
                RegisterControls,
                VisitStates));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var visit = await _visitRepo.GetVisit(id);
            VisitViewModel visitVm = VisitViewModelFactory.Delete(visit, returnUrl,
                VisitTypes,
                Employees,
                Offices,
                RegisterControls,
                VisitStates);

            return View("Edit", visitVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VisitViewModel visitVM)
        {
            await _visitRepo.DeleteVisit(visitVM.Visit);

            TempData["message"] = "Your data has been deleted successfully.";
            TempData["toasterType"] = ToasterType.success;

            //return RedirectToAction(nameof(Index));

            return Redirect(visitVM.ReturnUrl);
        }
    }
}
