using ShopApp.Dao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Service
{
    public static class CartStorage
    {
        private const string UserIdKey = "user_id_int";

        public static int GetUserId()
        {
            // Lấy id từ cache 
            var userIdString = Preferences.Get(UserIdKey, string.Empty);
            if (!string.IsNullOrEmpty(userIdString) && int.TryParse(userIdString, out int userId))
                return userId;
            // Nếu không có thì tạo mới và lưu
            var newUserId = CreateNewUserId();
            Preferences.Set(UserIdKey, newUserId.ToString());
            return newUserId;
        }
        private static int CreateNewUserId()
        {
            // lay id lon nhat + 1 de tranh trung lap 
            return DatabaseCart.GetMaxUserId() + 1; 
        }

        public static void ClearUserId()
        {
            Preferences.Remove(UserIdKey);
        }
    }
}
