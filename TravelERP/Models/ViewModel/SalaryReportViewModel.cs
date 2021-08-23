using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models.ViewModel
{
    public class SalaryReportViewModel
    {
        public int Id { get; set; }

        [Display(Name = "اسم الموظف")]  
        public string EMP_Name { get; set; }


        [Display(Name = "المرتب الاساسى")]
        public int EMP_Salary { get; set; }


        [Display(Name = "خدمه واتس اب")]
        public int EMP_WhatsApp { get; set; }


        [Display(Name = "اوفر تايم")]
        public float EMP_OverTime { get; set; }


        [Display(Name = "عموله 4 %")]
        public float Commission { get; set; }


        [Display(Name = "عموله حج وعمره")]
        public float Omra_Add { get; set; }


        [Display(Name = "مكافأه")]
        public float Bonus_Add { get; set; }



        [Display(Name = "خصم تأخير")]
        public float EMP_Late { get; set; }


        [Display(Name = "خصم الاذن")]
        public float EMP_Early { get; set; }


        [Display(Name = "خصم غياب")]
        public float EMP_Absent { get; set; }


        [Display(Name = "خصم الكترونى")]
        public int Whatsapp_Cut { get; set; }


        [Display(Name = "خصم ت")]
        public float EMP_insurance { get; set; }


        [Display(Name = "خصم سلفيات")]
        public int loans { get; set; }


        [Display(Name = "خصم اضافى")]
        public int Other_Cut { get; set; }


    }
}
