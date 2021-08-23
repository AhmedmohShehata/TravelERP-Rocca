using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class SalaryAddandCut
    {
        public int Id { get; set; }
        [Display(Name = "اسم الموظف")]
        public string EMP_Name { get; set; }

        [Display(Name = "من تاريخ")]
        [DataType(DataType.Date)]
        public DateTime FromDate { get; set; }

        [Display(Name = "الى تاريخ")]
        [DataType(DataType.Date)]
        public DateTime ToDate { get; set; }

        [Display(Name = "عموله حج وعمره")]
        public int Omra_Add { get; set; }

        [Display(Name = "مكافأه")]
        public int Bonus_Add { get; set; }

        [Display(Name = "خصم الكترونى")]
        public int Whatsapp_Cut { get; set; }

        [Display(Name = "خصم اضافى")]
        public int Other_Cut { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

    }
}
