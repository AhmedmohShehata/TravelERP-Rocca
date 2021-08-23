using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "من فضلك ادخل اسم الشركه")]
        [Display(Name = "اسم الشركه - عربى")]
        public String Company_Name { get; set; }
        [Display(Name = "اسم الشركه - انجليزى")]
        public String Company_NameE { get; set; }

        [Display(Name = "عنوان الشركه")]
        public String Company_Address { get; set; }
        [Display(Name = "ارقام هواتف الشركه")]
        public String Company_PhonesNumber { get; set; }

        [Display(Name = "لوجو الشركه")]
        public string CompanyLogo { get; set; }
        [Display(Name = "صوره خلفيه ايصال الشركه")]
        public string CompanyA4EsalImage { get; set; }

        [Display(Name ="توقيت البلد")]
        [StringLength(150)]
        public string DateTimeCountry { get; set; }

    }
}
