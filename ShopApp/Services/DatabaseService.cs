using MySql.Data.MySqlClient;
using ShopApp.Models;
using System;

public class DatabaseService
{
    private string connectionString = "Server=localhost;Database=food_maui;User ID=root;Password=";

    public DatabaseService() { }

   // Lấy tất cả món ăn
        public List<Food> GetAllFood()
        {
            var foodList = new List<Food>();
            
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM food";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                foodList.Add(new Food
                                {
                                    IdFood = reader.GetInt32("idFood"),
                                    IdCategory = reader.GetInt32("idCategory"),
                                    FoodName = reader.GetString("foodName"),
                                    Price = reader.GetDecimal("price"),
                                    Description = reader.GetString("description"),
                                    DiscountPrice = reader.GetInt32("discountPrice"),
                                    Img = reader.GetString("img"),
                                    Quantity = reader.GetInt32("quantity")
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return foodList;
        }

        public List<Review> GetReviewsByFoodId(int idFood)
{
    var reviewList = new List<Review>();

    using (MySqlConnection conn = new MySqlConnection(connectionString))
    {
        try
        {
            conn.Open();
            string query = "SELECT * FROM reviews WHERE idFood = @idFood";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@idFood", idFood);
                
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reviewList.Add(new Review
                        {
                            UserName = reader.GetString("userName"),
                            Comment = reader.GetString("comment"),
                            Rating = reader.GetInt32("rating")
                        });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    return reviewList;
}




    
}
