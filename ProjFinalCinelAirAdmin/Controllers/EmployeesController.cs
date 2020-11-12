using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAdmin.Data.Repositories;
using ProjFinalCinelAirAdmin.Helpers;
using ProjFinalCinelAirAdmin.Models;

namespace ProjFinalCinelAirAdmin.Controllers
{

    public class EmployeesController : Controller
    {
        #region atributos
        private readonly IUserHelper _userHelper;
        private readonly DataContext _context;
        private readonly ICountryRepository _countryRepository;
        private readonly IMailHelper _mailHelper;
        #endregion


        public EmployeesController(IUserHelper userHelper, DataContext context, ICountryRepository countryRepository, IMailHelper mailHelper)
        {
            _userHelper = userHelper;
            _context = context;
            _countryRepository = countryRepository;
            _mailHelper = mailHelper;
        }


        // Lista de Todos os empregados
        public async Task<IActionResult> Index()
        {
            try
            {
                // Seleccionar todos os registos

                List<User> RegularUser = (List<User>)await _userHelper.GetUsersInRoleAsync("RegularUser");
                List<User> superUser = (List<User>)await _userHelper.GetUsersInRoleAsync("SuperUser");
                List<User> admin = (List<User>)await _userHelper.GetUsersInRoleAsync("Admin");

                List<User> employeeList = new List<User>();
                employeeList.AddRange(RegularUser);
                employeeList.AddRange(superUser);
                employeeList.AddRange(admin);

                return View(employeeList);

            }
            catch (Exception)
            {

                return NotFound();
            }
        }


        public async Task<IActionResult> Details(string id) // User ID 
        {
            try
            {
                // Obter o user

                User user = await _userHelper.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                // Obter o role do user
                //var roleName = await _userHelper.;


                City city = await _countryRepository.GetCityAsync(user.CityId);


                string roleName = await _userHelper.GetRoleNameAsync(user);

                EmployeeViewModel employeeModel = new EmployeeViewModel()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.UserName,
                    StreetAddress = user.StreetAddress,
                    PostalCode = user.PostalCode,
                    PhoneNumber = user.PhoneNumber,
                    TaxNumber = user.TaxNumber,
                    Identification = user.Identification,
                    CityName = city.Name,
                    isActive = user.isActive,
                    DateofBirth = user.DateofBirth,
                    JoinDate = user.JoinDate,
                    RoleName = roleName,

                };

                var country = _countryRepository.GetCountryAsync(city);

                if (country != null)
                {
                    employeeModel.CountryId = country.Id;
                    employeeModel.Cities = await _countryRepository.GetComboCities(country.Id);
                    employeeModel.Countries = _countryRepository.GetComboCountries();
                    employeeModel.CityId = user.CityId;
                }

                return View(employeeModel);

            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        public IActionResult Create()
        {
            var model = new RegisterNewEmployeeViewModel
            {
                Countries = _countryRepository.GetComboCountries(),
                Category = GetCategory()
            };

            return View(model);

        }

        private IEnumerable<SelectListItem> GetCategory()
        {

            var list = _context.Roles.Where(r => r.Name != "Client");

            var category = list.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()

            }).OrderBy(l => l.Text).ToList();

            category.Insert(0, new SelectListItem
            {
                Text = "(Select a category...)",
                Value = "0"
            });

            return category;


        }

        public async Task<JsonResult> Xpto(int? countryId)
        {
            if (countryId == 0)
            {
                Country country1 = new Country() { Id = 0, };
                return this.Json(country1);
            }

            var country = await _countryRepository.GetCountryWithCitiesAsync(countryId.Value);
            return this.Json(country.Cities.OrderBy(c => c.Name));
        }






