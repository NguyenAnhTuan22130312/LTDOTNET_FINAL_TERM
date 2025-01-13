using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShopApp.Models;
using ShopApp.Pages;
using ShopApp.Service;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ShopApp.ViewModel
{
    public partial class CartViewModel : ObservableObject
    {

        public static CartViewModel _instance;
        private Cart _cart;
        public Cart Cart
    {
        get => _cart;
        set => SetProperty(ref _cart, value); 
    }
        public static CartViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CartViewModel();
                }
                return _instance;
            }
        }

        

        public CartViewModel()
        {
            LoadCart().ConfigureAwait(false);
        }
        public async Task LoadCart()
        {
            
        _cart = CartService.Instance.GetCart();
        OnPropertyChanged(nameof(Cart)); 
        }
        [RelayCommand]
        public async Task IncreaseQuantity(int cartItemId)
        {
            await CartService.Instance.UpdateQuantity(cartItemId, _cart.cartItems.FirstOrDefault(x => x.cart_item_id == cartItemId).quantity + 1);
            await LoadCart();
            UpdateCartSummary();
        }
        [RelayCommand]
        public async Task DecreaseQuantity(int cartItemId)
        {
            var cartItem = _cart.cartItems.FirstOrDefault(x => x.cart_item_id == cartItemId);
            if (cartItem != null)
            {
                if (cartItem.quantity > 1)
                    await CartService.Instance.UpdateQuantity(cartItemId, cartItem.quantity - 1);
                else
                    await RemoveItem(cartItemId);
                await LoadCart();
                UpdateCartSummary();
            }
        }

        [RelayCommand]
        public async Task RemoveItem(int cartItemId)
        {
            await CartService.Instance.RemoveFromCart(cartItemId);
            await LoadCart();
            UpdateCartSummary();
        }
        [RelayCommand]
        public async Task ClearCart()
        {
            await CartService.Instance.ClearCart();
            await LoadCart();
            UpdateCartSummary();
        }
    
        private void UpdateCartSummary()
        {
            if (_cart != null)
                _cart.totalQuantity = _cart.cartItems.Sum(item => item.quantity);
        }

        public async Task AddToCart(Food food, int quantity = 1, string note = null)
        {
            _cart = await CartService.Instance.AddToCart(food, quantity, note);
        }
     [RelayCommand]
public async Task GoToCheckout()
{
    if (_cart == null || !_cart.cartItems.Any())
    {
        // Bạn có thể hiển thị thông báo nếu giỏ hàng trống
        return;
    }

    var currentPage = Shell.Current.CurrentPage;

    // Tạo CheckoutPage mới và gán BindingContext với CheckoutViewModel
    var checkoutPage = new CheckoutPage();
    var checkoutViewModel = new CheckoutViewModel(this.Cart); // Truyền giỏ hàng từ CartViewModel sang CheckoutViewModel
    checkoutPage.BindingContext = checkoutViewModel;

    // Chuyển sang trang CheckoutPage
    await currentPage.Navigation.PushAsync(checkoutPage);
}

    }
}