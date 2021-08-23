using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.ViewModel
{
    public class EsalsHomePageViewModel
    {
        public int Id { get; set; }

        [Display(Name = "رقم الايصال")]
        public int EsalId { get; set; }

        [Display(Name = "تاريخ الايصال")]
        [StringLength(11)]
        public string EsalDate { get; set; }

        [Display(Name = "الاسم")]
        public string CustomerSupplier { get; set; }

        [Display(Name = "رقم الفاتوره")]
        public int? BillIdId { get; set; }

        [Display(Name = "قائمه رئيسيه")]
        public string MenuLE0 { get; set; }

        [Display(Name = "المبلغ المدفوع")]
        public int AmountPaid { get; set; }

        [Display(Name = "وسيله الدفع")]
        public string PaymentMethod { get; set; }

    }
}
