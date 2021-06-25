using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Company;
using VisitPop.MVC.Components;
using VisitPop.MVC.Models.ViewModels;
using VisitPop.MVC.Services.Company;

namespace VisitPop.MVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class CompaniesController : Controller
    {
        private ICompanyRepository _companyRepo;

        public CompaniesController(ICompanyRepository companyRepo)
        {
            _companyRepo = companyRepo ??
                throw new ArgumentNullException(nameof(companyRepo));
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, String filters = "", String sortOrder = "")
        {
            ViewBag.pageSize = pageSize;
            ViewBag.filter = filters;

            ViewData["IdSortParm"] = sortOrder == "Id" ? "-Id" : "Id";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "-Name" : "Name";

            CompanyParametersDto companyParameters = new CompanyParametersDto()
            {
                PageNumber = page,
                PageSize = pageSize,
                SortOrder = sortOrder,
                Filters = filters
            };

            var pagingResponse = await _companyRepo.GetCompaniesAsync(companyParameters);

            return View(pagingResponse);
        }

        public IActionResult Create(String returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Request.Headers["Referer"].ToString();

            return View("Edit", CompanyViewModelFactory.Create(new CompanyDto(), returnUrl));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CompanyViewModel companyVM)
        {
            if (ModelState.IsValid)
            {
                var newCompany = await _companyRepo.AddCompany(companyVM.Company);

                TempData["message"] = "Your data has been submitted successfully.";
                TempData["toasterType"] = ToasterType.success;

                //return RedirectToAction(nameof(Index));

                return RedirectToAction(nameof(Edit), new { id = newCompany.Id, returnUrl = companyVM.ReturnUrl });

            }

            TempData["message"] = "A problem has been ocurred while submitting your data.";
            TempData["toasterType"] = ToasterType.info;

            return View("Edit", CompanyViewModelFactory.Create(companyVM.Company, companyVM.ReturnUrl));
        }

        public async Task<IActionResult> Details(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var company = await _companyRepo.GetCompany(id);
            CompanyViewModel companyVM = CompanyViewModelFactory.Details(company, returnUrl);

            return View("Edit", companyVM);
        }

        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            var company = await _companyRepo.GetCompany(id);
            CompanyViewModel companyVM = CompanyViewModelFactory.Edit(company, returnUrl);

            return View("Edit", companyVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] CompanyViewModel companyVM)
        {
            if (ModelState.IsValid)
            {
                await _companyRepo.UpdateCompany(companyVM.Company);

                TempData["message"] = "Your data has been updated successfully.";
                TempData["toasterType"] = ToasterType.success;

                return RedirectToAction(nameof(Edit), new { id = companyVM.Company.Id, returnUrl = companyVM.ReturnUrl });
            }
            else
            {
                TempData["message"] = "A problem has been ocurred while updating your data.";
                TempData["toasterType"] = ToasterType.info;
            }

            return View("Edit", CompanyViewModelFactory.Edit(companyVM.Company, companyVM.ReturnUrl));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var company = await _companyRepo.GetCompany(id);
            CompanyViewModel companyVM = CompanyViewModelFactory.Delete(company, returnUrl);

            return View("Edit", companyVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CompanyViewModel companyVM)
        {
            await _companyRepo.DeleteCompany(companyVM.Company);

            TempData["message"] = "Your data has been deleted successfully.";
            TempData["toasterType"] = ToasterType.success;

            //return RedirectToAction(nameof(Index));

            return Redirect(companyVM.ReturnUrl);
        }
    }
}
