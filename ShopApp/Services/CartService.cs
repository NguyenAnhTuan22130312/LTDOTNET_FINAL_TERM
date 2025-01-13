using ShopApp.Dao;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Service
{
    public class CartService
    {
        private static CartService _instance;
        private Cart _cart;
        public static CartService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CartService();
                }
                return _instance;
            }
        }
        private CartService()
        {
            LoadOrCreateCart().ConfigureAwait(false);
        }
        public async Task LoadOrCreateCart()
        {
            try {
                // Load cart from database based on userId
                _cart = DatabaseCart.GetCartByUserId(CartStorage.GetUserId());
                UpdateCartSummary();
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString);
            } 
        }
        public Cart GetCart()
        {
            return _cart;
        }
        public async Task<Cart> AddToCart(Food food, int quantity = 1, string note = null)
        {
            try
            {
                Debug.WriteLine("call add to cart " + food.FoodName);
                int userId = CartStorage.GetUserId();
                Debug.WriteLine("userId " + userId);
                _cart = DatabaseCart.AddItemToCart(userId, food.IdFood, quantity, note);
                UpdateCartSummary();
                Debug.WriteLine("cartservice them thanh cong " + food.FoodName);
                Debug.WriteLine("tong tien trong cart " + _cart.totalPrice);
                return _cart;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error at CartService AddToCart: " + ex.ToString());
                return null;
            }

        }
        public async Task UpdateQuantity(int cartItemId, int newQuantity)
        {
            DatabaseCart.UpdateCartItemQuantity(cartItemId, newQuantity);
            await LoadOrCreateCart();
            UpdateCartSummary();
        }
        public async Task RemoveFromCart(int cartItemId)
        {
            DatabaseCart.RemoveItemFromCart(cartItemId);
            await LoadOrCreateCart();
            UpdateCartSummary();
        }

        public async Task ClearCart()
        {
            DatabaseCart.ClearCart(CartStorage.GetUserId());
            await LoadOrCreateCart();
            UpdateCartSummary();
        }

        public void SetCart(Cart cart)
        {
            _cart = cart;
        }
        private void UpdateCartSummary()
        {
            if (_cart != null)
                _cart.totalQuantity = _cart.cartItems.Sum(item => item.quantity);
        }
    }
}
