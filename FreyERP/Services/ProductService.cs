using FreyERP.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreyERP.Services
{
    public static class ProductService
    {
        public static List<Product> LoadAll()
        {
            try
            {
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dummydaten", "products.json");

                if (!File.Exists(path))
                    return new List<Product>();

                var json = File.ReadAllText(path);
                var list = JsonSerializer.Deserialize<List<Product>>(json);
                return list ?? new List<Product>();
            }
            catch
            {
                return new List<Product>();
            }
        }
    }

}
