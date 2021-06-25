using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.VehicleType;
using VisitPop.MVC.Components;
using VisitPop.MVC.Models.ViewModels;
using VisitPop.MVC.Services.VehicleType;

namespace VisitPop.MVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class VehicleTypesController : Controller
    {
        private IVehicleTypeRepository _vehicleTypeRepo;

        public VehicleTypesController(IVehicleTypeRepository vehicleType)
        {
            _vehicleTypeRepo = vehicleType ??
                throw new ArgumentNullException(nameof(vehicleType));
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, String filters = "", String sortOrder = "")
        {
            ViewBag.pageSize = pageSize;
            ViewBag.filter = filters;

            ViewData["IdSortParm"] = sortOrder == "Id" ? "-Id" : "Id";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "-Name" : "Name";

            VehicleTypeParametersDto vehicleTypeParameters = new VehicleTypeParametersDto()
            {
                PageNumber = page,
                PageSize = pageSize,
                SortOrder = sortOrder,
                Filters = filters
            };

            var pagingResponse = await _vehicleTypeRepo.GetVehicleTypesAsync(vehicleTypeParameters);

            return View(pagingResponse);
        }

        public IActionResult Create(String returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Request.Headers["Referer"].ToString();

            return View("Edit", VehicleTypeViewModelFactory.Create(new VehicleTypeDto(), returnUrl));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] VehicleTypeViewModel vehicleTypeVM)
        {
            if (ModelState.IsValid)
            {
                var newVehicleType = await _vehicleTypeRepo.AddVehicleType(vehicleTypeVM.VehicleType);

                TempData["message"] = "Your data has been submitted successfully.";
                TempData["toasterType"] = ToasterType.success;

                //return RedirectToAction(nameof(Index));

                return RedirectToAction(nameof(Edit), new { id = newVehicleType.Id, returnUrl = vehicleTypeVM.ReturnUrl });

            }

            TempData["message"] = "A problem has been ocurred while submitting your data.";
            TempData["toasterType"] = ToasterType.info;

            return View("Edit", VehicleTypeViewModelFactory.Create(vehicleTypeVM.VehicleType, vehicleTypeVM.ReturnUrl));
        }

        public async Task<IActionResult> Details(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var vehicleType = await _vehicleTypeRepo.GetVehicleType(id);
            VehicleTypeViewModel vehicleTypeVM = VehicleTypeViewModelFactory.Details(vehicleType, returnUrl);

            return View("Edit", vehicleTypeVM);
        }

        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            var vehicleType = await _vehicleTypeRepo.GetVehicleType(id);
            VehicleTypeViewModel vehicleTypeVM = VehicleTypeViewModelFactory.Edit(vehicleType, returnUrl);

            return View("Edit", vehicleTypeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] VehicleTypeViewModel vehicleTypeVM)
        {
            if (ModelState.IsValid)
            {
                await _vehicleTypeRepo.UpdateVehicleType(vehicleTypeVM.VehicleType);

                TempData["message"] = "Your data has been updated successfully.";
                TempData["toasterType"] = ToasterType.success;

                return RedirectToAction(nameof(Edit), new { id = vehicleTypeVM.VehicleType.Id, returnUrl = vehicleTypeVM.ReturnUrl });
            }
            else
            {
                TempData["message"] = "A problem has been ocurred while updating your data.";
                TempData["toasterType"] = ToasterType.info;
            }

            return View("Edit", VehicleTypeViewModelFactory.Edit(vehicleTypeVM.VehicleType, vehicleTypeVM.ReturnUrl));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var vehicleType = await _vehicleTypeRepo.GetVehicleType(id);
            VehicleTypeViewModel vehicleTypeVM = VehicleTypeViewModelFactory.Delete(vehicleType, returnUrl);

            return View("Edit", vehicleTypeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(VehicleTypeViewModel vehicleTypeVM)
        {
            await _vehicleTypeRepo.DeleteVehicleType(vehicleTypeVM.VehicleType);

            TempData["message"] = "Your data has been deleted successfully.";
            TempData["toasterType"] = ToasterType.success;

            //return RedirectToAction(nameof(Index));

            return Redirect(vehicleTypeVM.ReturnUrl);
        }
    }
}
