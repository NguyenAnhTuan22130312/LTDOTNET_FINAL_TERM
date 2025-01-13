using System.Collections.ObjectModel;
public class HomePageViewModel
{
    public ObservableCollection<Product> Products { get; set; }

    public DatabaseService databaseService = new DatabaseService();

    



    public HomePageViewModel()
    {
        Products = new ObservableCollection<Product>
        {
            new Product { Id = 1, Name = "Cơm Tấm Sườn Bì Chả", Image = "com_tam_suon_bi_cha.png", Price = 50.000M, Description = "Thơm ngon mời bạn ăn nha" },
            new Product { Id = 2, Name = "Bún Bò Huế", Image = "bun_bo_hue.png", Price = 40.000M, Description = "Thơm ngon mời bạn ăn nha" },
            new Product { Id = 3, Name = "Bún Chả Ghẹ", Image = "bun_cha_ghe.png", Price = 45.000M, Description = "Thơm ngon mời bạn ăn nha" },
            new Product { Id = 4, Name = "Bún Chả Hà Nội", Image = "bun_cha_ha_noi.png", Price = 45.000M, Description = "Thơm ngon mời bạn ăn nha" },
            new Product { Id = 5, Name = "Bún Gà Sa Tế", Image = "bun_ga_sa_te.png", Price = 45.000M, Description = "Thơm ngon mời bạn ăn nha" },
            new Product { Id = 6, Name = "Bún Heo Quay", Image = "bun_heo_quay.png", Price = 45.000M, Description = "Thơm ngon mời bạn ăn nha" },
            new Product { Id = 7, Name = "Bún Huế Chay", Image = "bun_hue_chay.png", Price = 45.000M, Description = "Thơm ngon mời bạn ăn nha" },
            new Product { Id = 8, Name = "Bún Thịt Nướng", Image = "bun_thit_nuong.png", Price = 45.000M, Description = "Thơm ngon mời bạn ăn nha" }
        };


    }
}
