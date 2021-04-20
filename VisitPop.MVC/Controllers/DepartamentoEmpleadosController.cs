using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisitPop.Application.Dtos.DepartamentoEmpleado;
using VisitPop.MVC.Models.ViewModels;
using VisitPop.MVC.Services.DepartamentoEmpleado;

namespace VisitPop.MVC.Controllers
{
    public class DepartamentoEmpleadosController : Controller
    {
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10, string filters = "", string sortOrder = "")
        {
            //var Filters = String.IsNullOrEmpty(filters)? "" : $"Nombre @=* {filters}";

            ViewBag.pageSize = pageSize;
            ViewBag.filter = filters;

            ViewData["IdSortParm"] = sortOrder == "Id" ? "-Id" : "Id";
            ViewData["NombreSortParm"] = sortOrder == "Nombre" ? "-Nombre" : "Nombre";


            DepartamentoEmpleadoParametersDto departamentoEmpleadoParameters = new DepartamentoEmpleadoParametersDto() { PageNumber = page, PageSize = pageSize, SortOrder = sortOrder, Filters = filters };
            var pagingResponse = await new DepartamentoEmpleadoRepository().GetDepartamentoEmpleadosAsync(departamentoEmpleadoParameters);

            return View(pagingResponse);

        }
    }
}
