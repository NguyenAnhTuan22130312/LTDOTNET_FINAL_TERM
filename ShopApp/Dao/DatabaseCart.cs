using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using ShopApp.Models;
using System.Diagnostics;
using System.Data;

namespace ShopApp.Dao
{
    public partial class DatabaseCart
    {
        private static string connectionString = "Server=localhost;Database=food_maui;UserID=root;Password=";

        // Helper method to execute a query and return a single value.
        private static object ExecuteScalar(string query, Dictionary<string, object> parameters = null)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            cmd.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }
        // Helper method to execute a query and return data rows
        private static List<Dictionary<string, object>> ExecuteQuery(string query, Dictionary<string, object> parameters = null)
        {
            var dataRows = new List<Dictionary<string, object>>();
            using (var connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue(param.Key, param.Value);
                        }
                    }
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }
                            dataRows.Add(row);
                        }
                    }
                }
            }
            return dataRows;
        }
        // 1. Lấy Giỏ Hàng của Người Dùng (Hoặc Tạo Mới):
        public static Cart GetCartByUserId(int userId)
        {
            Cart cart = null;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM carts WHERE user_id = @userId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userId", userId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cart = new Cart
                                {
                                    cart_id = reader.GetInt32("cart_id"),
                                    user_id = reader.GetInt32("user_id"),
                                    cartItems = GetCartItemsByCartId(reader.GetInt32("cart_id")),
                                    orderDate = reader.GetDateTime("created_at")
                                };
                            }
                            else
                            {
                                int newCartId = CreateNewCart(userId);
                                if (newCartId > 0)
                                    cart = new Cart
                                    {
                                        cart_id = newCartId,
                                        user_id = reader.GetInt32(userId),
                                        cartItems = new List<CartItemHuy>(),
                                        orderDate = DateTime.Now
                                    };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error at GetCartByUserId: " + ex.Message);
                    return null;
                }
            }
            return cart;
        }
        // 2. Tạo Mới Giỏ Hàng:
        private static int CreateNewCart(int userId)
        {
            string query = "INSERT INTO carts (user_id, created_at) VALUES (@userId, NOW()); SELECT LAST_INSERT_ID();";
            Dictionary<string, object> parameters = new Dictionary<string, object>
             {
                  {"@userId", userId}
             };
            var newCartId = ExecuteScalar(query, parameters);
            return newCartId != null ? Convert.ToInt32(newCartId) : 0;
        }
        private static List<CartItemHuy> GetCartItemsByCartId(int cartId)
        {
            var cartItems = new List<CartItemHuy>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ci.cart_item_id, ci.cart_id, ci.food_id, ci.quantity, ci.note, f.idFood, f.foodName, f.price, f.discountPrice, f.img, f.description " +
                             "FROM cart_items ci " +
                             "INNER JOIN food f ON ci.food_id = f.idFood " +
                             "WHERE ci.cart_id = @cartId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cartId", cartId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var cartItem = new CartItemHuy
                                {
                                    cart_item_id = reader.GetInt32("cart_item_id"),
                                    cart_id = reader.GetInt32("cart_id"),
                                    food = new Food
                                    {
                                        IdFood = reader.GetInt32("idFood"),
                                        FoodName = reader.GetString("foodName"),
                                        Price = reader.GetDecimal("price"),
                                        DiscountPrice = reader.IsDBNull("discountPrice") ? 0 : reader.GetInt32("discountPrice"),
                                        Img = reader.GetString("img"),
                                        Description = reader.IsDBNull("description") ? null : reader.GetString("description")
                                    },
                                    quantity = reader.GetInt32("quantity"),
                                    note = reader.IsDBNull("note") ? null : reader.GetString("note")
                                };
                                Debug.WriteLine("thong tin cart item " + cartItem.quantity + " " + cartItem.totalPrice + " " + cartItem.food.FoodName);
                                cartItems.Add(cartItem);
                            }
                        }

                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error at GetCartItemsByCartId: " + ex.Message);
                }
            }
            return cartItems;
        }

        // 4. Thêm Sản Phẩm Vào Giỏ Hàng:
        public static Cart AddItemToCart(int userId, int foodId, int quantity, string note = null)
        {
            // check xem cart có tồn tại hay không
            Cart cart = GetCartByUserId(userId);
            if (cart == null) return null;
            try
            {
                var existingItem = GetCartItemsByCartId(cart.cart_id).FirstOrDefault(item => item.food.IdFood == foodId);
                if (existingItem != null)
                {
                    UpdateCartItemQuantity(existingItem.cart_item_id, existingItem.quantity + quantity);
                    return GetCartByUserId(userId);
                }
                string query = "INSERT INTO cart_items (cart_id, food_id, quantity, note) VALUES (@cartId, @foodId, @quantity, @note)";
                Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                       {"@cartId", cart.cart_id},
                        {"@foodId", foodId},
                        {"@quantity", quantity},
                        {"@note", note}
                    };
                ExecuteScalar(query, parameters);
                UpdateCartLastUpdate(cart.cart_id);
                return GetCartByUserId(userId);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error in AddItemToCart: " + ex.ToString());
                return null;
            }
        }

        // 5. Cập Nhật Số Lượng Sản Phẩm Trong Giỏ Hàng:
        public static void UpdateCartItemQuantity(int cartItemId, int quantity)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE cart_items SET quantity = @quantity WHERE cart_item_id = @cartItemId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cartItemId", cartItemId);
                        cmd.Parameters.AddWithValue("@quantity", quantity);
                        cmd.ExecuteNonQuery();
                    }
                    var cartItem = GetCartItemById(cartItemId);
                    if (cartItem != null)
                        UpdateCartLastUpdate(GetCartByUserId(GetCartItemById(cartItemId).cart_id).cart_id);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error at UpdateCartItemQuantity: " + ex.Message);
                }
            }
        }
        // 6. Xóa Sản Phẩm Khỏi Giỏ Hàng:
        public static void RemoveItemFromCart(int cartItemId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM cart_items WHERE cart_item_id = @cartItemId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cartItemId", cartItemId);
                        cmd.ExecuteNonQuery();
                    }
                    var cartItem = GetCartItemById(cartItemId);
                    if (cartItem != null)
                        UpdateCartLastUpdate(GetCartByUserId(GetCartItemById(cartItemId).cart_id).cart_id);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error at RemoveItemFromCart: " + ex.Message);
                }
            }
        }

        // 7. Xóa Toàn Bộ Giỏ Hàng:
        public static void ClearCart(int userId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    Cart cart = GetCartByUserId(userId);
                    if (cart == null) return;

                    connection.Open();
                    string query = "DELETE FROM cart_items WHERE cart_id = @cartId";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@cartId", cart.cart_id);
                        cmd.ExecuteNonQuery();
                    }
                    string query2 = "DELETE FROM carts WHERE cart_id = @cartId";
                    using (MySqlCommand cmd2 = new MySqlCommand(query2, connection))
                    {
                        cmd2.Parameters.AddWithValue("@cartId", cart.cart_id);
                        cmd2.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error at ClearCart: " + ex.Message);
                }
            }

        }
        // 8. Lấy 1 cart Item
        private static CartItemHuy GetCartItemById(int cartItemId)
        {
            CartItemHuy cartItem = null;
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT ci.cart_item_id, ci.cart_id, ci.food_id, ci.quantity, ci.note, f.idFood, f.foodName, f.price, f.discountPrice, f.img, f.description " +
                            "FROM cart_items ci " +
                            "INNER JOIN food f ON ci.food_id = f.idFood " +
                            "WHERE ci.cart_item_id = @cartItemId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cartItemId", cartItemId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cartItem = new CartItemHuy
                                {
                                    cart_item_id = reader.GetInt32("cart_item_id"),
                                    cart_id = reader.GetInt32("cart_id"),
                                    food = new Food
                                    {
                                        IdFood = reader.GetInt32("idFood"),
                                        FoodName = reader.GetString("foodName"),
                                        Price = reader.GetDecimal("price"),
                                        DiscountPrice = reader.IsDBNull("discountPrice") ? 0 : reader.GetInt32("discountPrice"),
                                        Img = reader.GetString("img"),
                                        Description = reader.IsDBNull("description") ? null : reader.GetString("description")
                                    },
                                    quantity = reader.GetInt32("quantity"),
                                    note = reader.IsDBNull("note") ? null : reader.GetString("note")
                                };
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error at GetCartItemById: " + ex.Message);
                }
            }
            return cartItem;
        }

        // 9. Cập nhật thời gian của cart
        private static void UpdateCartLastUpdate(int cartId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE carts SET created_at = NOW() WHERE cart_id = @cartId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@cartId", cartId);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error at UpdateCartLastUpdate: " + ex.Message);
                }
            }
        }

        internal static int GetMaxUserId()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT MAX(user_id) FROM carts";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        var result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in GetMaxUserId: " + ex.ToString());
                }
                return 0;
            }
        }
    }
}