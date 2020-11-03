using Microsoft.AspNetCore.Identity;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirClient.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random();
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); //ve se a base de dados está criada

            await _userHelper.CheckRoleAsync("Client");


            var user = await _userHelper.GetUserByEmailAsync("tsilveira01@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Tiago",
                    LastName = "Silveira",
                    Email = "tsilveira01@gmail.com",
                    UserName = "tsilveira01@gmail.com",
                    PhoneNumber = "213456789",
                    Client_Number = 100000002,
                    TaxNumber = 354647362,
                    Identification = "63547589",
                    DateofBirth = Convert.ToDateTime("1993-04-29"),
                    StreetAddress = "Rua das Ruas",
                };

                var result = await _userHelper.AddUserAsync(user, "123456");  //cria um user com aqueles dados e aquela password
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

            var IsInRole = await _userHelper.IsUserInRoleAsync(user, "Client");
            var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            await _userHelper.ConfirmEmailAsync(user, token);

            if (!IsInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Client");
            }

            var user2 = await _userHelper.GetUserByEmailAsync("dcruzsimoes@gmail.com");
            if (user2 == null)
            {
                user2 = new User
                {
                    FirstName = "Dulce",
                    LastName = "Simões",
                    Client_Number = 100000001,
                    TaxNumber = 226250989,
                    Identification = "11895671",
                    DateofBirth = Convert.ToDateTime("1981-01-11"),
                    StreetAddress = "Travessa das Flores",
                    Email = "dcruzsimoes@gmail.com",
                    UserName = "dcruzsimoes@gmail.com",
                    PhoneNumber = "213456789"
                };

                var result = await _userHelper.AddUserAsync(user2, "123456");  //cria um user com aqueles dados e aquela password
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
            }

            var IsInRole2 = await _userHelper.IsUserInRoleAsync(user2, "Client");
            var token2 = await _userHelper.GenerateEmailConfirmationTokenAsync(user2);
            await _userHelper.ConfirmEmailAsync(user2, token2);

            if (!IsInRole2)
            {
                await _userHelper.AddUserToRoleAsync(user2, "Client");
            }






            if (!_context.Status.Any())
            {
                this.Add_Status("Basic");
                this.Add_Status("Silver");
                this.Add_Status("Gold");

                await _context.SaveChangesAsync();
            }


            if (!_context.Rate.Any())
            {
                this.Add_Rate("Desconto", 10);
                this.Add_Rate("Basico_Europa", 40);
                this.Add_Rate("Basico_Intercontinentais", 50);
                this.Add_Rate("Classica_Europa", 70);
                this.Add_Rate("Classica_Intercontinentais", 100);
                this.Add_Rate("Plus_Europa", 100);
                this.Add_Rate("Plus_Intercontinentais", 150);
                this.Add_Rate("Executiva", 150);
                this.Add_Rate("TopExecutiva", 200);

                await _context.SaveChangesAsync();
            }


            if (!_context.Status.Any())
            {
                this.Add_Status("Basic");
                this.Add_Status("Silver");
                this.Add_Status("Gold");

                await _context.SaveChangesAsync();
            }


        }



        private void Add_Status(string description)
        {
            _context.Status.Add(new Status
            {
                Description = description
            });
        }


        private void Add_Travel_Ticket(int ticket_id, DateTime travel_date, string departure_city, string arrival_city, string userId, int rateId, int miles_status_id, int miles_bonus_id )
        {
            _context.Travel_Ticket.Add(new Travel_Ticket
            {
                TicketId = ticket_id,
                Travel_Date = travel_date,
                DepartureCity = departure_city,
                ArrivalCity = arrival_city,
                UserId = userId,
                RateId = rateId,
                Miles_BonusId = miles_bonus_id,
                Miles_StatusId = miles_status_id
            });
        }

        private void Add_Mile_Status(decimal miles, DateTime validity, string userId)
        {
            _context.Mile_Status.Add(new Mile_Status
            {
                Miles_Number = miles,
                Validity = validity,
                UserId = userId
            });
        }


        private void Add_Mile_Bonus(decimal miles, DateTime validity, string userId)
        {
            _context.Mile_Bonus.Add(new Mile_Bonus
            {
                Miles_Number = miles,
                Validity = validity,
                UserId = userId
            });
        }


        private void Add_Rate(string description, decimal percentage)
        {
            _context.Rate.Add(new Rate
            {
                Description = description,
                Percentage = percentage
            });
        }


        private void Add_Historic_Status(DateTime start, DateTime end, bool isValidated, int statusId)
        {
            _context.Historic_Status.Add(new Historic_Status
            {
                Start_Date = start,
                End_Date = end, 
                isValidated = isValidated,
                StatusId = statusId
            });
        }



    }

}
