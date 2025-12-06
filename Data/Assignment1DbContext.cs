using Microsoft.EntityFrameworkCore;
using Assignment1.Models;

namespace Assignment1.Data
{
    public class Assignment1DbContext : DbContext
    {
        public Assignment1DbContext(DbContextOptions<Assignment1DbContext> options) : base(options) { }

        public DbSet<VetDoctor> VetDoctors { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetProfile> PetProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Use singular table names (Lab 5 style)
            modelBuilder.Entity<VetDoctor>().ToTable("VetDoctor");
            modelBuilder.Entity<Pet>().ToTable("Pet");
            modelBuilder.Entity<PetProfile>().ToTable("PetProfile");

            // VetDoctor 1-* Pet
            modelBuilder.Entity<Pet>()
                .HasOne(p => p.VetDoctor)
                .WithMany(d => d.Pets)
                .HasForeignKey(p => p.VetDoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Pet 1-1 PetProfile (unique PetId)
            modelBuilder.Entity<PetProfile>()
                .HasOne(pp => pp.Pet)
                .WithOne(p => p.PetProfile)
                .HasForeignKey<PetProfile>(pp => pp.PetId);

            modelBuilder.Entity<PetProfile>()
                .HasIndex(pp => pp.PetId)
                .IsUnique();
        }
    }
}
