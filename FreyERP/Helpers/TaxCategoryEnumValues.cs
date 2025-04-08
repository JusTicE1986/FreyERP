using FreyERP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyERP.Helpers
{
    public static class TaxCategoryEnumValues
    {
        public static IEnumerable<TaxCategory> All => Enum.GetValues(typeof(TaxCategory)).Cast<TaxCategory>();
    }
}
