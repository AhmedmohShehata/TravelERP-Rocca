using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class EznForEsal
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "رقم الاذن")]
        public int EznId { get; set; }


        [Required]
        [Display(Name = "تاريخ الاذن")]
        public DateTime EznDate { get; set; }


        [Display(Name = "عميل او مورد!")]
        public int CustomerOrSupplierId { get; set; }
        public CustomerOrSupplier CustomerOrSupplier { get; set; }


        [Display(Name = "الاسم")]
        public int CustomerSupplierId { get; set; }
        public CustomerSupplier CustomerSupplier { get; set; }


        [Display(Name = "قائمه منسدله رئيسيه")]
        public int MenuLE0Id { get; set; }
        public MenuLE0 MenuLE0 { get; set; }


        //[Display(Name = "قائمه منسدله فرعيه 1")]
        //public int MenuLE1Id { get; set; }
        //public MenuLE1 MenuLE1 { get; set; }


        //[Display(Name = "قائمه منسدله فرعيه 2")]
        //public int MenuLE2Id { get; set; }
        //public MenuLE2 MenuLE2 { get; set; }


        [Display(Name = "بيان المصروف")]
        public string ExpenseName { get; set; }


        [Required]
        [Display(Name = "المبلغ")]
        public int AmountWithdrawn { get; set; }

        [Required]
        [Display(Name = "وسيله السحب")]
        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }


        [ScaffoldColumn(false)]
        [Display(Name = "اسم الموظف")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }



        [ScaffoldColumn(false)]
        [Display(Name = "اسم الشركه")]
        public int CompanyID { get; set; }
        public Company Company { get; set; }


    }
}
