using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.ViewModel
{
    public class AllBillsViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "رقم الفاتوره")]
        public int BillId { get; set; }

        public string BillType { get; set; }

        [Display(Name = "تاريخ التسجيل")]
        public DateTime BillDate { get; set; }

        [Display(Name = "تاريخ الاصدار")]
        public DateTime BillApprovedDate { get; set; }


        [Display(Name = "عميل او مورد!")]
        public string CustomerOrSupplierName { get; set; }
        //public CustomerOrSupplier CustomerOrSupplier { get; set; }


        [Display(Name = "الاسم")]
        public string CustomerSupplierName { get; set; }
        //public CustomerSupplier CustomerSupplier { get; set; }


        [Display(Name = "قائمه منسدله رئيسيه")]
        public string MenuLE0Name { get; set; }
        //public MenuLE0 MenuLE0 { get; set; }


        [Display(Name = "قائمه منسدله فرعيه 1")]
        public string MenuLE1Name { get; set; }
        //public MenuLE1 MenuLE1 { get; set; }


        [Display(Name = "قائمه منسدله فرعيه 2")]
        public string MenuLE2Name { get; set; }
        //public MenuLE2 MenuLE2 { get; set; }


        [Display(Name = "واجهه السفر")]
        public string Direction { get; set; }


        [Display(Name = "رقم الحجز")]
        public string PNR { get; set; }


        [Display(Name = "الشركه المصدره")]
        public string TicketExportName { get; set; }
        //public CustomerSupplier TicketExport { get; set; }

        [Display(Name = "عدد البالغين")]
        public int AdultN { get; set; }

        [Display(Name = "عدد الاطفال")]
        public int ChildN { get; set; }

        [Display(Name = "سعر البيع")]
        public int CustomerPrice { get; set; }
        [Display(Name = "سعر النت")]
        public int NetPrice { get; set; }

        [Display(Name = "العموله")]
        public float Commission { get; set; }

        [Display(Name = "تاريخ الذهاب")]
        [DataType(DataType.Date)]
        public DateTime TicketFrom { get; set; }
        [Display(Name = "تاريخ العوده")]
        [DataType(DataType.Date)]
        public Nullable<DateTime> TicketTo { get; set; }
        [Display(Name = "ملاحظات")]
        public string Commnets { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "اسم الموظف")]
        public string UserName { get; set; }
        [ScaffoldColumn(false)]
        [Display(Name = "اسم الشركه")]
        public int CompanyID { get; set; }
        public Company Company { get; set; }

        public bool BillState { get; set; }

    }
}
