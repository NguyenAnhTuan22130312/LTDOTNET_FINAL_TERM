using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace ShopApp.ViewModel
{
    public class DetailOrderViewModel
    {
        public ICommand BackCommand { get; }

        public DetailOrderViewModel()
        {
            BackCommand = new Command(OnBack);
        }

        private async void OnBack()
        {
            // Điều hướng về trang OrdersPage
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}