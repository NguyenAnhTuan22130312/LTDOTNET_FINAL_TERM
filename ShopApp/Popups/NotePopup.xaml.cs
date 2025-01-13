using CommunityToolkit.Maui.Views;

namespace ShopApp.Popups
{
    public partial class NotePopup : Popup
    {
        public NotePopup()
        {
            InitializeComponent();
        }

        private void OnConfirmClicked(object sender, EventArgs e)
        {
            //TODO: Lấy note và truyền ngược lại giỏ hàng
            Close();
        }
        private void OnCancelClicked(object sender, EventArgs e)
        {
            Close();
        }
        private void OnSuggestTapped(object sender, EventArgs e)
        {
            var button = (Button)sender;
            NoteEntry.Text = button.Text;
        }
    }
}