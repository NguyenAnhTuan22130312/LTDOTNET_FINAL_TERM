using CommunityToolkit.Mvvm.ComponentModel;
using ShopApp.Models;

namespace ShopApp.ViewModel
{
    public class CheckoutViewModel : ObservableObject
{
    private Cart _cart;

    public Cart Cart
    {
        get => _cart;
        set => SetProperty(ref _cart, value);
    }

    public CheckoutViewModel(Cart cart)
    {
        Cart = cart;
    }

        public CheckoutViewModel()
        {
        }
    }

    
}
