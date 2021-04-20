using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitPop.MVC.Models.ViewModels
{
    public class MenuInfo
    {
        public string Nombre { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string IconClass { get; set; }
    }
}
