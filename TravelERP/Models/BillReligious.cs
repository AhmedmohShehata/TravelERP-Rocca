using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class BillReligious
    {
        public int Id { get; set; }


        [Required]
        [Display(Name = "رقم الفاتوره")]
        [ScaffoldColumn(false)]
        public int BillId { get; set; }


        [Display(Name = "تاريخ الاصدار")]
        [ScaffoldColumn(false)]
        public DateTime BillDate { get; set; }


        //[Required(ErrorMessage = "من فضلك اختر اسم الوكيل او العميل")]
        [Display(Name = "عميل او مورد!")]
        public int CustomerOrSupplierId { get; set; }
        public CustomerOrSupplier CustomerOrSupplier { get; set; }


        [Required(ErrorMessage = "من فضلك اختر اسم الوكيل او العميل")]
        [Display(Name = "اسم المندوب")]
        public int CustomerSupplierId { get; set; }
        public CustomerSupplier CustomerSupplier { get; set; }


        //[Required(ErrorMessage = "من فضلك ادخل اسم القائمه")]
        [Display(Name = "قائمه منسدله رئيسيه")]
        public int MenuLE0Id { get; set; }
        public MenuLE0 MenuLE0 { get; set; }


        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه الفرعيه 1")]
        [Display(Name = "قائمه منسدله فرعيه 1")]
        public int MenuLE1Id { get; set; }
        public MenuLE1 MenuLE1 { get; set; }


        [Required(ErrorMessage = " من فضلك ادخل اسم القائمه الفرعيه 2")]
        [Display(Name = "قائمه منسدله فرعيه 2")]
        public int? MenuLE2Id { get; set; }
        public MenuLE2 MenuLE2 { get; set; }


        ////[Required(ErrorMessage = "من فضلك ادخل واجهه السفر")]
        //[Display(Name = "واجهه السفر")]
        //public string Direction { get; set; }


        //[Display(Name = "رقم الحجز")]
        //public string PNR { get; set; }


        ////[Required(ErrorMessage = "من فضلك اختر اسم الشركه المصدره")]
        //[Display(Name = "الشركه المصدره")]
        //public int TicketExportId { get; set; }
        //public CustomerSupplier TicketExport { get; set; }

        ////[Required(ErrorMessage = "من فضلك ادخل عدد صحيح")]
        //[Display(Name = "عدد البالغين")]
        //public int AdultN { get; set; }
        ////[Required(ErrorMessage = "من فضلك ادخل عدد صحيح")]
        //[Display(Name = "عدد الاطفال")]
        //public int ChildN { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل مبلغ صحيح")]
        [Display(Name = "المبلغ")]
        public int CustomerPrice { get; set; }
        //[Display(Name = "سعر النت")]
        //public int NetPrice { get; set; }
        //[Required(ErrorMessage ="من فضلك ادخل تاريخ الذهاب")]
        //[Display(Name = "تاريخ الذهاب")]
        //[DataType(DataType.Date)]
        //public DateTime TicketFrom { get; set; }
        //[Display(Name = "تاريخ العوده")]
        //[DataType(DataType.Date)]
        //public Nullable<DateTime> TicketTo { get; set; }
        [Display(Name = "ملاحظات")]
        public string Commnets { get; set; }


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
