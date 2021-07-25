using com.allcard.institution.models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace com.allcard.institution.repository
{


    public class InstitutionContext : DbContext
    {
        public InstitutionContext(DbContextOptions<InstitutionContext> options) : base(options)
        {
        }

        public DbSet<Institution> Institution { get; set; }
        public DbSet<Chain> Chain { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Merchant> Merchant { get; set; }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Location> Location { get; set; }

        public DbSet<Member> Member { get; set; }
        public DbSet<BranchSchedule> BranchSchedule { get; set; }
        public DbSet<BranchScheduleMember> BranchScheduleMember { get; set; }
        public DbSet<UsersProfile> UsersProfile { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        //public DbSet<RefGroupOfIsland> RefGroupOfIsland { get; set; }
        //public DbSet<RefRegion> RefRegion { get; set; }
        //public DbSet<RefProvince> RefProvince { get; set; }
        public DbSet<AuditLog> AuditLog { get; set; }
        public DbSet<RefCityMunicipality> RefCityMunicipality { get; set; }
        public DbSet<RefProvince> RefProvince { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Institution>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<Chain>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<Group>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<Merchant>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<Branch>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<Location>().Property(p => p.Version).IsRowVersion();

            modelBuilder.Entity<Location>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<BranchSchedule>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<BranchScheduleMember>().Property(p => p.Version).IsRowVersion();

            modelBuilder.Entity<UsersProfile>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<Role>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<UserRole>().Property(p => p.Version).IsRowVersion();

            //modelBuilder.Entity<RefGroupOfIsland>().Property(p => p.Version).IsRowVersion();
            //modelBuilder.Entity<RefRegion>().Property(p => p.Version).IsRowVersion();
            //modelBuilder.Entity<RefProvince>().Property(p => p.Version).IsRowVersion();

            modelBuilder.Entity<AuditLog>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<RefCityMunicipality>().Property(p => p.Version).IsRowVersion();
            modelBuilder.Entity<RefProvince>().Property(p => p.Version).IsRowVersion();

        }

        #region Overrides
        public override int SaveChanges()
        {
            var AddedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();
            var currentDate = DateTime.Now;
            AddedEntities.ForEach(E =>
            {
                E.Property("GUID").CurrentValue = Guid.NewGuid();
                E.Property("CreatedDate").CurrentValue = currentDate;
                E.Property("UpdatedDate").CurrentValue = currentDate;
            });

            var EditedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

            EditedEntities.ForEach(E =>
            {
                E.Property("UpdatedDate").CurrentValue = currentDate;
            });

            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            var AddedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();
            var currentDate = DateTime.Now;
            AddedEntities.ForEach(E =>
            {
                E.Property("GUID").CurrentValue = Guid.NewGuid();
                E.Property("CreatedDate").CurrentValue = currentDate;
                E.Property("UpdatedDate").CurrentValue = currentDate;
            });

            var EditedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

            EditedEntities.ForEach(E =>
            {
                E.Property("UpdatedDate").CurrentValue = currentDate;
            });

            return base.SaveChangesAsync(cancellationToken);
        }
        #endregion
    }
}

