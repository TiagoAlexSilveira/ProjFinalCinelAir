using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirAdmin.Data.Repositories;
using ProjFinalCinelAirAdmin.Helpers;
using ProjFinalCinelAirAdmin.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjFinalCinelAirAdmin.Controllers
{
   

    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;
        private readonly ICountryRepository _countryRepository;
        private readonly DataContext _context;

        public AccountController(IUserHelper userHelper, IConfiguration configuration, IMailHelper mailHelper, ICountryRepository countryRepository, DataContext context)
        {
            _userHelper = userHelper;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _countryRepository = countryRepository;
            _context = context;
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsyncWithEmail(model);
                var user = await _userHelper.GetUserByEmailAsync(model.Username);

                //Obter o role do utilizador 
                var roleUser = _context.UserRoles.Where(x => x.UserId == user.Id).FirstOrDefault();
                var role = _context.Roles.Where(x => x.Id == roleUser.RoleId).FirstOrDefault();

                if (result.Succeeded && (role.Name == "Admin" || role.Name == "SuperUser" || role.Name == "RegularUser"))
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl")) //se tentar entrar numa zona em que não está autorizado, isto redireciona-o para o login e depois quando auntenticado, para a página em que tentou entrar
                    {
                        //Direção de retorno
                        return this.Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }

                this.ModelState.AddModelError(string.Empty, "Access Only for employees");

            }

            this.ModelState.AddModelError(string.Empty, "Failed to login.");
            return this.View(model);
        }



        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View();
        }



        [Authorize(Roles = "Admin, SuperUser, RegularUser")]
        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return this.RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin, SuperUser, RegularUser")]
        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Address = user.StreetAddress;
                model.PhoneNumber = user.PhoneNumber;
                model.TaxNumber = user.TaxNumber;
                model.Username = user.UserName;
                model.Identification = user.Identification;
                model.PostalCode = user.PostalCode;
                model.DateOfBirth = Convert.ToDateTime(user.DateofBirth);


                var city = await _countryRepository.GetCityAsync(user.CityId);
                if (city != null)
                {
                    var country = _countryRepository.GetCountryAsync(city);
                    if (country != null)
                    {
                        model.CountryId = country.Id;
                        model.Cities = await _countryRepository.GetComboCities(country.Id);
                        model.Countries = _countryRepository.GetComboCountries();
                        model.CityId = user.CityId;
                    }
                }
            }

            model.Cities = await _countryRepository.GetComboCities(model.CountryId);
            model.Countries = _countryRepository.GetComboCountries();
            return this.View(model);
        }


        [Authorize(Roles = "Admin, SuperUser, RegularUser")]
        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.StreetAddress = model.Address;
                    user.PhoneNumber = model.PhoneNumber;


                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.StreetAddress = model.Address;
                    user.PhoneNumber = model.PhoneNumber;
                    user.TaxNumber = model.TaxNumber;
                    user.UserName = model.Username;
                    user.Identification = model.Identification;
                    user.PostalCode = model.PostalCode;
                    user.DateofBirth = Convert.ToDateTime(model.DateOfBirth);
                    user.CityId = model.CityId;

                    var respose = await _userHelper.UpdateUserAsync(user);

                    if (respose.Succeeded)
                    {
                        this.ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, respose.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return this.View(model);
        }




        public async Task GetCombos(RegisterNewUserViewModel model)
        {
            model.Cities = await _countryRepository.GetComboCities(0);
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


        [Authorize(Roles = "Admin, SuperUser, RegularUser")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize(Roles = "Admin, SuperUser, RegularUser")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var response = await _userHelper.CHangePasswordAsync(user, model.OldPassword, model.NewPassword);

                    if (response.Succeeded)
                    {
                        this.ViewBag.Message = "Password Updated!";
                        return View(model);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, response.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "user not found");
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin, SuperUser, RegularUser")]
        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await _userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                        };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            _configuration["Tokens:Issuer"],
                            _configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);
                    }
                }
            }

            return this.BadRequest();
        }


        public IActionResult RecoverPassword()
        {
            return View();
        }


        
        public IActionResult ResetPassword(string token)
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
                    this.ViewBag.Message = "Password reset successful.";
                    return this.View();
                }

                this.ViewBag.Message = "Error while resetting the password.";
                return View(model);
            }

            this.ViewBag.Message = "User not found.";
            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> RecoverPassword(RecoverPasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email); //verifica se user existe
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "The email doesn't correspont to a registered user.");
                    return this.View(model);
                }

                var myToken = await _userHelper.GeneratePasswordResetTokenAsync(user); //gera token

                var link = this.Url.Action(
                    "ResetPassword",
                    "Account",
                    new { token = myToken }, protocol: HttpContext.Request.Scheme);

                //mandar token através de email
                _mailHelper.SendMail(model.Email, "CinelMiles Password Reset", $"<h1>CinelMiles Password Reset</h1>" +
                $"To reset the password click in this link:</br></br>" +
                $"<a href = \"{link}\">Reset Password</a>");
                this.ViewBag.Message = "The instructions to recover your password has been sent to email.";
                return this.View();

            }

            return this.View(model);
        }


        public IActionResult NotAuthorized()
        {
            return View();
        }

    }
}
