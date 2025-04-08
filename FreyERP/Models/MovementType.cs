using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyERP.Models
{
    public enum MovementType
    {
        [Description("Zugang")]
        Zugang = 0,
        [Description("Abgang")]
        Abgang = 1,
        [Description("Korrektur")]
        Korrektur = 2
    }
}
