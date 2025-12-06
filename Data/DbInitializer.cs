using Microsoft.EntityFrameworkCore;
using Assignment1.Models;

namespace Assignment1.Data
{
    public static class DbInitializer
    {
        public static void Initialize(Assignment1DbContext context)
        {
            try
            {
                // Apply migrations (recommended)
               // context.Database.Migrate();

                // Seed VetDoctors
                if (!context.VetDoctors.Any())
                {
                    var vetDoctors = new[]
                    {
                        new VetDoctor { Name = "Dr. Smith", Specialty = "General Care" },
                        new VetDoctor { Name = "Dr. Jones", Specialty = "Surgery" },
                        new VetDoctor { Name = "Dr. Brown", Specialty = "Dentistry" }
                    };
                    context.VetDoctors.AddRange(vetDoctors);
                    context.SaveChanges();
                }

                // Seed Pets
                if (!context.Pets.Any())
                {
                    var docs = context.VetDoctors.OrderBy(d => d.Id).ToList();
                    if (docs.Count >= 3)
                    {
                        var pets = new[]
                        {
                            new Pet { Name = "Max",  MicrochipId = "123456789", Species = "Dog",   VetDoctorId = docs[0].Id },
                            new Pet { Name = "Luna", MicrochipId = "987654321", Species = "Cat",   VetDoctorId = docs[0].Id },
                            new Pet { Name = "Buddy",MicrochipId = "456789123", Species = "Dog",   VetDoctorId = docs[1].Id },
                            new Pet { Name = "Milo", MicrochipId = "321654987", Species = "Rabbit",VetDoctorId = docs[2].Id }
                        };
                        context.Pets.AddRange(pets);
                        context.SaveChanges();
                    }
                }

                // Seed PetProfiles (one per pet)
                if (!context.PetProfiles.Any())
                {
                    var pets = context.Pets.OrderBy(p => p.Id).ToList();
                    if (pets.Count >= 4)
                    {
                        var profiles = new[]
                        {
                            new PetProfile { PetId = pets[0].Id, VetNotes = "Healthy, needs annual checkup." },
                            new PetProfile { PetId = pets[1].Id, VetNotes = "Recent vaccination completed." },
                            new PetProfile { PetId = pets[2].Id, VetNotes = "Minor dental cleaning required." },
                            new PetProfile { PetId = pets[3].Id, VetNotes = "Sensitive digestion, monitor diet." }
                        };
                        context.PetProfiles.AddRange(profiles);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization failed: {ex}");
                throw;
            }
        }
    }
}
