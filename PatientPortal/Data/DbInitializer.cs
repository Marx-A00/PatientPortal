using Microsoft.EntityFrameworkCore;
using PatientPortal.Models;

namespace PatientPortal.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Apply any pending migrations (better than EnsureCreated)
            context.Database.Migrate();

            // Seed patients if they don't exist
            if (!context.Patients.Any())
            {
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

            // Seed payments if they don't exist (separate check!)
            if (!context.Payments.Any())
            {
                var existingPatients = context.Patients.ToList();
                if (existingPatients.Count >= 10) // Ensure we have enough patients
                {
                    var payments = new Payment[]
                    {
                        new Payment { CheckNumber = "CHK1001", Amount = 100.00m, Status = "Ready for Release", PatientId = existingPatients[0].Id },
                        new Payment { CheckNumber = "CHK1002", Amount = 150.50m, Status = "Released", PatientId = existingPatients[1].Id },
                        new Payment { CheckNumber = "CHK1003", Amount = 200.75m, Status = "Determining Path", PatientId = existingPatients[2].Id },
                        new Payment { CheckNumber = "CHK1004", Amount = 250.00m, Status = "Out for Payment", PatientId = existingPatients[3].Id },
                        new Payment { CheckNumber = "CHK1005", Amount = 300.25m, Status = "Shipped", PatientId = existingPatients[4].Id },
                        new Payment { CheckNumber = "CHK1006", Amount = 75.10m, Status = "Cashed", PatientId = existingPatients[5].Id },
                        new Payment { CheckNumber = "CHK1007", Amount = 500.00m, Status = "Escheatment", PatientId = existingPatients[6].Id },
                        new Payment { CheckNumber = "CHK1008", Amount = 425.75m, Status = "Released", PatientId = existingPatients[7].Id },
                        new Payment { CheckNumber = "CHK1009", Amount = 80.00m, Status = "Out for Payment", PatientId = existingPatients[8].Id },
                        new Payment { CheckNumber = "CHK1010", Amount = 999.99m, Status = "Ready for Release", PatientId = existingPatients[9].Id }
                    };

                    context.Payments.AddRange(payments);
                    context.SaveChanges();
                }
            }
        }
    }
} 