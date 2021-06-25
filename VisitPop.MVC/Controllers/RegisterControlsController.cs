using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.RegisterControl;
using VisitPop.MVC.Components;
using VisitPop.MVC.Models.ViewModels;
using VisitPop.MVC.Services.RegisterControl;

namespace VisitPop.MVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class RegisterControlsController : Controller
    {
        private IRegisterControlRepository _registerControlRepo;

        public RegisterControlsController(IRegisterControlRepository registerControlRepo)
        {
            _registerControlRepo = registerControlRepo ??
                throw new ArgumentNullException(nameof(registerControlRepo));
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string filters = "", string sortOrder = "")
        {
            ViewBag.pageSize = pageSize;
            ViewBag.filter = filters;

            ViewData["IdSortParm"] = sortOrder == "Id" ? "-Id" : "Id";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "-Name" : "Name";
            ViewData["LocationSortParm"] = sortOrder == "Location" ? "-Location" : "Location";

            RegisterControlParametersDto registerControlParameters = new RegisterControlParametersDto()
            {
                PageNumber = page,
                PageSize = pageSize,
                SortOrder = sortOrder,
                Filters = filters
            };

            var pagingResponse = await _registerControlRepo.GetRegisterControlsAsync(registerControlParameters);

            return View(pagingResponse);
        }

        public IActionResult Create(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            return View("Edit", RegisterControlViewModelFactory.Create(new RegisterControlDto(), returnUrl));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RegisterControlViewModel registerControlVm)
        {
            if (ModelState.IsValid)
            {

                var newRegisterControl = await _registerControlRepo.AddRegisterControl(registerControlVm.RegisterControl);

                TempData["message"] = "Your data has been submitted successfully.";
                TempData["toasterType"] = ToasterType.success;

                //return RedirectToAction(nameof(Index));

                return RedirectToAction(nameof(Edit), new { id = newRegisterControl.Id, returnUrl = registerControlVm.ReturnUrl });
            }

            TempData["message"] = "A problem has been ocurred while submitting your data.";
            TempData["toasterType"] = ToasterType.info;

            return View("Edit", RegisterControlViewModelFactory.Create(registerControlVm.RegisterControl, registerControlVm.ReturnUrl));
        }

        public async Task<IActionResult> Details(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var registerControl = await _registerControlRepo.GetRegisterControl(id);
            RegisterControlViewModel registerControlVm = RegisterControlViewModelFactory.Details(registerControl, returnUrl);

            return View("Edit", registerControlVm);
        }

        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            var registerControl = await _registerControlRepo.GetRegisterControl(id);
            RegisterControlViewModel registerControlVm = RegisterControlViewModelFactory.Edit(registerControl, returnUrl);

            return View("Edit", registerControlVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] RegisterControlViewModel registerControlVM)
        {
            if (ModelState.IsValid)
            {
                await _registerControlRepo.UpdateRegisterControl(registerControlVM.RegisterControl);

                TempData["message"] = "Your data has been updated successfully.";
                TempData["toasterType"] = ToasterType.success;

                return RedirectToAction(nameof(Edit), new { id = registerControlVM.RegisterControl.Id, returnUrl = registerControlVM.ReturnUrl });
            }
            else
            {
                TempData["message"] = "A problem has been ocurred while updating your data.";
                TempData["toasterType"] = ToasterType.info;
            }

            return View("Edit", RegisterControlViewModelFactory.Edit(registerControlVM.RegisterControl, registerControlVM.ReturnUrl));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var registerControl = await _registerControlRepo.GetRegisterControl(id);
            RegisterControlViewModel registerControlVm = RegisterControlViewModelFactory.Delete(registerControl, returnUrl);

            return View("Edit", registerControlVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegisterControlViewModel registerControlVM)
        {
            await _registerControlRepo.DeleteRegisterControl(registerControlVM.RegisterControl);

            TempData["message"] = "Your data has been deleted successfully.";
            TempData["toasterType"] = ToasterType.success;

            //return RedirectToAction(nameof(Index));

            return Redirect(registerControlVM.ReturnUrl);
        }
    }
}
