using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class OpeningBalance
    {
        public int Id { get; set; }

        [Display(Name ="اسم المجموعه")]
        [Required(ErrorMessage ="قم بأختيار اسم مجموعه")]
        public int CustomerOrSupplierId { get; set; }
        public CustomerOrSupplier CustomerOrSupplier { get; set; }

        [Display(Name = "الاسم")]
        [Required(ErrorMessage = "قم بأختيار اسم ")]
        public int CustomerSupplierId { get; set; }
        public CustomerSupplier CustomerSupplier { get; set; }

        [Display(Name = "مجموعه التقارير")]
        [Required(ErrorMessage = "قم بأختيار مجموعه التقارير ")]
        public int StatementTypeId { get; set; }
        public StatementType StatementType { get; set; }

        [Display(Name = "التصنيف")]
        [Required(ErrorMessage = "قم بأختيار تصنيف ")]
        public int MenuLE0Id { get; set; }
        public MenuLE0 MenuLE0 { get; set; }

        [Required(ErrorMessage = "قم بكتابه مبلغ")]
        [Display(Name = "المبلغ")]
        public int Balance { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "اسم الشركه")]
        public int CompanyID { get; set; }
        public Company Company { get; set; }

    }
}
