using ShopApp.Models;
using ShopApp.ViewModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopApp.Service
{
    public static class OrderService
    {
        private static string connectionString = "Server=localhost;Database=food_maui;User ID=root;Password=";

        // Lưu đơn hàng vào cơ sở dữ liệu
        
        public static async Task<bool> SaveOrder(Order order)
{
    try
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            await conn.OpenAsync();

            // Bắt đầu transaction để đảm bảo tất cả các thao tác được thực hiện cùng lúc
            using (var transaction = await conn.BeginTransactionAsync())
            {
                try
                {
                    // Insert đơn hàng vào bảng orders
                    string insertOrderQuery = "INSERT INTO orders (deliveryAddress, paymentMethod, totalAmount) VALUES (@deliveryAddress, @paymentMethod, @totalAmount)";
                    using (var cmd = new MySqlCommand(insertOrderQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@deliveryAddress", order.DeliveryAddress);
                        cmd.Parameters.AddWithValue("@paymentMethod", order.PaymentMethod);
                        cmd.Parameters.AddWithValue("@totalAmount", order.TotalAmount);

                        // Lấy ID của đơn hàng mới tạo
                        await cmd.ExecuteNonQueryAsync();

                        // Lấy idOrder vừa insert vào
                        string getOrderIdQuery = "SELECT LAST_INSERT_ID()";
                        using (var cmdId = new MySqlCommand(getOrderIdQuery, conn, transaction))
                        {
                            int orderId = Convert.ToInt32(await cmdId.ExecuteScalarAsync());

                            // Insert các mục đơn hàng vào bảng order_items
                            string insertOrderItemQuery = "INSERT INTO order_items (orderId, foodName, quantity, price, totalPrice) VALUES (@orderId, @foodName, @quantity, @price, @totalPrice)";
                            foreach (var item in order.CartItems)
                            {
                                using (var itemCmd = new MySqlCommand(insertOrderItemQuery, conn, transaction))
                                {
                                    itemCmd.Parameters.AddWithValue("@orderId", orderId);
                                    itemCmd.Parameters.AddWithValue("@foodName", item.FoodName);
                                    itemCmd.Parameters.AddWithValue("@quantity", item.Quantity);
                                    itemCmd.Parameters.AddWithValue("@price", item.Price);
                                    itemCmd.Parameters.AddWithValue("@totalPrice", item.TotalPrice);

                                    await itemCmd.ExecuteNonQueryAsync();
                                }
                            }
                        }

                        // Commit transaction
                        await transaction.CommitAsync();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    // Rollback nếu có lỗi
                    await transaction.RollbackAsync();
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Database Error: " + ex.Message);
        return false;
    }
}

    }
}
