using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.ViewModel
{
    public class BillsHomePageViewModel
    {
        public int Id { get; set; }
        [Display (Name = "#")]
        public int BillId { get; set; }

        [Display(Name = "التاريخ")]
        public string BillDate { get; set; }

        [Display(Name = "اسم العميل و الشركه")]
        public string CustomerSupplier { get; set; }

        [Display(Name = "التفاصيل")]
        public string Details { get; set; }

        [Display(Name = "رقم الحجز والتكت")]
        public string TicketNo { get; set; }

        [Display(Name = "سعر النت")]
        public int NetPrice { get; set; }

        [Display(Name = "سعر العميل")]
        public int CustomerPrice { get; set; }

        [Display(Name = "العموله")]
        public float EMPCommission { get; set; }

        [Display(Name = "الشركه المنفذه")]
        public string TicketExport { get; set; }
    }
}
