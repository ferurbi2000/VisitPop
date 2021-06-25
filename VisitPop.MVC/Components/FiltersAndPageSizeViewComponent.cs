using Microsoft.AspNetCore.Mvc;
using VisitPop.MVC.Models.ViewModels;

namespace VisitPop.MVC.Components
{
    public class FiltersAndPageSizeViewComponent : ViewComponent
    {
        public FiltersAndPageSizeViewComponent()
        {

        }

        public IViewComponentResult Invoke(int pageSize, string filter, string toolTip)
        {
            return View(new FiltersAndPageSizeVm { PageSize = pageSize, Filter = filter, ToolTip = toolTip });
        }
    }
}
