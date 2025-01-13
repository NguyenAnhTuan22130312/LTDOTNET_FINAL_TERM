using CommunityToolkit.Maui.Views;
using ShopApp.Popups;
using ShopApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Pages
{
    public partial class CartPage : ContentPage
    {
        public CartPage()
        {
            InitializeComponent();
            BindingContext = CartViewModel.Instance;
        }

        private async void OnNoteLabelTapped(object sender, EventArgs e)
        {
            var popup = new NotePopup();
            var result = await this.ShowPopupAsync(popup);
        }

        private async void OnCheckoutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CheckoutPage());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var cartViewModel = BindingContext as CartViewModel;
            if (cartViewModel != null)
            {
                await cartViewModel.LoadCart();
            }
        }
    }
    
}
