﻿using Microsoft.AspNetCore.Identity;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirClient.Helpers;
using System;
using System.Collections.Generic;
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

            #region Criar Roles
            await _userHelper.CheckRoleAsync("Client");
            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("SuperUser");
            await _userHelper.CheckRoleAsync("RegularUser");
            #endregion

            if (!_context.Card.Any())
            {
                this.Add_Card(Convert.ToDateTime("2021-01-05"));
                this.Add_Card(Convert.ToDateTime("2020-10-05"));

                await _context.SaveChangesAsync();
            }



            #region Criar Cidades e País
            // Adicionar as seguintes cidades a Portugal
            if (!_context.Country.Any())
            {
                var cities = new List<City>();
                cities.Add(new City { Name = "Lisboa" });
                cities.Add(new City { Name = "Porto" });
                cities.Add(new City { Name = "Faro" });
                cities.Add(new City { Name = "Coimbra" });

                _context.Country.Add(new Country
                {
                    Cities = cities,
                    Name = "Portugal"
                });

                await _context.SaveChangesAsync();
            }
            #endregion


            var user = await _userHelper.GetUserByEmailAsync("tsilveira01@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    Email = "tsilveira01@gmail.com",
                    UserName = "tsilveira01@gmail.com",
                    CityId = 1,
                    TaxNumber = 354647362,
                    Identification = "63547589",
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
                    CityId = 1,
                    TaxNumber = 226250989,
                    Identification = "11895671",
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



            #region Criar Admin

            var admin = await _userHelper.GetUserByEmailAsync("admincinelair@yopmail.com");
            if (admin == null)
            {
                admin = new User
                {
                    Email = "admincinelair@yopmail.com",
                    UserName = "admincinelair@yopmail.com",
                    CityId = 1,
                    FirstName = "Maria",
                    LastName = "Augusta",
                    TaxNumber = 111111111
                };

                var resultAdmin = await _userHelper.AddUserAsync(admin, "123456");  //cria um user com aqueles dados e aquela password
                if (resultAdmin != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the admin in seeder");
                }

            }

            var IsInRoleAdmim = await _userHelper.IsUserInRoleAsync(admin, "Admin");
            var tokenAdmin = await _userHelper.GenerateEmailConfirmationTokenAsync(admin);
            await _userHelper.ConfirmEmailAsync(admin, tokenAdmin);

            if (!IsInRoleAdmim)
            {
                await _userHelper.AddUserToRoleAsync(admin, "Admin");
            }

            #endregion

           


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

            if (!_context.BuyMilesShop.Any())
            {
                this.Add_BuyMilesShop(2000, 50);
                this.Add_BuyMilesShop(4000, 70);
                this.Add_BuyMilesShop(6000, 90);
                this.Add_BuyMilesShop(8000, 110);
                this.Add_BuyMilesShop(10000, 130);
                this.Add_BuyMilesShop(12000, 150);
                this.Add_BuyMilesShop(14000, 170);
                this.Add_BuyMilesShop(16000, 190);
                this.Add_BuyMilesShop(18000, 210);
                this.Add_BuyMilesShop(20000, 230);
            }


            if (!_context.Historic_Status.Any())
            {
                this.Add_Historic_Status(Convert.ToDateTime("2020-01-05"), null, true, false, 1, 1);
                this.Add_Historic_Status(Convert.ToDateTime("2019-10-05"), null, true, false,  1, 2);

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


        private void Add_Travel_Ticket(int ticket_id, DateTime travel_date, string departure_city, string arrival_city, int clientId, int rateId, int miles_status_id, int miles_bonus_id)
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


        private void Add_Historic_Status(DateTime start, DateTime? end, bool isValidated, bool nominated, int statusId, int clientId)
        {
            _context.Historic_Status.Add(new Historic_Status
            {
                Start_Date = start,
                End_Date = end,
                isValidated = isValidated,
                wasNominated = nominated,
                StatusId = statusId,
                ClientId = clientId
            });
        }

        private void Add_BuyMilesShop(int mileQuantity, decimal price)
        {
            _context.BuyMilesShop.Add(new BuyMilesShop
            {
                MileQuantity = mileQuantity,
                Price = price
            });
        }


        private void Add_Card(DateTime expiryDate)
        {
            _context.Card.Add(new Card
            {
                ExpirationDate = expiryDate
            });
        }


    }

}
