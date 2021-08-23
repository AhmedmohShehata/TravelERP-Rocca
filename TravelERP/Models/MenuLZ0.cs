using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class MenuLZ0
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه")]
        [StringLength(50)]
        [Display(Name = "قائمه منسدله رئيسيه")]
        public string M0_Name { get; set; }


    }
}
