using VisitPop.Application.Dtos.VisitPerson;
using VisitPop.MVC.Models.ViewModels.Common;

namespace VisitPop.MVC.Models.ViewModels
{
    public class VisitPersonViewModel : EntitiesActionsViewModel
    {
        public VisitPersonDto VisitPerson { get; set; }
    }

    public static class VisitPersonViewModelFactory
    {
        public static VisitPersonViewModel Create(VisitPersonDto visitPerson)
        {
            return new VisitPersonViewModel
            {
                VisitPerson = visitPerson
            };
        }
    }
}
