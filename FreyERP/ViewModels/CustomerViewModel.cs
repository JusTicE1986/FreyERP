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

namespace FreyERP.ViewModels
{
    public partial class CustomerViewModel : ObservableObject
    {
        #region temporäres Daten Speichern und Laden
        private readonly string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Dummydaten","customer.json");

        [RelayCommand]
        private void SaveToFile()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataPath)!);
            var json = JsonSerializer.Serialize(Customers);
            File.WriteAllText(dataPath, json);
        }
        [RelayCommand]
        private void LoadFromFile()
        {
            if (File.Exists(dataPath))
            {
                var json = File.ReadAllText(dataPath);
                var loaded = JsonSerializer.Deserialize<ObservableCollection<Customer>>(json);
                if (loaded is not null) Customers = loaded;
            }
        }

        #endregion
        public CustomerViewModel()
        {
            LoadFromFile();
        }

        // Kundenliste
        [ObservableProperty]
        private ObservableCollection<Customer> customers = new();

        // Aktuell ausgewählter Kunde
        [ObservableProperty]
        private Customer? selectedCustomer;

        [RelayCommand]
        private void NewCustomer()
        {
            SelectedCustomer = new Customer();
        }
        [RelayCommand]
        private void SaveCustomer()
        {
            if (SelectedCustomer is null) return;

            // Bestehenden Eintrag ersetzen oder neuen hinzufügen
            var existing = Customers.FirstOrDefault(c => c.Id == SelectedCustomer.Id);

            if(existing is not null)
            {
                //Update bestehender Eintrag
                var index = Customers.IndexOf(existing);
                Customers[index] = SelectedCustomer;
            }
            else
            {
                //Neue ID vergeben (nur lokal, noch keine DB-Logik)
                var newId = Customers.Any() ? Customers.Max(c => c.Id) + 1 : 1;
                SelectedCustomer.Id = newId;
                Customers.Add(SelectedCustomer);
            }

            //Optional: Auto-Speichern nach jeder Änderung
            SaveToFile();
        }
    }
}
