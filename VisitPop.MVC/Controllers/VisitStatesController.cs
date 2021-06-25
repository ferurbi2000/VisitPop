using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VisitState;
using VisitPop.MVC.Components;
using VisitPop.MVC.Models.ViewModels;
using VisitPop.MVC.Services.VisitState;

namespace VisitPop.MVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class VisitStatesController : Controller
    {
        IVisitStateRepository _visitStateRepo;

        public VisitStatesController(IVisitStateRepository visitStateRepo)
        {
            _visitStateRepo = visitStateRepo ??
                throw new ArgumentNullException(nameof(visitStateRepo));
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, String filters = "", String sortOrder = "")
        {
            ViewBag.pageSize = pageSize;
            ViewBag.filter = filters;

            ViewData["IdSortParm"] = sortOrder == "Id" ? "-Id" : "Id";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "-Name" : "Name";

            VisitStateParametersDto companyParameters = new VisitStateParametersDto()
            {
                PageNumber = page,
                PageSize = pageSize,
                SortOrder = sortOrder,
                Filters = filters
            };

            var pagingResponse = await _visitStateRepo.GetVisitStatesAsync(companyParameters);

            return View(pagingResponse);
        }

        public IActionResult Create(String returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Request.Headers["Referer"].ToString();

            return View("Edit", VisitStateViewModelFactory.Create(new VisitStateDto(), returnUrl));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] VisitStateViewModel visitStateVM)
        {
            if (ModelState.IsValid)
            {
                var newVisitState = await _visitStateRepo.AddVisitState(visitStateVM.VisitState);

                TempData["message"] = "Your data has been submitted successfully.";
                TempData["toasterType"] = ToasterType.success;

                //return RedirectToAction(nameof(Index));

                return RedirectToAction(nameof(Edit), new { id = newVisitState.Id, returnUrl = visitStateVM.ReturnUrl });

            }

            TempData["message"] = "A problem has been ocurred while submitting your data.";
            TempData["toasterType"] = ToasterType.info;

            return View("Edit", VisitStateViewModelFactory.Create(visitStateVM.VisitState, visitStateVM.ReturnUrl));
        }

        public async Task<IActionResult> Details(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var visitState = await _visitStateRepo.GetVisitState(id);
            VisitStateViewModel visitStateVM = VisitStateViewModelFactory.Details(visitState, returnUrl);

            return View("Edit", visitStateVM);
        }

        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            var visitState = await _visitStateRepo.GetVisitState(id);
            VisitStateViewModel visitStateVM = VisitStateViewModelFactory.Edit(visitState, returnUrl);

            return View("Edit", visitStateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] VisitStateViewModel visitStateVM)
        {
            if (ModelState.IsValid)
            {
                await _visitStateRepo.UpdateVisitState(visitStateVM.VisitState);

                TempData["message"] = "Your data has been updated successfully.";
                TempData["toasterType"] = ToasterType.success;

                return RedirectToAction(nameof(Edit), new { id = visitStateVM.VisitState.Id, returnUrl = visitStateVM.ReturnUrl });
            }
            else
            {
                TempData["message"] = "A problem has been ocurred while updating your data.";
                TempData["toasterType"] = ToasterType.info;
            }

            return View("Edit", VisitStateViewModelFactory.Edit(visitStateVM.VisitState, visitStateVM.ReturnUrl));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var visitState = await _visitStateRepo.GetVisitState(id);
            VisitStateViewModel visitStateVM = VisitStateViewModelFactory.Delete(visitState, returnUrl);

            return View("Edit", visitStateVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VisitStateViewModel visitStateVM)
        {
            await _visitStateRepo.DeleteVisitState(visitStateVM.VisitState);

            TempData["message"] = "Your data has been deleted successfully.";
            TempData["toasterType"] = ToasterType.success;

            //return RedirectToAction(nameof(Index));

            return Redirect(visitStateVM.ReturnUrl);
        }
    }
}
