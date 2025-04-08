using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FreyERP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FreyERP.ViewModels
{
    public partial class LagerbewegungViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Product> availableProducts = new();
        [ObservableProperty]
        private ObservableCollection<Lagerbewegung> lagerbewegungen = new();
        [ObservableProperty]
        private Lagerbewegung currentMovement = new();

        private readonly List<Lagerbestandseintrag> fifoBestand = new();

        [RelayCommand]
        private void Buchen()
        {
            if (!ValidateMovement(CurrentMovement)) return;

            var product = AvailableProducts.First(p => p.Id == CurrentMovement.ProductId);

            switch (CurrentMovement.Typ)
            {
                case MovementType.Zugang:
                    BuchenZugang(product, CurrentMovement);
                    break;
                case MovementType.Abgang:
                    BuchenAbgang(product, CurrentMovement);
                    break;
                case MovementType.Korrektur:
                    BuchenKorrektur(product, currentMovement);
                    break;
            }

            Lagerbewegungen.Add(CurrentMovement);
            CurrentMovement = new Lagerbewegung();
        }

        private bool ValidateMovement(Lagerbewegung bewegung)
        {
            if (bewegung.ProductId == 0 || bewegung.Menge <= 0)
            {
                MessageBox.Show("Bitte wähle einen Artikel und gib eine gültige Menge an.",
                            "Ungültige Eingabe", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            var product = AvailableProducts.FirstOrDefault(p => p.Id == bewegung.ProductId);
            
            if(product is null)
            {
                MessageBox.Show("Produkt nicht gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (bewegung.Typ == MovementType.Abgang && product.Lagerbestand < bewegung.Menge)
            {
                MessageBox.Show($"Nicht genügend Bestand für {product.Bezeichnung}.\nVerfügbar: {product.Lagerbestand}",
                        "Bestandsfehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void BuchenZugang(Product product, Lagerbewegung bewegung)
        {
            product.Lagerbestand += bewegung.Menge;
            fifoBestand.Add(new Lagerbestandseintrag
            {
                ProductId = product.Id,
                Menge = bewegung.Menge,
                Zugang = bewegung.Datum
            });
        }

        private void BuchenAbgang(Product product, Lagerbewegung bewegung)
        {
            product.Lagerbestand -= bewegung.Menge;
            ReduziereFIFO(product.Id, bewegung.Menge);
        }

        private void BuchenKorrektur(Product product, Lagerbewegung bewegung)
        {
            product.Lagerbestand += bewegung.Menge;
        }

        private void ReduziereFIFO(int productId, int menge)
        {
            var eintraege = fifoBestand
                .Where(e => e.ProductId == productId)
                .OrderBy(e => e.Zugang)
                .ToList();

            foreach (var eintrag in eintraege)
            {
                if (menge <= 0) break;
                if (eintrag.Menge <= menge)
                {
                    menge -= eintrag.Menge;
                    eintrag.Menge = 0;
                }
                else
                {
                    eintrag.Menge -= menge;
                    menge = 0;
                }
            }
            fifoBestand.RemoveAll(e => e.Menge == 0);
        }



    }
}
