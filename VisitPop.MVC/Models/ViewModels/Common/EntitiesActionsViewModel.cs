using VisitPop.MVC.Components;

namespace VisitPop.MVC.Models.ViewModels.Common
{
    public class EntitiesActionsViewModel
    {
        public string Action { get; set; } = "Create";
        public bool ReadOnly { get; set; } = false;
        public string Theme { get; set; } = "primary";
        public bool ShowAction { get; set; } = true;
        public bool ShowCreateNewAction { get; set; } = false;
        public string ReturnUrl { get; set; }
    }
}
