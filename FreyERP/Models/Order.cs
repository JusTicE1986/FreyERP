using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyERP.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } //=> Verknüpfung zum Kunden
        public string? Kundenname { get; set; }
        public DateTime Bestelldatum { get; set; } = DateTime.Now;

        public List<OrderItem> Positionen { get; set; } = new();
        public decimal Gesamtsumme => Positionen.Sum(p => p.Gesamtpreis);
    }
}
