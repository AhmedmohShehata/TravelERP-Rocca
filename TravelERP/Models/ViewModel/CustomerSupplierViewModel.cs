using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.ViewModel
{
    public class CustomerSupplierViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل الاسم")]
        [Display(Name = "الاسم")]
        [StringLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل رقم الهاتف")]
        [Range(0, int.MaxValue, ErrorMessage = "رقم هاتف غير صحيح")]
        [Display(Name = "رقم الهاتف 1")]
        public string PhoneNumber1 { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "رقم هاتف غير صحيح")]
        [Display(Name = "رقم الهاتف 2")]
        public string PhoneNumber2 { get; set; }

        [Display(Name = "رقم جواز السفر")]
        public string PassportNo { get; set; }

        [Display(Name = "تصنيف")]
        public string CustomerOrSupplier { get; set; }
    }
}
