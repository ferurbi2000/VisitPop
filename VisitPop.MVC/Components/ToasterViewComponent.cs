using Microsoft.AspNetCore.Mvc;

namespace VisitPop.MVC.Components
{
    public class ToasterViewComponent : ViewComponent
    {
        public ToasterViewComponent() { }

        public IViewComponentResult Invoke(string message, ToasterType toasterType = ToasterType.info)
        {
            return View(new ToasterViewModel { Message = message, ToasterType = toasterType });
        }
    }

    public class ToasterViewModel
    {
        public string Message { get; set; }
        public ToasterType ToasterType { get; set; }
    }

    public enum ToasterType
    {
        success,
        info,
        warning,
        danger,
        primary,
        secondary,
        dark,
        light
    }
}
