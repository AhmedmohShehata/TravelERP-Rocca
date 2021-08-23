using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.ViewModel
{
    public class CreateRoleViewModel
    {
       
        [Required (ErrorMessage ="لم يتم ادخال اسم صلاحيه")]
        [Display(Name ="إسم الصلاحيه")]
        public string RoleName { get; set; }
    }
}
