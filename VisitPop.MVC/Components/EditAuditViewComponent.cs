using Microsoft.AspNetCore.Mvc;
using VisitPop.Domain.Common;

namespace VisitPop.MVC.Components
{
    public class EditAuditViewComponent : ViewComponent
    {
        public EditAuditViewComponent() { }

        public IViewComponentResult Invoke(AuditableEntity entity)
        {
            return View(new EditAuditViewModel
            {
                IsActive = entity.IsActive,
                IsDeleted = entity.IsDeleted,
                CreatedDate = entity.CreatedDate,
                LastModified = entity.LastModified
            });
        }
    }

    public class EditAuditViewModel : AuditableEntity { }
}
