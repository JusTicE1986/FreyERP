using CommunityToolkit.Mvvm.ComponentModel;
using FreyERP.Models;
using FreyERP.Services;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyERP.ViewModels
{
    public partial class LagerUebersichtViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Product> products = new();

        public LagerUebersichtViewModel()
        {
            Products = new ObservableCollection<Product>(ProductService.LoadAll());
        }
    }
}
