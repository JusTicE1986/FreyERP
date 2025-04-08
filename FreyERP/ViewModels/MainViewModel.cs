using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyERP.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private object currentView;

        public MainViewModel()
        {
            ShowDashboard();
        }

        [RelayCommand]
        private void ShowDashboard() => CurrentView = new Views.DashboardView();
        [RelayCommand]
        private void ShowCustomers() => CurrentView = new Views.CustomerView();
        [RelayCommand]
        private void ShowOrders() => CurrentView = new Views.OrdersView();
        [RelayCommand]
        private void ShowProducts() => CurrentView = new Views.ProductsView();
        [RelayCommand]
        private void ShowStock() => CurrentView = new Views.StockView();
        private void ShowInvoices() => CurrentView = new Views.InvoiceView();
        private void ShowFinance() => CurrentView = new Views.FinanceView();
        private void ShowEmployees() => CurrentView = new Views.EmployeesView();
        private void ShowDocuments() => CurrentView = new Views.DocumentsView();
        private void ShowBackup() => CurrentView = new Views.BackupView();
        private void ShowSettings() => CurrentView = new Views.SettingsView();
    }
}
