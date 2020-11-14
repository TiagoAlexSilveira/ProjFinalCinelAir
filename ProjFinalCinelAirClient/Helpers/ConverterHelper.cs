using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Helpers
{
    public class ConverterHelper : IConverterHelper
    {
        public ChangeUserViewModel ToChangeUserViewModelClient(Client client)
        {

            return new ChangeUserViewModel
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                StreetAddress = client.StreetAddress,
                PhoneNumber = client.PhoneNumber,
                PostalCode = client.PostalCode,
                DateofBirth = client.DateofBirth,
                TaxNumber = client.TaxNumber,
                Email = client.Email,
                Identification = client.Identification,
                Client_Number = client.Client_Number,
                JoinDate = client.JoinDate,
                UserId = client.UserId,
                AnnualMilesBought = client.AnnualMilesBought,
                AnnualMilesConverted = client.AnnualMilesConverted,
                AnnualMilesExtended = client.AnnualMilesExtended,
                AnnualMilesTransfered = client.AnnualMilesTransfered,
                isClientNumberConfirmed = client.isClientNumberConfirmed,
                Miles_Bonus = client.Miles_Bonus,
                Miles_Status = client.Miles_Status,              
                //ImageUrl = client.ImageUrl
            };
        }

        public Client ToClient(ChangeUserViewModel model)
        {
            return new Client
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                StreetAddress = model.StreetAddress,
                PhoneNumber = model.PhoneNumber,
                PostalCode = model.PostalCode,
                DateofBirth = model.DateofBirth,
                TaxNumber = model.TaxNumber,
                Email = model.Email,
                Identification = model.Identification,
                Client_Number = model.Client_Number,
                JoinDate = model.JoinDate,
                UserId = model.UserId,
                AnnualMilesBought = model.AnnualMilesBought,
                AnnualMilesConverted = model.AnnualMilesConverted,
                AnnualMilesExtended = model.AnnualMilesExtended,
                AnnualMilesTransfered = model.AnnualMilesTransfered,
                isClientNumberConfirmed = model.isClientNumberConfirmed,
                Miles_Bonus = model.Miles_Bonus,
                Miles_Status = model.Miles_Status,
                //ImageUrl = client.ImageUrl
            };
        }
    }
}
