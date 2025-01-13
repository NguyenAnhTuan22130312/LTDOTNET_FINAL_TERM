using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public CartViewModel Cart { get; set; }
        public MainViewModel()
        {
            Cart = new CartViewModel();
        }

        public event PropertyChangedEventHandler? PropertyChanged
        {
            add
            {
                ((INotifyPropertyChanged)Cart).PropertyChanged += value;
            }

            remove
            {
                ((INotifyPropertyChanged)Cart).PropertyChanged -= value;
            }
        }
    }
}
