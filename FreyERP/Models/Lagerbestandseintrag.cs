using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyERP.Models
{
    public class Lagerbestandseintrag //Tracker für FIFO-Verfolgung
    {
        public int ProductId { get; set; }
        public int Menge { get; set; }
        public DateTime Zugang { get; set; } = DateTime.Now;
    }
}
