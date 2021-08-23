using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.ViewModel
{
    public class BillCountViewModel
    {
        public int Id { get; set; }

        public int Count { get; set; }

        public String CustomerSupplierName { get; set; }

        public string CompanyName { get; set; }
    }
}
