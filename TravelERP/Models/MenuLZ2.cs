using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class MenuLZ2
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه")]
        [StringLength(50)]
        [Display(Name = "قائمه منسدله فرعيه 2")]
        public string M2_Name { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه")]
        [Display(Name = "قائمه منسدله فرعيه 1")]
        public int MenuLZ1Id { get; set; }
        public MenuLZ1 MenuLZ1 { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه")]
        [Display(Name = "قائمه منسدله رئيسيه")]
        public int MenuLZ0Id { get; set; }
        public MenuLZ0 MenuLZ0 { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "اسم الشركه")]
        public int? CompanyId { get; set; }
        public Company Company { get; set; }


    }
}
