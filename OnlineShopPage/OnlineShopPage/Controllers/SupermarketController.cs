using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using OnlineShopPage.Models;

namespace MyProject.Controllers
{
    public class SupermarketController : Controller
    {
        private readonly string connectionString = "Data Source=WIN10-2004;Initial Catalog=MarketDB;Integrated Security=true;";

        public ActionResult Index()
        {
            List<Category> categories = new List<Category>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT Id, Name FROM Categories";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Category category = new Category();
                            category.Id = reader.GetInt32(0);
                            category.Name = reader.GetString(1);
                            categories.Add(category);
                        }
                    }
                }
            }

            return View(categories);
        }

        public ActionResult GetSubcategories(int categoryId)
        {
            List<Subcategory> Subcategories = new List<Subcategory>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT Id, Name FROM Subcategories WHERE CategoryId = @CategoryId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CategoryId", categoryId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Subcategory subcategory = new Subcategory();
                            subcategory.Id = reader.GetInt32(0);
                            subcategory.Name = reader.GetString(1);
                            Subcategories.Add(subcategory);
                        }
                    }
                }
            }

            return Json(Subcategories, JsonRequestBehavior.AllowGet);
        }



        public ActionResult GetProducts(int subcategoryId)
        {
            List<ProductViewModel> products = new List<ProductViewModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "SELECT Id, Name, Price FROM Products WHERE SubcategoryId = @SubcategoryId";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@SubcategoryId", subcategoryId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ProductViewModel product = new ProductViewModel();
                            product.Id = reader.GetInt32(0);
                            product.Name = reader.GetString(1);
                            product.Price = reader.GetDecimal(2);

                            products.Add(product);
                        }
                    }
                }
            }

            return Json(products, JsonRequestBehavior.AllowGet);
        }


    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Subcategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //public string Description { get; set; }
        public decimal Price { get; set; }

    }
}


