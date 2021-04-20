using Microsoft.AspNetCore.Mvc;
using VisitPop.Application.Wrappers;

namespace VisitPop.MVC.Components
{
    public class PaginationViewComponent : ViewComponent
    {
        public PaginationViewComponent()
        {

        }

        public IViewComponentResult Invoke(MetaData values, string filters, string sortOrder)
        {
            return View(new PaginationViewModel() { List = values, Filters = filters, SortOrder = sortOrder });
        }
    }

    public class PaginationViewModel
    {
        public MetaData List { get; set; }
        public string Filters { get; set; }
        public string SortOrder { get; set; }
    }
}
