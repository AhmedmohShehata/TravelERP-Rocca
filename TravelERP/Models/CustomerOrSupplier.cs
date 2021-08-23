using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class CustomerOrSupplier
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك الاسم")]
        [Display(Name = "اسم المجموعه")]
        public string Name { get; set; }
    }
}
