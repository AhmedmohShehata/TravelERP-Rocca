using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="من فضلك ادخل اسم طريقه الدفع")]
        [Display(Name ="اسم طريقه الدفع")]
        public string Name { get; set; }
    }
}
