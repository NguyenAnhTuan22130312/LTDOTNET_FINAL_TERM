using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Models
{
    public partial class CartItemHuy : INotifyPropertyChanged
    {
        public int cart_item_id { get; set; }
        public int cart_id { get; set; }
        public Food food { get; set; }
        
        private int _quantity;
        public int quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged(nameof(quantity));
                OnPropertyChanged(nameof(totalPrice));
            }
        }
        public decimal totalPrice => food.Price * quantity;
        public String note { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
