using FreyERP.Models;
using FreyERP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FreyERP.Views
{
    /// <summary>
    /// Interaktionslogik für OrdersView.xaml
    /// </summary>
    public partial class OrdersView : UserControl
    {
        public OrdersView()
        {
            InitializeComponent();
        }
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox &&
                comboBox.SelectedItem is Product selectedProduct &&
                comboBox.DataContext is OrderItem item)
            {
                item.Bezeichnung = selectedProduct.Bezeichnung;
                item.Einzelpreis = selectedProduct.Verkaufspreis;
            }
        }

        private void CustomerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender is ComboBox comboBox &&
                comboBox.SelectedItem is Customer selectedCustomer &&
                DataContext is OrderViewModel vm &&
                vm.SelectedOrder is not null)
            {
                vm.SelectedOrder.Kundenname = selectedCustomer.FullName;
            }
        }
    }
}
