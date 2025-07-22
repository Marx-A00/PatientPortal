using Microsoft.EntityFrameworkCore;
using PatientPortal.Models;

namespace PatientPortal.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if data already exists
            if (context.Patients.Any())
            {
                return; // Database has been seeded
            }

            var patients = new Patient[]
            {
                new Patient
                {
                    Name = "Salvador Dali",
                    DOB = new DateTime(1904, 5, 11),
                    Email = "melting.clocks@surreal.com"
                },
                new Patient
                {
                    Name = "Frida Kahlo",
                    DOB = new DateTime(1907, 7, 6),
                    Email = "self.portraits@mexico.art"
                },
                new Patient
                {
                    Name = "Vincent van Gogh",
                    DOB = new DateTime(1853, 3, 30),
                    Email = "starry.night@postimpressionist.com"
                },
                new Patient
                {
                    Name = "Benjamin Reichwald",
                    DOB = new DateTime(1994, 9, 4),
                    Email = "bladee.city@bladeeRadio.2real"
                },
                new Patient
                {
                    Name = "Andy Warhol",
                    DOB = new DateTime(1928, 8, 6),
                    Email = "campbell.soup@popart.com"
                },
                new Patient
                {
                    Name = "Marina Abramović",
                    DOB = new DateTime(1946, 11, 30),
                    Email = "rhythm.zero@performance.art"
                },
                new Patient
                {
                    Name = "Jean-Michel Basquiat",
                    DOB = new DateTime(1960, 12, 22),
                    Email = "neo.expressionist@nyc.com"
                },
                new Patient
                {
                    Name = "Tracey Emin",
                    DOB = new DateTime(1963, 7, 3),
                    Email = "unmade.bed@yba.com"
                },
                new Patient
                {
                    Name = "Damien Hirst",
                    DOB = new DateTime(1965, 6, 7),
                    Email = "shark.tank@yba.com"
                },
                new Patient
                {
                    Name = "Banksy",
                    DOB = new DateTime(1974, 7, 28),
                    Email = "anonymous@street.art"
                },
                new Patient
                {
                    Name = "Yayoi Kusama",
                    DOB = new DateTime(1929, 3, 22),
                    Email = "infinity.dots@polka.com"
                },
                new Patient
                {
                    Name = "Ai Weiwei",
                    DOB = new DateTime(1957, 8, 28),
                    Email = "sunflower.seeds@contemporary.com"
                },
                new Patient
                {
                    Name = "Cindy Sherman",
                    DOB = new DateTime(1954, 1, 19),
                    Email = "untitled.film@stills.com"
                },
                new Patient
                {
                    Name = "Jeff Koons",
                    DOB = new DateTime(1955, 1, 21),
                    Email = "balloon.dog@kitsch.com"
                },
                new Patient
                {
                    Name = "Kara Walker",
                    DOB = new DateTime(1969, 11, 26),
                    Email = "silhouettes@history.com"
                },
                new Patient
                {
                    Name = "Maurizio Cattelan",
                    DOB = new DateTime(1960, 9, 21),
                    Email = "banana.tape@contemporary.com"
                },
                new Patient
                {
                    Name = "Olafur Eliasson",
                    DOB = new DateTime(1967, 2, 5),
                    Email = "weather.project@tate.org"
                },
                new Patient
                {
                    Name = "Anish Kapoor",
                    DOB = new DateTime(1954, 3, 12),
                    Email = "void@sculpture.com"
                },
                new Patient
                {
                    Name = "Jenny Holzer",
                    DOB = new DateTime(1950, 7, 29),
                    Email = "truisms@led.com"
                },
                new Patient
                {
                    Name = "Chris Ofili",
                    DOB = new DateTime(1968, 10, 10),
                    Email = "elephant.dung@painting.com"
                }
            };

            context.Patients.AddRange(patients);
            context.SaveChanges();
        }
    }
} 