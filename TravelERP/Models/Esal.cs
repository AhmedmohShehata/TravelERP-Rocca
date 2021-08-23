using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace TravelERP.Models
{
    public class Esal
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Display(Name = "رقم الايصال")]
        public int EsalId { get; set; }


        [Required]
        [Display(Name = "تاريخ الايصال")]
        public DateTime EsalDate { get; set; }
        public int? BillId { get; set; }


        [Display(Name = "رقم الفاتوره")]
        public int? BillIdId { get; set; }


        [Required(ErrorMessage = "من فضلك اختر  الوكيل او العميل")]
        [Display(Name = "عميل او مورد!")]
        public int CustomerOrSupplierId { get; set; }
        public CustomerOrSupplier CustomerOrSupplier { get; set; }


        [Required(ErrorMessage = "من فضلك ادخل اسم الوكيل او العميل")]
        [Display(Name = "الاسم")]
        public int CustomerSupplierId { get; set; }
        public CustomerSupplier CustomerSupplier { get; set; }


        //[Required(ErrorMessage = "من فضلك اختر اسم الشركه المصدره")]
        [Display(Name = "الشركه المصدره")]
        public int? TicketExportId { get; set; }
        public CustomerSupplier TicketExport { get; set; }


        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه الرئيسيه")]
        [Display(Name = "قائمه رئيسيه")]
        public int MenuLE0Id { get; set; }
        public MenuLE0 MenuLE0 { get; set; }


        //[Required(ErrorMessage = "من فضلك ادخل اسم القائمه الفرعيه 1")]
        [Display(Name = "قائمه فرعيه 1")]
        public int? MenuLE1Id { get; set; }
        public MenuLE1 MenuLE1 { get; set; }


        //[Required(ErrorMessage = "من فضلك ادخل اسم القائمه الفرعيه 2")]
        [Display(Name = "قائمه فرعيه 2")]
        public int? MenuLE2Id { get; set; }
        public MenuLE2 MenuLE2 { get; set; }


        //[Required(ErrorMessage = "من فضلك ادخل وصف المبلغ")]
        [Display(Name = "وصف المبلغ")]
        public string DepositDesc { get; set; }


        [Required(ErrorMessage = "من فضلك ادخل مبلغ صحيح")]
        [Display(Name = "المبلغ المدفوع")]
        public int AmountPaid { get; set; }


        [Required(ErrorMessage = "من فضلك اختر وسيله الدفع")]
        [Display(Name = "وسيله الدفع")]
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
