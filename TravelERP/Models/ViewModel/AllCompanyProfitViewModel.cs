using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.ViewModel
{
    public class AllCompanyProfitViewModel
    {
        public int Id { get; set; }

        [Display(Name ="اسم الشركه")]
        public String CompanyName { get; set; }

        [Display(Name = "حركه الخزينه")]
        public int StatementDate { get; set; }

        [Display(Name = "مبيعات اليوم")]
        public float BillToday { get; set; }

        [Display(Name = "مبيعات الشهر الحالى")]
        public float BillProfitThisMonth { get; set; }

    }
}
