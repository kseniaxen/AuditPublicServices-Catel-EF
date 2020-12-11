using PublicServicesDomain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicServicesDomain
{
    public class PSDBContext : DbContext
    {
        //public PSDBContext():base("DbConnection")
        //{

        //}
        static string connectionString = "data source=(localdb)\\MSSQLLocalDB;Initial Catalog=PubServDB;Integrated Security=True;";
        public PSDBContext() : base(connectionString)
        {
            
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.Login).HasMaxLength(20);
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<ArbitraryAmount> ArbitraryAmounts { get; set; }
        public virtual DbSet<Rate> Rates { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<VolumeIndication> VolumeIndications { get; set; }

    }
}
