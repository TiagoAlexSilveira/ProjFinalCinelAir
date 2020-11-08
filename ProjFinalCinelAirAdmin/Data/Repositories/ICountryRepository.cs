using Microsoft.AspNetCore.Mvc.Rendering;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Data.Repositories
{
    public interface ICountryRepository : IGenericRepository<Country>
	{
		IQueryable GetCountriesWithCities();


		Task<Country> GetCountryWithCitiesAsync(int id);


		Task<City> GetCityAsync(int id);


		Task AddCityAsync(CityViewModel model);


		Task<int> UpdateCityAsync(City city);


		Task<int> DeleteCityAsync(City city);


		IEnumerable<SelectListItem> GetComboCountries();


		Task<IEnumerable<SelectListItem>> GetComboCities(int countryId);

		Country GetCountryAsync(City city);
    }
}
