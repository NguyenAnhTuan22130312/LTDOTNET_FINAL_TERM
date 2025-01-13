using ShopApp.ViewModel;

namespace ShopApp.Pages
{
    public partial class FoodDetailPage : ContentPage
    {
        private readonly DatabaseService _databaseService;

        public FoodDetailPage(Food food)
        {
            InitializeComponent();
            _databaseService = new DatabaseService(); // Khởi tạo đối tượng DatabaseService
            BindingContext = new FoodDetailViewModel(food, _databaseService, Navigation);  // Truyền food và databaseService vào ViewModel
        }
    }
}
