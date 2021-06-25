using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Office;
using VisitPop.MVC.Components;
using VisitPop.MVC.Models.ViewModels;
using VisitPop.MVC.Services.Office;

namespace VisitPop.MVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class OfficesController : Controller
    {

        private IOfficeRepository _officeRepo;

        public OfficesController(IOfficeRepository officeRepo)
        {
            _officeRepo = officeRepo ??
                throw new ArgumentNullException(nameof(officeRepo));
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, String filters = "", String sortOrder = "")
        {
            ViewBag.pageSize = pageSize;
            ViewBag.filter = filters;

            ViewData["IdSortParm"] = sortOrder == "Id" ? "-Id" : "Id";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "-Name" : "Name";

            OfficeParametersDto officeParameters = new OfficeParametersDto()
            {
                PageNumber = page,
                PageSize = pageSize,
                SortOrder = sortOrder,
                Filters = filters
            };

            var pagingResponse = await _officeRepo.GetOfficesAsync(officeParameters);

            return View(pagingResponse);
        }

        public IActionResult Create(String returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Request.Headers["Referer"].ToString();

            return View("Edit", OfficeViewModelFactory.Create(new OfficeDto(), returnUrl));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] OfficeViewModel officesVM)
        {
            if (ModelState.IsValid)
            {
                var newOffice = await _officeRepo.AddOffice(officesVM.Office);

                TempData["message"] = "Your data has been submitted successfully.";
                TempData["toasterType"] = ToasterType.success;

                //return RedirectToAction(nameof(Index));

                return RedirectToAction(nameof(Edit), new { id = newOffice.Id, returnUrl = officesVM.ReturnUrl });

            }

            TempData["message"] = "A problem has been ocurred while submitting your data.";
            TempData["toasterType"] = ToasterType.info;

            return View("Edit", OfficeViewModelFactory.Create(officesVM.Office, officesVM.ReturnUrl));
        }

        public async Task<IActionResult> Details(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var office = await _officeRepo.GetOffice(id);
            OfficeViewModel officeVM = OfficeViewModelFactory.Details(office, returnUrl);

            return View("Edit", officeVM);
        }

        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            var office = await _officeRepo.GetOffice(id);
            OfficeViewModel officeVM = OfficeViewModelFactory.Edit(office, returnUrl);

            return View("Edit", officeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] OfficeViewModel officeVM)
        {
            if (ModelState.IsValid)
            {
                await _officeRepo.UpdateOffice(officeVM.Office);

                TempData["message"] = "Your data has been updated successfully.";
                TempData["toasterType"] = ToasterType.success;

                return RedirectToAction(nameof(Edit), new { id = officeVM.Office.Id, returnUrl = officeVM.ReturnUrl });
            }
            else
            {
                TempData["message"] = "A problem has been ocurred while updating your data.";
                TempData["toasterType"] = ToasterType.info;
            }

            return View("Edit", OfficeViewModelFactory.Edit(officeVM.Office, officeVM.ReturnUrl));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var office = await _officeRepo.GetOffice(id);
            OfficeViewModel officeVM = OfficeViewModelFactory.Delete(office, returnUrl);

            return View("Edit", officeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(OfficeViewModel officeVM)
        {
            await _officeRepo.DeleteOffice(officeVM.Office);

            TempData["message"] = "Your data has been deleted successfully.";
            TempData["toasterType"] = ToasterType.success;

            //return RedirectToAction(nameof(Index));

            return Redirect(officeVM.ReturnUrl);
        }
    }
}
