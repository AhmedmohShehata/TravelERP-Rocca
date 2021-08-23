using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.ViewModel
{
    public class EznsHomePageViewModel
    {
        public int Id { get; set; }

        [Display(Name = "رقم الاذن")]
        public int EznId { get; set; }


        [Display(Name = "تاريخ الاذن")]
        public string EznDate { get; set; }


        [Display(Name = "يصرف للسيد")]
        public string Name { get; set; }


        [Display(Name = "بيان المصروف")]
        public string ExpenseName { get; set; }


        [Display(Name = "المبلغ")]
        public int AmountWithdrawn { get; set; }

        [Display(Name = "وسيله السحب")]
        [StringLength(20)]
        public string PaymentMethod { get; set; }

    }
}
