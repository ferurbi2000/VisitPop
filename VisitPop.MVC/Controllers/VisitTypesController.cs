using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitType;
using VisitPop.MVC.Components;
using VisitPop.MVC.Models.ViewModels;
using VisitPop.MVC.Services.VisitType;

namespace VisitPop.MVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class VisitTypesController : Controller
    {
        private IVisitTypeRepository _visitTypeRepo;

        public VisitTypesController(IVisitTypeRepository visitTypeRepo)
        {
            _visitTypeRepo = visitTypeRepo ??
                throw new ArgumentNullException(nameof(visitTypeRepo));
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, String filters = "", String sortOrder = "")
        {
            ViewBag.pageSize = pageSize;
            ViewBag.filter = filters;

            ViewData["IdSortParm"] = sortOrder == "Id" ? "-Id" : "Id";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "-Name" : "Name";

            VisitTypeParametersDto visitTypeParameters = new VisitTypeParametersDto()
            {
                PageNumber = page,
                PageSize = pageSize,
                SortOrder = sortOrder,
                Filters = filters
            };

            var pagingResponse = await _visitTypeRepo.GetVisitTypesAsync(visitTypeParameters);

            return View(pagingResponse);
        }

        public IActionResult Create(String returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Request.Headers["Referer"].ToString();

            return View("Edit", VisitTypeViewModelFactory.Create(new VisitTypeDto(), returnUrl));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] VisitTypeViewModel visitTypeVM)
        {
            if (ModelState.IsValid)
            {
                var newVisitType = await _visitTypeRepo.AddVisitType(visitTypeVM.VisitType);

                TempData["message"] = "Your data has been submitted successfully.";
                TempData["toasterType"] = ToasterType.success;

                //return RedirectToAction(nameof(Index));

                return RedirectToAction(nameof(Edit), new { id = newVisitType.Id, returnUrl = visitTypeVM.ReturnUrl });

            }

            TempData["message"] = "A problem has been ocurred while submitting your data.";
            TempData["toasterType"] = ToasterType.info;

            return View("Edit", VisitTypeViewModelFactory.Create(visitTypeVM.VisitType, visitTypeVM.ReturnUrl));
        }

        public async Task<IActionResult> Details(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var visitType = await _visitTypeRepo.GetVisitType(id);
            VisitTypeViewModel companyVM = VisitTypeViewModelFactory.Details(visitType, returnUrl);

            return View("Edit", companyVM);
        }

        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            var visitType = await _visitTypeRepo.GetVisitType(id);
            VisitTypeViewModel visitTypeVM = VisitTypeViewModelFactory.Edit(visitType, returnUrl);

            return View("Edit", visitTypeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] VisitTypeViewModel visitTypeVM)
        {
            if (ModelState.IsValid)
            {
                await _visitTypeRepo.UpdateVisitType(visitTypeVM.VisitType);

                TempData["message"] = "Your data has been updated successfully.";
                TempData["toasterType"] = ToasterType.success;

                return RedirectToAction(nameof(Edit), new { id = visitTypeVM.VisitType.Id, returnUrl = visitTypeVM.ReturnUrl });
            }
            else
            {
                TempData["message"] = "A problem has been ocurred while updating your data.";
                TempData["toasterType"] = ToasterType.info;
            }

            return View("Edit", VisitTypeViewModelFactory.Edit(visitTypeVM.VisitType, visitTypeVM.ReturnUrl));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var vistiType = await _visitTypeRepo.GetVisitType(id);
            VisitTypeViewModel visitTypeVM = VisitTypeViewModelFactory.Delete(vistiType, returnUrl);

            return View("Edit", visitTypeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VisitTypeViewModel visitTypeVM)
        {
            await _visitTypeRepo.DeleteVisitType(visitTypeVM.VisitType);

            TempData["message"] = "Your data has been deleted successfully.";
            TempData["toasterType"] = ToasterType.success;

            //return RedirectToAction(nameof(Index));

            return Redirect(visitTypeVM.ReturnUrl);
        }
    }
}