        // Post do Create
        [HttpPost]
        public async Task<IActionResult> Create(RegisterNewEmployeeViewModel model)
        {

            var user = await _userHelper.GetUserByEmailAsync(model.Username);

            if (user == null)
            {
                var city = await _countryRepository.GetCityAsync(model.CityId);

                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    UserName = model.Username,
                    StreetAddress = model.StreetAdress,
                    PostalCode = model.PostalCode,
                    PhoneNumber = model.PhoneNumber,
                    TaxNumber = model.TaxNumber,
                    Identification = model.Identification,
                    CityId = city.Id,
                    isActive = true,
                    DateofBirth = model.DateofBirth,
                    JoinDate = model.JoinDate,
                    Email = model.Username

                };

                try
                {
                    var result = await _userHelper.AddUserAsync(user, "123456");

                }
                catch (Exception)
                {
                    model.Countries = _countryRepository.GetComboCountries();
                    this.ModelState.AddModelError(string.Empty, "The user couldn't be created. Please, confirm data");
                    model.Countries = _countryRepository.GetComboCountries();
                    model.Category = GetCategory();
                    return this.View(model);
                }

                try
                {
                    // Atribuir o role de Employee ao user
                    string name = await _userHelper.GetRoleNameByIdAsync(model.CategoryId);

                    await _userHelper.AddUserToRoleAsync(user, name);
                }
                catch (Exception)
                {
                   
                    model.Countries = _countryRepository.GetComboCountries();
                    model.Category = GetCategory();
                    this.ModelState.AddModelError(string.Empty, "Error adding the employee to the role! Please contact the technical support!");

                    return this.View(model);
                }


                try
                {
                    var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

                    await _userHelper.ConfirmEmailAsync(user, myToken); // Confirmar automaticamente

                }
                catch (Exception)
                {
                  
                    model.Countries = _countryRepository.GetComboCountries();
                  
                    this.ModelState.AddModelError(string.Empty, "Error on the email confirmation! Please, contact the technical suppoprt! ");
                    model.Category = GetCategory();
                    return this.View(model);
                }

                try
                {
                    // Criar um link que vai levar lá dentro uma acção. Quando o utilizador carregar neste link, 
                    // vai no controlador Account executar a action "ChangePassword"
                    // Este ConfirmEmail vai receber um objecto novo que terá um userid e um token.

                    var myTokenReset = await _userHelper.GeneratePasswordResetTokenAsync(user);

                    var link = this.Url.Action(
                        "ResetPassword",
                        "Employees",
                        new { token = myTokenReset }, protocol: HttpContext.Request.Scheme);

                    _mailHelper.SendMail(user.Email, "CinelAir Password Reset", $"<h1>CinelAir Password Reset</h1>" +
                    $"Welcome onboard! Please, reset your password, click in this link:</br></br>" +
                    $"<a href = \"{link}\">Reset Password</a>");

                    ViewBag.Message = "The employee was created with sucess! Was sent an email to the employee for the password reset!";
                    return View();

                }
                catch (Exception)
                {

                    model.Countries = _countryRepository.GetComboCountries();
                    model.Category = GetCategory();
                    this.ModelState.AddModelError(string.Empty, "Error on seeding the email to the employee! Please contact support!");

                    return RedirectToAction(nameof(Index));
                }
            }
            model.Countries = _countryRepository.GetComboCountries();
            model.Category = GetCategory();
            this.ModelState.AddModelError(string.Empty, "The user already exists");
            return View(model);
        }

        public IActionResult ResetPassword(string token) //Token gerado na action Create (Post)
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            var user = await _userHelper.GetUserByEmailAsync(model.UserName);
            if (user != null)
            {
                var result = await _userHelper.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    ViewBag.Message = "Password reset successfully.";
                    return RedirectToAction("Success");
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }


        public IActionResult Success()
        {

            return View();
        }
        
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                // Obter o user
                User user = await _userHelper.GetUserByIdAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                // Obter o role do user
                //var roleName = await _userHelper.;


                City city = await _countryRepository.GetCityAsync(user.CityId);

                var country = _countryRepository.GetCountryAsync(city);

                string roleName = await _userHelper.GetRoleNameAsync(user);

                

                var model = new UpdateEmployeeViewModel
                {
                    Countries = _countryRepository.GetComboCountries(),
                    Category = GetCategory(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    StreetAdress = user.StreetAddress,
                    PostalCode = user.PostalCode,
                    PhoneNumber = user.PhoneNumber,
                    TaxNumber = user.TaxNumber,
                    Identification = user.Identification,
                    CityId = user.CityId,
                    isActive = user.isActive,
                    DateofBirth = user.DateofBirth,
                    JoinDate = user.JoinDate,
                    Email = user.Email,
                   
                };

                var role = model.Category.Where(x => x.Text == roleName).FirstOrDefault();

                model.CategoryId = role.Value;

                if (country != null)
                {
                    model.CountryId = country.Id;
                    model.Cities = await _countryRepository.GetComboCities(country.Id);
                    model.Countries = _countryRepository.GetComboCountries();
                    model.CityId = user.CityId;
                }
                return View(model);
            }
            catch (Exception)
            {

                return NotFound();
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UpdateEmployeeViewModel model)
        {

            if (ModelState.IsValid)
            {
                // Nunca me fio no que vem da View, fazer sempre o check com a base de dados. ( Fazer a pesquisa por nif, número de identificação e email). QQ um destes campos pode ter mudado, ou pode ter havido um engano
                var user = await _userHelper.FindUser(model.Email, model.TaxNumber, model.Identification);

                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Email = model.Email;
                    user.StreetAddress = model.StreetAdress;
                    user.PhoneNumber = model.PhoneNumber;
                    user.TaxNumber = model.TaxNumber;
                    user.Identification = model.Identification;
                    user.CityId = model.CityId;
                    user.isActive = model.isActive;
                    user.PostalCode = model.PostalCode;
                    user.CityId = model.CityId;
                    user.isActive = model.isActive;
                    user.DateofBirth = model.DateofBirth;
                    user.JoinDate = model.JoinDate;

                    var response = await _userHelper.UpdateUserAsync(user); // Actualizar o User

                    var response2 = _userHelper.UpdateUserRole(user.Id, model.CategoryId);

                    // Actualizar os detalhes

                    if (response.Succeeded && response2 == true)
                    {
                        ViewBag.Message = "Employee Updated!";
                        return View();
                    }


                    ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    return View(model);

                }

                else
                {
                    ModelState.AddModelError(string.Empty, "User not found!");

                    return View(model);
                }
            }
            else
            {
                return NotFound();
            }
        }


        
    }

}
