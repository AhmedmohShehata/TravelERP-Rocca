using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class MenuLZ1
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه")]
        [StringLength(50)]
        [Display(Name = "قائمه منسدله فرعيه 1")]
        public string M1_Name { get; set; }

        [Display(Name = "قائمه منسدله رئيسيه")]
        public int MenuLZ0Id { get; set; }
        public MenuLZ0 MenuLZ0 { get; set; }

    }
}
