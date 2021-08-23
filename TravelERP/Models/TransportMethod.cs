using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class TransportMethod
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "من فضلك ادخل اسم وسيله السفر")]
        [Display(Name = "اسم وسيله السفر")]
        public string Name { get; set; }
    }
}
