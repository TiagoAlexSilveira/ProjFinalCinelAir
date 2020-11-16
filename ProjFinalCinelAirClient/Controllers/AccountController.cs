using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAirClient.Data.Repositories;
using ProjFinalCinelAirClient.Helpers;
using ProjFinalCinelAirClient.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IConverterHelper _converterHelper;
        private readonly IConfiguration _configuration;
        private readonly IMailHelper _mailHelper;
        private readonly IClientRepository _clientRepository;
        private readonly IHistoric_StatusRepository _historic_StatusRepository;

        public AccountController(IUserHelper userHelper, IConverterHelper converterHelper,
                                IConfiguration configuration, IMailHelper mailHelper,
                                IClientRepository clientRepository, IHistoric_StatusRepository historic_StatusRepository)
        {
            _userHelper = userHelper;
            _converterHelper = converterHelper;
            _configuration = configuration;
            _mailHelper = mailHelper;
            _clientRepository = clientRepository;
            _historic_StatusRepository = historic_StatusRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsyncWithEmail(model);

                
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl")) //se tentar ebntrar numa zona em que não está autorizado, isto redireciona-o para o login e depois quando auntenticado, para a página em que tentou entrar
                    {
                        //Direção de retorno
                        return this.Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
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


        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return this.RedirectToAction("Index", "Home");
        }


       public IActionResult Register()
       {
            var model = new RegisterNewUserViewModel
            {

            };

            return this.View(model);
       }


        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    user = new User
                    {
                        Email = model.Email,
                        UserName = model.Email,
                        CityId = 1,
                        TaxNumber = model.TaxNumber,
                        Identification = model.Identification
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        this.ModelState.AddModelError(string.Empty, "The user could not be created");
                        return View(model);  //retornamos a view outra vez para o user não ter de preencher os campos de novo
                    }

                    var clientList = _clientRepository.GetAll().ToList();
                    
                    var client = new Client
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Email,
                        PhoneNumber = model.PhoneNumber,
                        StreetAddress = model.StreetAddress,
                        PostalCode = model.PostalCode,
                        DateofBirth = model.DateofBirth,
                        TaxNumber = model.TaxNumber,
                        Identification = model.Identification,
                        JoinDate = DateTime.Now,
                        Miles_Bonus = 0,
                        Miles_Status = 0,
                        AnnualMilesBought = 0,
                        AnnualMilesConverted = 0,
                        AnnualMilesExtended = 0,
                        AnnualMilesTransfered = 0,
                        isClientNumberConfirmed = false, 
                        UserId = user.Id
                    };

                    if (clientList.Count == 0)
                    {
                        client.Client_Number = 100001000;
                    }
                    else
                    {
                        var lastClient = clientList.Last();
                        client.Client_Number = lastClient.Client_Number + 1;
                    }

                    await _clientRepository.CreateAsync(client);
                    var selClient = _clientRepository.GetClientByClientNumber(client.Client_Number);

                    var historicStatus = new Historic_Status
                    {
                        ClientId = selClient.Id,
                        Start_Date = (DateTime)client.JoinDate,
                        End_Date = DateTime.Now.AddYears(1),
                        StatusId = 3,
                        wasNominated = false,
                        isValidated = false
                    };

                 
                    await _historic_StatusRepository.CreateAsync(historicStatus);

                    var isRole = await _userHelper.IsUserInRoleAsync(user, "Client");

                    if (!isRole)
                    {
                        await _userHelper.AddUserToRoleAsync(user, "Client");
                    }


                    //var myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                    //var tokenLink = Url.Action("ConfirmEmail", "Account", new
                    //{
                    //    userid = user.Id,
                    //    token = myToken,
                    //}, protocol: HttpContext.Request.Scheme);



                    //try
                    //{
                    //    _mailHelper.SendMail(model.Username, "Email confirmation", $"<h1>Verify your email to finish signing up for AutoWorkshop.</h1>" +
                    //    $"<br><br>Please confirm your email by using this link:</br></br><a href = \"{tokenLink}\">Confirm Email</a>");
                    //    this.ViewBag.Message = "Instructions to confirm your sign up have been sent to your email.";
                    //}
                    //catch (Exception e)
                    //{

                    //    throw e;
                    //}

                    ViewBag.Message = "You will receive an email to complete your registration. This can take up to 24 hours. Thank you for joining us!";

                    return View(model);
                }

                ModelState.AddModelError(string.Empty, "That email is already registered");
            }

            return View(model);
        }

        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return NotFound();
            }

            var user = await _userHelper.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = await _userHelper.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return NotFound();
            }

            return View();
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);

            if (user != null)
            {
                var client = _clientRepository.GetClientByUserId(user.Id);

                var model = _converterHelper.ToChangeUserViewModelClient(client);

                return View(model);
            };

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (ModelState.IsValid)
            {              
                var user = await _userHelper.GetUserByEmailAsync(User.Identity.Name);
                if (user != null)
                {                    
                    var client = _converterHelper.ToClient(model);
                    await _clientRepository.UpdateAsync(client);

                    ViewBag.UserMessage = "Changes have been saved";                                      
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "User not found.");
                }
            }

            return View(model);
        }



        public IActionResult ChangePassword()
        {
            return View();
        }


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
                        return this.RedirectToAction("ChangeUser");
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
                _mailHelper.SendMail(model.Email, "CinelAir Miles Account Password Reset", $"<h1>CinelAir Miles Account Password Reset</h1>" +
                $"To reset your account password click in this link:</br></br>" +
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
