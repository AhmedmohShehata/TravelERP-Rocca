using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class Developer
    {
        public int Id { get; set; }
        [Display(Name ="العنوان")]
        public string Title { get; set; }
        [Display(Name ="الرساله")]
        public string Message { get; set; }
    }
}
