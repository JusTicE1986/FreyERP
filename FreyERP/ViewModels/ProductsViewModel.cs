using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FreyERP.Models;
using FreyERP.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FreyERP.ViewModels
{
    public partial class ProductsViewModel : ObservableObject
    {
        private readonly string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dummydaten", "products.json");

        [ObservableProperty]
        private ObservableCollection<Product> products = new();

        [ObservableProperty]
        private Product? selectedProduct;

        public ProductsViewModel()
        {
            LoadFromFile();
        }

        [RelayCommand]
        private void SaveProduct()
        {
            if (SelectedProduct is null) return;
            var existing = Products.FirstOrDefault(p => p.Id == SelectedProduct.Id);
            if(existing is not null)
            {
                var index = Products.IndexOf(existing);
                Products[index] = SelectedProduct;
            }
            else
            {
                var newId = Products.Any() ? Products.Max(p => p.Id) + 1 : 1;
                SelectedProduct.Id = newId;
                Products.Add(SelectedProduct);
            }

            SaveToFile();
        }

        [RelayCommand]
        private void SaveToFile()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataPath)!);
            var json = JsonSerializer.Serialize(Products);
            File.WriteAllText(dataPath, json);
        }

        [RelayCommand]
        private void LoadFromFile()
        {
            if (File.Exists(dataPath))
            {
                var json = File.ReadAllText(dataPath);
                var loaded = JsonSerializer.Deserialize<ObservableCollection<Product>>(json);
                if (loaded is not null) Products = new ObservableCollection<Product>(ProductService.LoadAll()); ;
            }
        }

        [RelayCommand]
        private void DeleteProduct()
        {
            if (SelectedProduct != null)
            {
                Products.Remove(SelectedProduct);
                SaveToFile();
                SelectedProduct = null;
            }
        }
    }
}
