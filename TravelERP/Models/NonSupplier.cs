using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class NonSupplier
    {
        public int Id { get; set; }

        [Display(Name = "الرقم البرمجى")]
        public int CustomerSupplierId { get; set; }
        public CustomerSupplier CustomerSupplier { get; set; }


        [ScaffoldColumn(false)]
        [Display(Name = "اسم الشركه")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

    }
}
