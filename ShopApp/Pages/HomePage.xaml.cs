using Microsoft.Maui.Controls;
using ShopApp.ViewModel;
using ShopApp.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShopApp.Pages
{
    public partial class HomePage : ContentPage
    {
        public ObservableCollection<Food> Products { get; set; }
        private List<Food> allFood; // Lưu trữ tất cả món ăn

        public HomePage()
        {
            InitializeComponent();
            Products = new ObservableCollection<Food>();
            allFood = new List<Food>(); // Khởi tạo danh sách tất cả món ăn
            BindingContext = this;
            LoadData();
        }

        private void LoadData()
        {
            DatabaseService dbService = new DatabaseService();
            var foodList = dbService.GetAllFood();
            allFood = foodList.ToList(); // Lưu danh sách món ăn vào allFood
            UpdateProducts(allFood); // Hiển thị tất cả món ăn ban đầu
        }

        private void UpdateProducts(List<Food> foodList)
        {
            Products.Clear();
            foreach (var food in foodList)
            {
                Products.Add(food);
            }
        }


        private async void OnProductSelected(object sender, SelectionChangedEventArgs e)
{
    var selectedProduct = e.CurrentSelection.FirstOrDefault() as Food;
    if (selectedProduct != null)
    {
        // Truyền đối tượng selectedProduct (Food) vào FoodDetailPage
        var detailPage = new FoodDetailPage(selectedProduct);
        
        // Chuyển sang trang chi tiết món ăn
        await Navigation.PushAsync(detailPage);
    }
}





        // Sự kiện khi nhấn vào nút lọc category
        private void OnCategorySelected(object sender, EventArgs e)
        {
            var button = sender as Button;
            int idCategory = int.Parse(button.CommandParameter.ToString());

            // Sử dụng idCategory để lọc món ăn
            var filteredFood = allFood.Where(food => food.IdCategory == idCategory).ToList();
            UpdateProducts(filteredFood); // Cập nhật danh sách món ăn hiển thị
        }

        // Sự kiện tìm kiếm món ăn theo từ khóa
        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = e.NewTextValue.ToLower();

            var filteredFood = allFood.Where(food => food.FoodName.ToLower().Contains(searchText)).ToList();
            UpdateProducts(filteredFood);
        }
        private void OnShowAllClicked(object sender, EventArgs e)
        {
            // Đặt lại danh sách sản phẩm để hiển thị tất cả món ăn
            Products = new ObservableCollection<Food>(allFood);
            OnPropertyChanged(nameof(Products)); // Cập nhật giao diện
        }

    }
}
