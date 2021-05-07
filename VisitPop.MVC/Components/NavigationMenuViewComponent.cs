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
                new MenuInfo{ Nombre="Empleados", Controller="Empleados", Action="Index", IconClass="fas fa-users" },
                new MenuInfo{ Nombre="Empresas", Controller="Empresas", Action="Index", IconClass="fas fa-building" },
                new MenuInfo{ Nombre="Estado Visitas", Controller="EstadoVisitas", Action="Index", IconClass="far fa-calendar" },
                new MenuInfo{ Nombre="Oficinas", Controller="Oficinas", Action="Index", IconClass="fas fa-sitemap" },
                new MenuInfo{ Nombre="Personas", Controller="Personas", Action="Index", IconClass="fas fa-street-view" },
                new MenuInfo{ Nombre="Puntos de Control", Controller="PuntoControles", Action="Index", IconClass="fas fa-user-shield" },
                new MenuInfo{ Nombre="Tipo Personas", Controller="TipoPersonas", Action="Index", IconClass="far fa-calendar" },
                new MenuInfo{ Nombre="Tipo Vehiculos", Controller="TipoVehiculos", Action="Index", IconClass="fas fa-car-side" },
                new MenuInfo{ Nombre="Tipo Visitas", Controller="TipoVisitas", Action="Index", IconClass="far fa-calendar" },
                //new MenuInfo{ Nombre="Departamento", Controller="DepartamentoEmpleados", Action="Index" },
                //new MenuInfo{ Nombre="Departamento", Controller="DepartamentoEmpleados", Action="Index" },
            };

            ViewBag.SelectedMenu = RouteData?.Values["controller"];
            return View(menu);
        }
    }
}
