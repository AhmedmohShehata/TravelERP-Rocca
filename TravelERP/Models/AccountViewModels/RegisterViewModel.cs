using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "اسم المستخدم")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "البريد الالكترونى")]
        public string Email { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل رقم الهاتف")]
        [Range(0, int.MaxValue, ErrorMessage = "رقم هاتف غير صحيح")]
        [Display(Name = "رقم الهاتف 1")]
        public string PhoneNumber1 { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "رقم هاتف غير صحيح")]
        [Display(Name = "رقم الهاتف 2")]
        public string PhoneNumber2 { get; set; }
        [Display(Name = "اسم الشركه او الفرع")]
        public int CompanyId { get; set; }
        public Company Company { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "الرقم السرى")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "تأكيد الرقم السرى")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
