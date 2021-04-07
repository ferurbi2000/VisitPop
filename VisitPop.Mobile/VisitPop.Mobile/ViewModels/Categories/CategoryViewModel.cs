using VisitPop.Mobile.Models;

namespace VisitPop.Mobile.ViewModels.Categories
{
    public class CategoryViewModel : BaseViewModel
    {
        public CategoryViewModel() { }

        public CategoryViewModel(Category category)
        {
            Id = category.Id;
            _name = category.Name;
            _description = category.Description;
        }

        private int _id;
        public int Id
        {
            get => _id;
            set { SetProperty(ref _id, value); }
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value);
                OnPropertyChanged(nameof(FullName));
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                SetProperty(ref _description, value);
                OnPropertyChanged(nameof(FullName));
            }
        }
        public string FullName { get => $"{Name} {Description}"; }
    }
}
