using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.MVC.Models;

namespace VisitPop.MVC.Controllers
{
    public class PersonController : Controller
    {
        public IActionResult Index()
        {
            return View(new List<Person>());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Person person)
        {

            return RedirectToAction(nameof(Index));
        }
    }
}
