using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.ViewModel
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }
        [Display(Name = "معرف المستخدم")]
        public string Id { get; set; }

        [Display(Name = "اسم المستخدم")]
        [Required(ErrorMessage = "من فضلك ادخل اسم مستخدم")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "اسم الشركه")]

        public int CompanyId { get; set; }

        public List<string> Claims { get; set; }

        public IList<string> Roles { get; set; }
    }
}
