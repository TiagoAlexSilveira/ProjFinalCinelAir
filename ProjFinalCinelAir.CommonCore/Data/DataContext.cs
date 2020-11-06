using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using System.Linq;

namespace ProjFinalCinelAir.CommonCore.Data
{
    public class DataContext : IdentityDbContext<User>
    {

        public DbSet<Client> Client { get; set; }

        public DbSet<Travel_Ticket> Travel_Ticket { get; set; }

        public DbSet<Mile_Bonus> Mile_Bonus { get; set; }

        public DbSet<Mile_Status> Mile_Status { get; set; }

        public DbSet<Rate> Rate { get; set; }

        public DbSet<Status> Status { get; set; }

        public DbSet<Historic_Status> Historic_Status { get; set; }

        public DbSet<Transaction> Transaction  { get; set; }

        public DbSet<Notification> Notification { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            //Cascading Delete Rule
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }


            base.OnModelCreating(modelBuilder);
        }
    }

}
