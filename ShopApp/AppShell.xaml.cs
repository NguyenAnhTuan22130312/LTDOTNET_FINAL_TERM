using Microsoft.Maui.Controls;

namespace ShopApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Android: Hiển thị thanh điều hướng dưới cùng
            if (DeviceInfo.Platform == DevicePlatform.Android || DeviceInfo.Platform == DevicePlatform.iOS)
            {
                Shell.SetTabBarIsVisible(this, true); // Hiển thị TabBar cho Android và iOS
                Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled); // Không dùng Flyout trên Android
            }

            // Windows: Hiển thị Flyout cố định bên trái
            if (DeviceInfo.Platform == DevicePlatform.WinUI)
            {
                Shell.SetFlyoutBehavior(this, FlyoutBehavior.Locked);
                Shell.SetFlyoutWidth(this, 300); // Chiếm 1/5 cửa sổ
            }
        }
    }
}
