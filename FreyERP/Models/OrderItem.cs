using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyERP.Models
{
    public class OrderItem
    {
        public int ProductId { get; set; } //=> Referenz zum Artikel
        public string Bezeichnung { get; set; } = string.Empty;
        public decimal Einzelpreis { get; set; }
        public int Menge { get; set; }

        public decimal Gesamtpreis => Einzelpreis * Menge;
    }
}
