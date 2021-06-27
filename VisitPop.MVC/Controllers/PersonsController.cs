using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.Company;
using VisitPop.Application.Dtos.Person;
using VisitPop.Application.Dtos.PersonType;
using VisitPop.MVC.Components;
using VisitPop.MVC.Models.ViewModels;
using VisitPop.MVC.Services.Person;

namespace VisitPop.MVC.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class PersonsController : Controller
    {
        private IPersonRepository _personRepo;

        private IEnumerable<PersonTypeDto> PersonTypes => GetPersonTypes();
        private IEnumerable<CompanyDto> Companies => GetCompanies();


        public PersonsController(IPersonRepository personRepo)
        {
            _personRepo = personRepo ??
                throw new ArgumentNullException(nameof(personRepo));
        }

        private IEnumerable<PersonTypeDto> GetPersonTypes()
        {
            var result = Task.Run(async () =>
            await _personRepo.GetPersonTypesAsync(
                new PersonTypeParametersDto
                {
                    PageSize = 1000,
                    Filters = "isActive==true",
                    SortOrder = "Name"
                })).ConfigureAwait(true);

            var response = result.GetAwaiter().GetResult();

            return response.Items;
        }

        private IEnumerable<CompanyDto> GetCompanies()
        {
            var result = Task.Run(async () =>
            await _personRepo.GetCompaniesAsync(
                new CompanyParametersDto
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

            PersonParametersDto personParameters = new PersonParametersDto()
            {
                PageNumber = page,
                PageSize = pageSize,
                SortOrder = sortOrder,
                Filters = filters
            };

            var pagingResponse = await _personRepo.GetPersonsAsync(personParameters);

            return View(pagingResponse);
        }

        public IActionResult Create(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            return View("Edit", PersonViewModelFactory.Create(new PersonDto(), returnUrl, PersonTypes, Companies));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PersonViewModel personVm)
        {
            if (ModelState.IsValid)
            {
                personVm.PersonTypes = default;
                personVm.Companies = default;


                var newPerson = await _personRepo.AddPerson(personVm.Person);

                TempData["message"] = "Your data has been submitted successfully.";
                TempData["toasterType"] = ToasterType.success;

                //return RedirectToAction(nameof(Index));

                return RedirectToAction(nameof(Edit), new { id = newPerson.Id, returnUrl = personVm.ReturnUrl });
            }

            TempData["message"] = "A problem has been ocurred while submitting your data.";
            TempData["toasterType"] = ToasterType.info;

            return View("Edit", PersonViewModelFactory.Create(personVm.Person, personVm.ReturnUrl, PersonTypes, Companies));
        }

        public async Task<IActionResult> Details(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var person = await _personRepo.GetPerson(id);
            PersonViewModel personVm = PersonViewModelFactory.Details(person, returnUrl, PersonTypes, Companies);

            return View("Edit", personVm);
        }

        public async Task<IActionResult> Edit(int id, string returnUrl = null)
        {
            if (String.IsNullOrEmpty(returnUrl))
            {
                returnUrl = Request.Headers["Referer"].ToString();
            }

            var person = await _personRepo.GetPerson(id);
            PersonViewModel personVm = PersonViewModelFactory.Edit(person, returnUrl, PersonTypes, Companies);

            return View("Edit", personVm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] PersonViewModel personVM)
        {
            if (ModelState.IsValid)
            {
                await _personRepo.UpdatePerson(personVM.Person);

                TempData["message"] = "Your data has been updated successfully.";
                TempData["toasterType"] = ToasterType.success;

                return RedirectToAction(nameof(Edit), new { id = personVM.Person.Id, returnUrl = personVM.ReturnUrl });
            }
            else
            {
                TempData["message"] = "A problem has been ocurred while updating your data.";
                TempData["toasterType"] = ToasterType.info;
            }

            return View("Edit", PersonViewModelFactory.Edit(personVM.Person, personVM.ReturnUrl, PersonTypes, Companies));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var returnUrl = Request.Headers["Referer"].ToString();

            var person = await _personRepo.GetPerson(id);
            PersonViewModel personVm = PersonViewModelFactory.Delete(person, returnUrl, PersonTypes, Companies);

            return View("Edit", personVm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(PersonViewModel personVM)
        {
            await _personRepo.DeletePerson(personVM.Person);

            TempData["message"] = "Your data has been deleted successfully.";
            TempData["toasterType"] = ToasterType.success;

            //return RedirectToAction(nameof(Index));

            return Redirect(personVM.ReturnUrl);
        }
    }
}
