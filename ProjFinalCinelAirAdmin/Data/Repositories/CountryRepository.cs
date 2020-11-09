﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Data.Repositories
{
    public class CountryRepository : GenericRepository<Country>, ICountryRepository
    {
        private readonly DataContext _context;

        public CountryRepository(DataContext context) : base(context)
        {
            _context = context;
        }


        public async Task AddCityAsync(CityViewModel model)
        {
            var country = await this.GetCountryWithCitiesAsync(model.CountryId);
            if (country == null)
            {
                return;
            }

            country.Cities.Add(new City { Name = model.Name });
            _context.Country.Update(country);
            await _context.SaveChangesAsync();

        }



        public async Task<int> DeleteCityAsync(City city)
        {
            var country = await _context.Country.Where(c => c.Cities.Any(ci => ci.Id == city.Id)).FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            _context.City.Remove(city);
            await _context.SaveChangesAsync();
            return country.Id;
        }


        public async Task<City> GetCityAsync(int id)
        {
            return await _context.City.FindAsync(id);
        }



        public async Task<IEnumerable<SelectListItem>> GetComboCities(int countryId)
        {
            var country = await GetCountryWithCitiesAsync(countryId);
            var list = new List<SelectListItem>();
            if (country != null)
            {
                list = country.Cities.Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).OrderBy(l => l.Text).ToList();
            }

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a city...)",
                Value = "0"
            });

            return list;

        }


        public IEnumerable<SelectListItem> GetComboCountries()
        {
            var list = _context.Country.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()

            }).OrderBy(l => l.Text).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select a country...)",
                Value = "0"
            });

            return list;

        }


        public IQueryable GetCountriesWithCities()
        {
            return _context.Country
            .Include(c => c.Cities)
            .OrderBy(c => c.Name);

        }


        public Country GetCountryAsync(City city)
        {
            Country country = new Country();

            country = _context.Country.Where(c => c.Cities.Any(ci => ci.Id == city.Id)).ToList().FirstOrDefault();

            return country;
            //return  _context.Countries.Where(c => c.Cities.Any(ci => ci.Id == city.Id));
        }



        public async Task<Country> GetCountryWithCitiesAsync(int id)
        {
            return await _context.Country
             .Include(c => c.Cities)
             .Where(c => c.Id == id)
             .FirstOrDefaultAsync();

        }


        public async Task<int> UpdateCityAsync(City city)
        {
            var country = await _context.Country.Where(c => c.Cities.Any(ci => ci.Id == city.Id)).FirstOrDefaultAsync();
            if (country == null)
            {
                return 0;
            }

            _context.City.Update(city);
            await _context.SaveChangesAsync();
            return country.Id;
        }

   
    }
}
