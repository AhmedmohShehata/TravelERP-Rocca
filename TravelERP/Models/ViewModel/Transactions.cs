using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.ViewModel
{
    public class Transactions
    {
        public int Id { get; set; }

        [Display(Name = "#")]

        public int EId { get; set; }
        [Display(Name = "تصنيف")]

        public string Type { get; set; }

        [Display(Name = "التاريخ")]

        public DateTime EDate { get; set; }

        [Display(Name = "تاريخ الاصدار")]

        public DateTime ADate { get; set; }

        [Display(Name = "مدين")]

        public int Debit { get; set; }
        [Display(Name = "دائن")]

        public int Credit { get; set; }
        [Display(Name = "بيان")]

        public string Statement { get; set; }

        [Display(Name = "اسم العميل")]

        public string CustomerName { get; set; }

        [Display(Name = "الشركه المصدره")]

        public string TicketExportName { get; set; }

        [Display(Name = "عميل او شركه")]

        public string CustomerOrSupplierName { get; set; }

        [Display(Name = "خط السير")]

        public string Direction { get; set; }

        [Display(Name = "رقم التكيت")]

        public string eTicketNumber { get; set; }

        [Display(Name = "PNR")]

        public string PNR { get; set; }

        [Display(Name = "بحث 1")]

        public int? Name1 { get; set; }

        [Display(Name = "بحث 2")]

        public int? Name2 { get; set; }

        [Display(Name = "بحث 3")]

        public string Name3 { get; set; }

        public float Commission { get; set; }



    }
}
