using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class DatesSearch
    {
        public int Id { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Name1 { get; set; }
        public int Name2 { get; set; }
        public int Name3 { get; set; }
        public int Name4 { get; set; }
        public int Name5 { get; set; }
        public int Name6 { get; set; }
        public string Name7 { get; set; }


    }
}
