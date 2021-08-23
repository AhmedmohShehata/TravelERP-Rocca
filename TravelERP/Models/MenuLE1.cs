using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class MenuLE1
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه")]
        [StringLength(50)]
        [Display(Name = "قائمه منسدله فرعيه 1")]
        public string M1_Name { get; set; }

        [Display(Name = "قائمه منسدله رئيسيه")]
        public int MenuLE0Id { get; set; }
        public MenuLE0 MenuLE0 { get; set; }

    }
}
