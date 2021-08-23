using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class TransportMethodTrip
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه")]
        [Display(Name = "قائمه منسدله رئيسيه")]
        public int MenuLE0Id { get; set; }
        public MenuLE0 MenuLE0 { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه")]
        [Display(Name = "قائمه منسدله فرعيه 1")]
        public int MenuLE1Id { get; set; }
        public MenuLE1 MenuLE1 { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم القائمه")]
        [Display(Name = "قائمه منسدله فرعيه 2")]
        public int MenuLE2Id { get; set; }
        public MenuLE2 MenuLE2 { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل عدد المقاعد")]
        [Display(Name = "عدد المقاعد")]

        public int SeatsCount { get; set; }

        [Display(Name = "وسيله النقل")]
        public bool IsBus { get; set; }

    }
}
