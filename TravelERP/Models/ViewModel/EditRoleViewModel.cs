using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.ViewModel
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        [Display (Name = "معرف الصلاحيه")]
        public string Id { get; set; }
        [Display(Name = "اسم الصلاحيه")]

        [Required(ErrorMessage ="من فضلك ادخل اسم صلاحيه")]
        public string RoleName { get; set; }

        public List<string> Users { get; set; }
    }
}
