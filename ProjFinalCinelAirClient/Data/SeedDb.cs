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
                    Email = "tsilveira01@gmail.com",
                    UserName = "tsilveira01@gmail.com",
                };

                var client = new Client
                {
                    FirstName = "Tiago",
                    LastName = "Silveira",
                    PhoneNumber = "213456789",
                    Client_Number = 100000002,
                    TaxNumber = 354647362,
                    Identification = "63547589",
                    DateofBirth = Convert.ToDateTime("1993-04-29"),
                    JoinDate = Convert.ToDateTime("2020-01-05"),
                    UserId = user.Id,
                    StreetAddress = "Rua das Ruas",
                };


                var result = await _userHelper.AddUserAsync(user, "123456");  //cria um user com aqueles dados e aquela password
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder!");
                }

                _context.Client.Add(client);
            }

            var IsInRole = await _userHelper.IsUserInRoleAsync(user, "Client");
            var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            await _userHelper.ConfirmEmailAsync(user, token);

            if (!IsInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Client");
            }


            //////////////////////////////////////////////

            var user2 = await _userHelper.GetUserByEmailAsync("dcruzsimoes@gmail.com");
            if (user2 == null)
            {
                user2 = new User
                {
                    Email = "dcruzsimoes@gmail.com",
                    UserName = "dcruzsimoes@gmail.com",
                };

                var client2 = new Client
                {
                    FirstName = "Dulce",
                    LastName = "Simões",
                    PhoneNumber = "213456789",
                    Client_Number = 100000001,
                    TaxNumber = 226250989,
                    Identification = "11895671",
                    DateofBirth = Convert.ToDateTime("1981-01-11"),
                    JoinDate = Convert.ToDateTime("2019-10-05"),
                    UserId = user2.Id,
                    StreetAddress = "Travessa das Flores",
                };


                var result2 = await _userHelper.AddUserAsync(user2, "123456");  //cria um user com aqueles dados e aquela password
                if (result2 != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }

                _context.Client.Add(client2);
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

            if (!_context.Historic_Status.Any())
            {
                this.Add_Historic_Status(Convert.ToDateTime("2020-01-05"), null, true, 1, 1);
                this.Add_Historic_Status(Convert.ToDateTime("2019-10-05"), null, true, 1, 2);

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


        private void Add_Travel_Ticket(int ticket_id, DateTime travel_date, string departure_city, string arrival_city, int clientId, int rateId, int miles_status_id, int miles_bonus_id )
        {
            _context.Travel_Ticket.Add(new Travel_Ticket
            {
                TicketId = ticket_id,
                Travel_Date = travel_date,
                DepartureCity = departure_city,
                ArrivalCity = arrival_city,
                ClientId = clientId,
                RateId = rateId,
                Miles_BonusId = miles_bonus_id,
                Miles_StatusId = miles_status_id
            });
        }

        private void Add_Mile_Status(int miles, DateTime validity, int clientId)
        {
            _context.Mile_Status.Add(new Mile_Status
            {
                Miles_Number = miles,
                Validity = validity,
                ClientId = clientId
            });
        }


        private void Add_Mile_Bonus(int miles, DateTime validity, int clientId)
        {
            _context.Mile_Bonus.Add(new Mile_Bonus
            {
                Miles_Number = miles,
                Validity = validity,
                ClientId = clientId
            });
        }


        private void Add_Rate(string description, int percentage)
        {
            _context.Rate.Add(new Rate
            {
                Description = description,
                Percentage = percentage
            });
        }


        private void Add_Historic_Status(DateTime start, DateTime? end, bool isValidated, int statusId, int clientId)
        {
            _context.Historic_Status.Add(new Historic_Status
            {
                Start_Date = start,
                End_Date = end, 
                isValidated = isValidated,
                StatusId = statusId,
                ClientId = clientId
            });
        }



    }

}
