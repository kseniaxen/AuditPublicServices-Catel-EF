using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PublicServicesDomain.Models;

namespace PublicServicesDomain
{
    public class PSDBContext : DbContext
    {
        public PSDBContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PublicServicesDB;Trusted_Connection=True;");
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<VolumeIndication> VolumeIndications { get; set; }
        public DbSet<ArbitraryAmount> ArbitraryAmounts { get; set; }
    }
}
