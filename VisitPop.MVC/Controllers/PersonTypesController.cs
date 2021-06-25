using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.PersonType;
using VisitPop.MVC.Components;
using VisitPop.MVC.Models.ViewModels;
using VisitPop.MVC.Services.PersonType;

namespace VisitPop.MVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class PersonTypesController : Controller
    {
        private IPersonTypeRepository _personTypeRepo;

        public PersonTypesController(IPersonTypeRepository personType)
        {
            _personTypeRepo = personType ??
               throw new ArgumentNullException(nameof(personType));
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, String filters = "", String sortOrder = "")
        {
            ViewBag.pageSize = pageSize;
            ViewBag.filter = filters;

            ViewData["IdSortParm"] = sortOrder == "Id" ? "-Id" : "Id";
            ViewData["NameSortParm"] = sortOrder == "Name" ? "-Name" : "Name";

            PersonTypeParametersDto personTypeParameters = new PersonTypeParametersDto()
            {
                PageNumber = page,
                PageSize = pageSize,
                SortOrder = sortOrder,
                Filters = filters
            };

            var pagingResponse = await _personTypeRepo.GetPersonTypesAsync(personTypeParameters);

            return View(pagingResponse);
        }

        public IActionResult Create(String returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Request.Headers["Referer"].ToString();

            return View("Edit", PersonTypeViewModelFactory.Create(new PersonTypeDto(), returnUrl));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PersonTypeViewModel personTypeVM)
        {
            if (ModelState.IsValid)
            {
                var newPersonType = await _personTypeRepo.AddPersonType(personTypeVM.PersonType);

                TempData["message"] = "Your data has been submitted successfully.";
                TempData["toasterType"] = ToasterType.success;

                //return RedirectToAction(nameof(Index));

                return RedirectToAction(nameof(Edit), new { id = newPersonType.Id, returnUrl = personTypeVM.ReturnUrl });

            }

            TempData["message"] = "A problem has been ocurred while submitting your data.";
            TempData["toasterType"] = ToasterType.info;

            return View("Edit", PersonTypeViewModelFactory.Create(personTypeVM.PersonType, personTypeVM.ReturnUrl));
        }

        public async Task<IActionResult> Details(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var personType = await _personTypeRepo.GetPersonType(id);
            PersonTypeViewModel personTypeVM = PersonTypeViewModelFactory.Details(personType, returnUrl);

            return View("Edit", personTypeVM);
        }

        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            var personType = await _personTypeRepo.GetPersonType(id);
            PersonTypeViewModel personTypeVM = PersonTypeViewModelFactory.Edit(personType, returnUrl);

            return View("Edit", personTypeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] PersonTypeViewModel personTypeVM)
        {
            if (ModelState.IsValid)
            {
                await _personTypeRepo.UpdatePersonType(personTypeVM.PersonType);

                TempData["message"] = "Your data has been updated successfully.";
                TempData["toasterType"] = ToasterType.success;

                return RedirectToAction(nameof(Edit), new { id = personTypeVM.PersonType.Id, returnUrl = personTypeVM.ReturnUrl });
            }
            else
            {
                TempData["message"] = "A problem has been ocurred while updating your data.";
                TempData["toasterType"] = ToasterType.info;
            }

            return View("Edit", PersonTypeViewModelFactory.Edit(personTypeVM.PersonType, personTypeVM.ReturnUrl));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var personType = await _personTypeRepo.GetPersonType(id);
            PersonTypeViewModel personTypeVM = PersonTypeViewModelFactory.Delete(personType, returnUrl);

            return View("Edit", personTypeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PersonTypeViewModel personTypeVM)
        {
            await _personTypeRepo.DeletePersonType(personTypeVM.PersonType);

            TempData["message"] = "Your data has been deleted successfully.";
            TempData["toasterType"] = ToasterType.success;

            //return RedirectToAction(nameof(Index));

            return Redirect(personTypeVM.ReturnUrl);
        }
    }
}
