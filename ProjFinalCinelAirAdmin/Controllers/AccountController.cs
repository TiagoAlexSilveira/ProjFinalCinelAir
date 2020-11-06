﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAdmin.Data.Repositories;
using ProjFinalCinelAirAdmin.Helpers;
using ProjFinalCinelAirAdmin.Models;

namespace ProjFinalCinelAirAdmin.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;
        private readonly ICountryRepository _countryRepository;
  

        public AccountController(IUserHelper userHelper, IConfiguration configuration, IMailHelper mailHelper, ICountryRepository countryRepository)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _countryRepository = countryRepository;
        
        }

        public IActionResult Login(string returnUrl)
        {

            ViewBag.ReturnUrl = returnUrl;

            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View();

        }

        public async Task<IActionResult> LogOut()
        {
            await _userHelper.LogoutAsync();

            return this.RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {

            ViewBag.ReturnUrl = returnUrl;


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userHelper.LoginAsyncWithEmail(model);

            if (Url.IsLocalUrl(ViewBag.ReturnUrl))
                return Redirect(ViewBag.ReturnUrl);


            return RedirectToAction("Index", "Home");
        }

        /*
        public void GetCombos(RegisterNewUserViewModel model)
        {
            model.Cities = _countryRepository.GetComboCities(0);
            model.Countries = _countryRepository.GetComboCountries();

        }

        public async Task<JsonResult> GetCitiesAsync(int? countryId)
        {
            if (countryId == 0)
            {
                Country country1 = new Country() { Id = 0, };
                return this.Json(country1);
            }

            var country = await _countryRepository.GetCountryWithCitiesAsync(countryId.Value);
            return this.Json(country.Cities.OrderBy(c => c.Name));

        }

        public IActionResult Register()
        {
            RegisterNewUserViewModel model = new RegisterNewUserViewModel();
            GetCombos(model);
            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {


            var user = await _userHelper.GetUserByEmailAsync(model.Username);

            if (user == null)
            {
                City city = await _countryRepository.GetCityAsync(model.CityId);

                user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Username,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Username,
                    SocialSecurityNumber = model.SocialSecurityNumber,
                    TaxNumber = model.TaxNumber,
                    Address = model.Address,
                    City = city,
                    isActive = true,
                };


                var result = await _userHelper.AddUserAsync(user, model.Password);


                if (result != IdentityResult.Success)
                {
                    this.ModelState.AddModelError(string.Empty, "The user couldn't be created. Please, contact support!");
                    return this.View(model);
                }

                var result2 = await _userHelper.AddUserToRoleAsync(user, "Customer");

                var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);

                // Criar um link que vai levar lá dentro uma acção. Quando o utilizador carregar neste link, 
                // vai no controlador Account executar a action "ConfirmEmail"(Ainda será feita)
                // Este ConfirmEmail vai receber um objecto novo que terá um userid e um token.
                var tokenLink = this.Url.Action("ConfirmEmail", "Account", new
                {
                    userid = user.Id,
                    token = myToken,

                }, protocol: HttpContext.Request.Scheme);

                _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Email Confirmation</h1>" +
                   $"To allow the user, " +
                   $"plase click in this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");

                this.ViewBag.Message = "Go to your email to activate your account";


                return this.View(model);
            }

            this.ModelState.AddModelError(string.Empty, "The user already exists");


            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            // Se o user nao estiver na base de dados
            var user = await _userHelper.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            // Confirmar o Email
            var result = await _userHelper.ConfirmEmailAsync(user, token);

            // Caso corra alguma coisa mal
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }




        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Address = user.Address;
                model.PhoneNumber = user.PhoneNumber;
                model.TaxNumber = user.TaxNumber;
                model.Username = user.UserName;
                model.SocialSecurityNumber = user.SocialSecurityNumber;


                var city = await _countryRepository.GetCityAsync(user.CityId);
                if (city != null)
                {
                    var country = _countryRepository.GetCountryAsync(city);
                    if (country != null)
                    {
                        model.CountryId = country.Id;
                        model.Cities = _countryRepository.GetComboCities(country.Id);
                        model.Countries = _countryRepository.GetComboCountries();
                        model.CityId = user.CityId;
                    }
                }
            }

            model.Cities = _countryRepository.GetComboCities(model.CountryId);
            model.Countries = _countryRepository.GetComboCountries();
            return this.View(model);
        }



        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var city = await _countryRepository.GetCityAsync(model.CityId);

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;
                    user.TaxNumber = model.TaxNumber;
                    user.SocialSecurityNumber = model.SocialSecurityNumber;
                    user.CityId = model.CityId;
                    user.City = city;
                    user.UserName = model.Username;

                    var result = await _userHelper.UpdateUserAsync(user);

                    if (result.Succeeded)
                    {
                        ViewBag.Message = "User updated!";
                        model.Cities = _countryRepository.GetComboCities(model.CountryId);
                        model.Countries = _countryRepository.GetComboCountries();
                        return this.View(model);
                    }
                    else
                    {

                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                        model.Cities = _countryRepository.GetComboCities(model.CountryId);
                        model.Countries = _countryRepository.GetComboCountries();
                        return this.View(model);

                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User no found.");
                    model.Cities = _countryRepository.GetComboCities(model.CountryId);
                    model.Countries = _countryRepository.GetComboCountries();
                    return this.View(model);
                }
            }

            model.Cities = _countryRepository.GetComboCities(model.CountryId);
            model.Countries = _countryRepository.GetComboCountries();
            return this.View(model);
        }

        public async Task<IActionResult> ChangePasswordEmployee(string userId, string token)
        {

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            // Se o user nao estiver na base de dados
            var user = await _userHelper.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            // Confirmar o Email
            var result = await _userHelper.ConfirmEmailAsync(user, token);

            // Caso corra alguma coisa mal
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return this.RedirectToAction("ChangePassword", "Account");
        }

        public IActionResult ChangePassword()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

                if (user != null)
                {
                    var result = await _userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        ViewBag.Message = "Password updated";
                        return View();
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description); //Mostrar o primeiro erro que aparece
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return View(model);
        }




        [HttpPost] //Método para proteger a API
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid) // Ver se o modelo é válido
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null) // Ver se o user existe
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded) // Ver se a password está correcta
                    {
                        var claims = new[] // Claims (perfil de utilizador)
                        {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                        //Algoritmo de encriptação do Token(SymetricSecurityKey)
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"])); // É aqui que vai aceder ao JSON app, através do configuration
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15), //Quando é que o token expira (os bancos expiram muito rapidamente)
                            signingCredentials: credentials);

                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created(string.Empty, results); //Não retorna uma view, retorna um token
                        // 
                    }
                }
            }

            return this.BadRequest();
        }

        public IActionResult RecoverPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return this.View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                _mailHelper.SendMail(model.Email, "Airline Password Reset", $"<h1>Airline Password Reset</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");
                this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                return this.View();

            }

            return this.View(model);
        }


        public IActionResult ResetPassword(string token) //Token gerado na action RecoverPassword (Post)
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

        public IActionResult NotAutorized()
        {

            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> MyFlights()
        {
            string email = this.User.Identity.Name;

            List<Ticket> MyList = new List<Ticket>();

            MyList = _ticketRepository.FlightTicketsByUser(email);

            foreach (var item in MyList)
            {
                Destination cityFrom = await _destinationRepository.GetDestinationWithUserCityAndCoutryAsync(item.Flight.From.Id);
                Destination cityTo = await _destinationRepository.GetDestinationWithUserCityAndCoutryAsync(item.Flight.To.Id);
                item.Flight.From.City.Name = cityFrom.City.Name;
                item.Flight.To.City.Name = cityTo.City.Name;
            }


            return View(MyList);
        }
    }*/
}
}
