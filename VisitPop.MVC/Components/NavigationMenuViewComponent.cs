using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using VisitPop.MVC.Models.ViewModels;

namespace VisitPop.MVC.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        //private IDepartamentoEmpleadoRepository _repo;

        public NavigationMenuViewComponent()
        {
            //_repo = repo;
        }

        public IViewComponentResult Invoke()
        {           

            List <MenuInfo> menu = new List<MenuInfo> {
                new MenuInfo{ Nombre="Dashboard", Controller="Home", Action="Index", IconClass="fas fa-tachometer-alt" },
                new MenuInfo{ Nombre="Visitas", Controller="Visitas", Action="Index", IconClass="fas fa-id-card" },
                new MenuInfo{ Nombre="Employee Departments", Controller="EmployeeDepartments", Action="Index", IconClass="far fa-calendar" },
                new MenuInfo{ Nombre="Employees", Controller="Employees", Action="Index", IconClass="fas fa-users" },
                new MenuInfo{ Nombre="Companies", Controller="Companies", Action="Index", IconClass="fas fa-building" },
                new MenuInfo{ Nombre="Visit States", Controller="VisitStates", Action="Index", IconClass="far fa-calendar" },
                new MenuInfo{ Nombre="Offices", Controller="Offices", Action="Index", IconClass="fas fa-sitemap" },
                new MenuInfo{ Nombre="Persons", Controller="Persons", Action="Index", IconClass="fas fa-street-view" },
                new MenuInfo{ Nombre="Register Controls", Controller="RegisterControls", Action="Index", IconClass="fas fa-user-shield" },
                new MenuInfo{ Nombre="Person Types", Controller="PersonTypes", Action="Index", IconClass="far fa-calendar" },
                new MenuInfo{ Nombre="Vehicle Types", Controller="VehicleTypes", Action="Index", IconClass="fas fa-car-side" },
                new MenuInfo{ Nombre="Visit Types", Controller="VisitTypes", Action="Index", IconClass="far fa-calendar" },
                //new MenuInfo{ Nombre="Departamento", Controller="DepartamentoEmpleados", Action="Index" },
                //new MenuInfo{ Nombre="Departamento", Controller="DepartamentoEmpleados", Action="Index" },
            };

            ViewBag.SelectedMenu = RouteData?.Values["controller"];
            return View(menu);
        }
    }
}
