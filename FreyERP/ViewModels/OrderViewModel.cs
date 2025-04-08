using Azure.Identity;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FreyERP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FreyERP.ViewModels
{
    public partial class OrderViewModel : ObservableObject
    {
        private readonly string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dummydaten", "orders.json");

        [ObservableProperty]
        private ObservableCollection<Order> orders = new();

        [ObservableProperty]
        private Order? selectedOrder;

        [ObservableProperty]
        private ObservableCollection<Customer> availableCustomers = new();

        [ObservableProperty]
        private ObservableCollection<Product> availableProducts = new();

        public OrderViewModel()
        {
            LoadAvailableProducts();
            LoadAvailableCustomers();
            LoadFromFile();
        }

        [RelayCommand]
        private void SaveOrder()
        {
            if (SelectedOrder is null) return;

            if (!IsValidOrder(SelectedOrder)) return;

            if (!HasSufficientStock(SelectedOrder)) return;

            ReserveProducts(SelectedOrder);
            PersistOrder(SelectedOrder);
            SaveToFile();

            MessageBox.Show("Bestellung gespeichert und Artikel reserviert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        [RelayCommand]
        private void SaveToFile()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataPath)!);
            var json = JsonSerializer.Serialize(Orders, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(dataPath, json);
        }
        [RelayCommand]
        private void LoadFromFile()
        {
            if (File.Exists(dataPath))
            {
                var json = File.ReadAllText(dataPath);
                var loaded = JsonSerializer.Deserialize<ObservableCollection<Order>>(json);
                if (loaded is not null) Orders = loaded;
            }
            else
            {
                //Erstes Mal? Dann bauen wir Thors Bestellung:
                Orders.Add(CreateDemoOrderForThor());
            }
        }
        [RelayCommand]
        private void AddPosition()
        {
            if (SelectedOrder is null) return;
            SelectedOrder.Positionen.Add(new OrderItem());
        }
        [RelayCommand]
        private void RemovePosition(object? parameter)
        {
            if (parameter is not OrderItem item || SelectedOrder is null) return;
            SelectedOrder.Positionen.Remove(item);
        }
        [RelayCommand]
        private void NewOrder()
        {
            SelectedOrder = new Order();
        }
        [RelayCommand]
        private void DeleteOrder()
        {
            if(SelectedOrder != null)
            {
                Orders.Remove(SelectedOrder);
                SaveToFile();
                SelectedOrder = null;
            }
        }

        private Order CreateDemoOrderForThor()
        {
            return new Order
            {
                Id = 1,
                CustomerId = 1,
                Kundenname = "Thor Odinson",
                Bestelldatum = DateTime.Today,
                Positionen = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = 1,
                        Bezeichnung = "Mjölnir - Hammer des Donners",
                        Einzelpreis = 999.99m,
                        Menge = 1
                    },
                    new OrderItem
                    {
                        ProductId = 2,
                        Bezeichnung = "Gungnir - Speer des Odin",
                        Einzelpreis = 1199.00m,
                        Menge = 1
                    }
                }
            };
        }
        private void LoadAvailableCustomers()
        {
            var customerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dummydaten", "customer.json");

            if (File.Exists(customerPath))
            {
                var json = File.ReadAllText(customerPath);
                var loaded = JsonSerializer.Deserialize<List<Customer>>(json);
                if (loaded is not null)
                    AvailableCustomers = new ObservableCollection<Customer>(loaded);
            }
        }
        private void LoadAvailableProducts()
        {
            var productPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dummydaten", "products.json");
            if (File.Exists(productPath))
            {
                var json = File.ReadAllText(productPath);
                var loaded = JsonSerializer.Deserialize<List<Product>>(json);
                if (loaded is not null)
                    AvailableProducts = new ObservableCollection<Product>(loaded);
            }
        }
        private bool IsValidOrder(Order order)
        {
            foreach (var pos in order.Positionen)
            {
                if (pos.ProductId == 0 || pos.Menge <= 0) 
                { 
                MessageBox.Show("Mindestens eine Position ist ungültig (kein Artikel oder Menge ≤ 0).", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
                }
                if (!AvailableProducts.Any(p => p.Id == pos.ProductId))
                {
                    MessageBox.Show($"Artikel mit ID {pos.ProductId} nicht gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            return true;
        }
        private bool HasSufficientStock(Order order)
        {
            foreach (var pos in order.Positionen)
            {
                var product = AvailableProducts.First(p => p.Id == pos.ProductId);
                var verfuegbar = product.Lagerbestand - product.Reserviert;

                if (verfuegbar < pos.Menge)
                {
                    MessageBox.Show($"Nicht genügend verfügbarer Bestand für {product.Bezeichnung}.\n" +
                            $"Verfügbar: {verfuegbar}, benötigt: {pos.Menge}",
                            "Nicht genug Bestand", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            return true;
        }
        private void ReserveProducts(Order order)
        {
            foreach (var pos in order.Positionen)
            {
                var product = AvailableProducts.First(p => p.Id == pos.ProductId);
                product.Reserviert += pos.Menge;
            }
        }
        private void PersistOrder(Order order)
        {
            var existing = Orders.FirstOrDefault(o => o.Id == order.Id);
            if (existing is not null)
            {
                var index = Orders.IndexOf(existing);
                Orders[index] = order;
            }
            else
            {
                var newId = Orders.Any() ? Orders.Max(o => o.Id) + 1 : 1;
                order.Id = newId;
                Orders.Add(order);
            }
                

        }
            
    }
}
