using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyERP.Models
{
    public class Lagerbewegung
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Produktbezeichnung { get; set; } = string.Empty;
        public DateTime Datum { get; set; } = DateTime.Now;
        public int Menge { get; set; }
        public MovementType Typ { get; set; } = MovementType.Zugang;
        public string Bemerkung { get; set; } = "";
    }
}
