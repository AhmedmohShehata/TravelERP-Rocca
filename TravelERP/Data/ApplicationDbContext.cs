using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelERP.Models;
using TravelERP.Models.ViewModel;

namespace TravelERP.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        public DbSet<MenuLE0> MenuLE0 { get; set; }
        public DbSet<MenuLE1> MenuLE1 { get; set; }
        public DbSet<MenuLE2> MenuLE2 { get; set; }
        public DbSet<MenuLZ0> MenuLZ0 { get; set; }
        public DbSet<MenuLZ1> MenuLZ1 { get; set; }
        public DbSet<MenuLZ2> MenuLZ2 { get; set; }


        public DbSet<CustomerSupplier> CustomersSuppliers { get; set; }
        public DbSet<CustomerOrSupplier> CustomerOrSuppliers { get; set; }


        public DbSet<Company> Companies { get; set; }

        public DbSet<BillAirLine> BillAirLines { get; set; }
        public DbSet<BillDomestic> BillDomestic { get; set; }
        public DbSet<BillReligious> BillReligious { get; set; }
        public DbSet<BillVisa> BillVisas { get; set; }


        public DbSet<Esal> Esals { get; set; }
        public DbSet<EznForEsal> EznsForEsals { get; set; }
        public DbSet<Ezn> Ezns { get; set; }


        public DbSet<PaymentMethod> paymentMethods { get; set; }


        public DbSet<TravelERP.Models.DatesSearch> DatesSearches { get; set; }


        public DbSet<OpeningBalance> OpeningBalances { get; set; }


        public DbSet<StatementType> StatementTypes { get; set; }


        public DbSet<TravelERP.Models.Developer> Developer { get; set; }


        public DbSet<TravelERP.Models.Personnel> Personnels { get; set; }
        public DbSet<TravelERP.Models.EznPersonnel> EznPersonnels { get; set; }
        public DbSet<TravelERP.Models.UnuserContactsInfo> UnuserContactsInfos { get; set; }

        public DbSet<TravelERP.Models.SalaryAddandCut> salaryAddandCuts { get; set; }

        public DbSet<TravelERP.Models.SalaryData> salaryDatas { get; set; }

        public DbSet<TravelERP.Models.SalaryAttendance> salaryAttendances { get; set; }

        public DbSet<TravelERP.Models.UsersDetail> UsersDetails { get; set; }


        public DbSet<TravelERP.Models.NonSupplier> NonSuppliers { get; set; }

        public DbSet<BillForeign> BillForeigns { get; set; }

        public DbSet<TransportMethod> TransportMethods { get; set; }

        public DbSet<TransportMethodTrip> TransportMethodTrips { get; set; }



    }
}
