using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyERP.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Artikelnummer { get; set; } = string.Empty;
        public string Bezeichnung { get; set; } = string.Empty;
        public string Einheit { get; set; } = "Stk.";

        public decimal Einkaufspreis { get; set; }
        public decimal Verkaufspreis { get; set; }
        public TaxCategory MwstKategorie { get; set; } = TaxCategory.Normal;
        public decimal Bruttopreis => Verkaufspreis * (1 + (decimal)MwstKategorie / 100);

        public int Lagerbestand { get; set; }
        public int Reserviert { get; set; }
        public int Verfügbar => Lagerbestand - Reserviert;

    }
}
