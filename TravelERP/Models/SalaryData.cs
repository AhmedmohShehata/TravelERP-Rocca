using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class SalaryData
    {
        public int Id { get; set; }

        [Display(Name = "اسم الموظف")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [Display(Name = "الاسم فى جهاز البصمه")]
        public string EMP_FingerPrintName { get; set; }

        [Display(Name = "الاسم لربط السلفيات")]
        public string EMP_Name { get; set; }

        [Display(Name = "المرتب الاساسى")]
        public int EMP_Salary { get; set; }

        [Display(Name = "ق ساعه الاضافى")]
        public int EMP_OverTime { get; set; }

        [Display(Name = "ق ساعه الاذن")]
        public int EMP_Early { get; set; }

        [Display(Name = "ق ساعه التأخير")]
        public int EMP_Late { get; set; }

        [Display(Name = "ق يوم الغياب")]
        public int EMP_Absent { get; set; }

        [Display(Name = "ق بدل الواتس اب")]
        public int EMP_WhatsApp { get; set; }

        [Display(Name = "خصم التأمين")]
        public int EMP_insurance { get; set; }

        [ScaffoldColumn(false)]
        public int? CompanyId { get; set; }

        public Company Company { get; set; }



    }
}
