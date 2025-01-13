using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopApp.Service;
using ShopApp.ViewModel;

namespace ShopApp.Pages
{
    public partial class CheckoutPage : ContentPage
    {
        public CheckoutPage()
        {
            InitializeComponent();
            BindingContext = new CheckoutViewModel();
            
        }

        private async void OnOrderButtonClicked(object sender, EventArgs e)
        {
            var viewModel = BindingContext as CheckoutViewModel;
            if (viewModel == null || viewModel.Cart == null || !viewModel.Cart.cartItems.Any())
            {
                // Hiển thị thông báo lỗi nếu giỏ hàng trống
                await DisplayAlert("Lỗi", "Giỏ hàng trống. Vui lòng thêm sản phẩm vào giỏ hàng.", "OK");
                return;
            }

            // Lấy địa chỉ giao hàng từ AddressEditor
            string deliveryAddress = AddressEditor.Text;

            // Kiểm tra nếu địa chỉ giao hàng trống
            if (string.IsNullOrEmpty(deliveryAddress))
            {
                await DisplayAlert("Lỗi", "Vui lòng nhập địa chỉ giao hàng.", "OK");
                return;
            }

            // Lấy phương thức thanh toán
            var paymentMethod = 0; // Mặc định là ShopeePay (0)
if (DirectPaymentRadioButton.IsChecked) // Kiểm tra nếu thanh toán trực tiếp (1)
{
    paymentMethod = 1;
}


            // Tạo đơn hàng
            var order = new Order
            {
                DeliveryAddress = deliveryAddress,
                PaymentMethod = paymentMethod,
                CartItems = viewModel.Cart.cartItems.Select(item => new OrderItem
                {
                    FoodName = item.food.FoodName,
                    Quantity = item.quantity,
                    Price = item.food.Price,
                    TotalPrice = item.totalPrice
                }).ToList(),
                TotalAmount = viewModel.Cart.TotalAmount
            };

            // Lưu đơn hàng vào cơ sở dữ liệu
            var result = await OrderService.SaveOrder(order);
            if (result)
            {
                // Hiển thị thông báo thành công và quay lại trang giỏ hàng
                await DisplayAlert("Thành công", "Đơn hàng của bạn đã được đặt thành công!", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                // Hiển thị lỗi nếu lưu đơn hàng không thành công
                await DisplayAlert("Lỗi", "Có lỗi xảy ra khi đặt đơn hàng. Vui lòng thử lại.", "OK");
            }
        }

    }
}
