using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class UnuserContactsInfo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل الاسم")]
        [Display(Name = "الاسم")]

        public string Name { get; set; }

        [Display(Name = "البريد الالكترونى")]
        public string Email { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل رقم الهاتف")]
        [Range(0, int.MaxValue, ErrorMessage = "رقم هاتف غير صحيح")]
        [Display(Name = "رقم الهاتف")]

        public string PhoneNumber { get; set; }

        [Display(Name = "خطه الاشتراك")]
        [Required(ErrorMessage = "من فضلك ادخل الخطه")]
        public string subscribe { get; set; }
    }
}
