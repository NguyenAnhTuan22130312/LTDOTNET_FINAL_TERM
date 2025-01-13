using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.Input;
using ShopApp.Service;
using ShopApp.ViewModel;
using System.Diagnostics;
//using ShopApp.ViewModels;


public partial class FoodDetailViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;
    private Food _food;
    public INavigation Navigation { get; set; }

    public Food Food
    {
        get => _food;
        set
        {
            _food = value;
            OnPropertyChanged();
        }
    }

    public List<Review> Reviews { get; private set; }

    public FoodDetailViewModel(Food food, DatabaseService databaseService, INavigation navigation)
    {
        _food = food;
        _databaseService = databaseService;
        Navigation = navigation;
        LoadReviews();
        

    }

    private void LoadReviews()
    {
        Reviews = _databaseService.GetReviewsByFoodId(_food.IdFood);
    }

    // Huy
    //[RelayCommand]
    //public async Task AddToCart()
    //{
    //    if (Food == null)
    //    {
    //        await MainThread.InvokeOnMainThreadAsync(async () =>
    //        {
    //            await Toast.Make("Không có sản phẩm nào được chọn", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
    //        });
    //        return;
    //    }
    //    try
    //    {
    //        var cart = await CartService.Instance.AddToCart(Food);
    //        await MainThread.InvokeOnMainThreadAsync(async () =>
    //        {
    //            await Task.Delay(1000);
    //            await Toast.Make("Đã thêm vào giỏ hàng thành công", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();
    //        });
    //        if (cart != null)
    //            await Navigation.PushAsync(new ShopApp.Pages.CartPage());

    //    }
    //    catch (Exception ex)
    //    {
    //        await MainThread.InvokeOnMainThreadAsync(async () =>
    //        {
    //            Debug.WriteLine(Food.FoodName + Food.Price + Food.IdFood);
    //            Debug.WriteLine("loi roi ");
    //            await Toast.Make( "foodDetail" + $"Lỗi: {ex.Message}", CommunityToolkit.Maui.Core.ToastDuration.Short).Show();

    //        });
    //    }

    [RelayCommand]
    public async Task AddToCart()
    {
        if (Food == null)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                Debug.WriteLine("Không có sản phẩm nào được chọn"); //log thay vì toast
            });
            return;
        }
        try
        {
            var cart = await CartService.Instance.AddToCart(Food);
            Debug.WriteLine("CartService thành công " + Food.FoodName); //log thay vì toast
            if (cart != null)
                await Navigation.PushAsync(new ShopApp.Pages.CartPage());
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                Debug.WriteLine(Food.FoodName + Food.Price + Food.IdFood);
                Debug.WriteLine("loi roi ");
                Debug.WriteLine("Loi" + ex.Message);
            });
        }
    }
}

