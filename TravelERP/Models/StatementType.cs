using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class StatementType
    {
        public int Id { get; set; }

        [Display(Name ="رقم التصنيف")]
        public int NameId { get; set; }

        [Display(Name = "اسم التصنيف")]
        public string Name { get; set; }

    }
}
