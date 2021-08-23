using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelERP.Models
{
    public class SalaryAttendance
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Normal { get; set; }

        public int Actual { get; set; }

        public int Late { get; set; }

        public int Early { get; set; }

        public int Absent { get; set; }

        public int OT { get; set; }

        public int CompanyId { get; set; }

        public Company Company { get; set; }


    }
}
