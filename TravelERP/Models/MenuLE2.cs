using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class MenuLE2
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه")]
        [StringLength(50)]
        [Display(Name = "قائمه منسدله فرعيه 2")]
        public string M2_Name { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه")]
        [Display(Name = "قائمه منسدله فرعيه 1")]
        public int MenuLE1Id { get; set; }
        public MenuLE1 MenuLE1 { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه")]
        [Display(Name = "قائمه منسدله رئيسيه")]
        public int MenuLE0Id { get; set; }
        public MenuLE0 MenuLE0 { get; set; }

    }
}
