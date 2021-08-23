using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.ViewModel
{
    public class Ezns
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "رقم الاذن")]
        public int EznId { get; set; }


        [Required]
        [Display(Name = "تاريخ الاذن")]
        public DateTime EznDate { get; set; }


        [Display(Name = "يصرف للسيد")]
        public string Name { get; set; }


        [Display(Name = "قائمه منسدله رئيسيه")]
        public String Menu0 { get; set; }


        [Display(Name = "قائمه منسدله فرعيه 1")]
        public String Menu1 { get; set; }


        [Display(Name = "قائمه منسدله فرعيه 2")]
        public String Menu2 { get; set; }


        [Display(Name = "بيان المصروف")]
        public string ExpenseName { get; set; }


        [Required]
        [Display(Name = "المبلغ")]
        public int AmountWithdrawn { get; set; }

        [Required]
        [Display(Name = "وسيله السحب")]
        public string PaymentMethod { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "اسم الموظف")]
        public string UserName { get; set; }

        [Display(Name = "رقم الشركه")]
        public int CompanyId { get; set; }

        [Display(Name = "اسم الشركه")]
        public string CompanyName { get; set; }

        public Company Company { get; set; }
    }
}
