using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAir.CommonCore.Models;
using ProjFinalCinelAirClient.ApiData;
using ProjFinalCinelAirClient.Helpers;
using ProjFinalCinelAirClient.Request;
using ProjFinalCinelAirClient.Responses;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjFinalCinelAirClient.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserHelper _userHelper;
        private readonly IMailHelper _mailHelper;
        private readonly DataContext _context;
        private IConfiguration _configuration;

        public AccountController(IUserHelper userHelper,  IMailHelper mailHelper, DataContext context, IConfiguration configuration)
        {
            _userHelper = userHelper;                    
            _mailHelper = mailHelper;
            _context = context;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            List<User> users = await _context.Users.ToListAsync();
            return Ok(users);
        }


        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUser(string email)
        {
            User user = await _userHelper.GetUserByEmailAsync(email);
           

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }




        /*
        [HttpPost]
        [Route("CreateToken")]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result = await _userHelper.ValidatePasswordAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        object results = GetToken(user);
                        return Created(string.Empty, results);
                    }
                }

            }

            return BadRequest();
        }


        private object GetToken(User user)
        {
            Claim[] claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new JwtSecurityToken(
                _configuration["Tokens:Issuer"],
                _configuration["Tokens:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(99),
                signingCredentials: credentials);

            return new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                user
            };
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            User user = await _userHelper.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound("Error001");
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> PostUser([FromBody] UserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request",
                    Result = ModelState
                });
            }

            User user = await _userHelper.GetUserAsync(request.Email);
            if (user != null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Error003"
                });
            }

          
            user = new User
            {
                Address = request.Address,
                Document = request.Document,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.Phone,
                UserName = request.Email,
                ImageId = imageId,
                UserType = UserType.User,
                City = city,
                Latitude = request.Latitude,
                Logitude = request.Logitude
            };

            IdentityResult result = await _userHelper.AddUserAsync(user, request.Password);
            if (result != IdentityResult.Success)
            {
                return BadRequest(result.Errors.FirstOrDefault().Description);
            }

            User userNew = await _userHelper.GetUserAsync(request.Email);
            await _userHelper.AddUserToRoleAsync(userNew, user.UserType.ToString());

            string myToken = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
            string tokenLink = Url.Action("ConfirmEmail", "Account", new
            {
                userid = user.Id,
                token = myToken
            }, protocol: HttpContext.Request.Scheme);

            _mailHelper.SendMail(request.Email, "Email Confirmation", $"<h1>Email Confirmation</h1>" +
                $"To confirm your email please click on the link<p><a href = \"{tokenLink}\">Confirm Email</a></p>");

            return Ok(new Response { IsSuccess = true });
        }
        /*
        [HttpPost]
        [Route("RecoverPassword")]
        public async Task<IActionResult> RecoverPassword([FromBody] EmailRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Bad request"
                });
            }

            User user = await _userHelper.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new Response
                {
                    IsSuccess = false,
                    Message = "Error001"
                });
            }

            string myToken = await _userHelper.GeneratePasswordResetTokenAsync(user);
            string link = Url.Action("ResetPassword", "Account", new { token = myToken }, protocol: HttpContext.Request.Scheme);
            _mailHelper.SendMail(request.Email, "Password Recover", $"<h1>Password Recover</h1>" +
                $"Click on the following link to change your password:<p>" +
                $"<a href = \"{link}\">Change Password</a></p>");

            return Ok(new Response { IsSuccess = true });
        }*/
        /*
                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                [HttpPut]
                public async Task<IActionResult> PutUser([FromBody] UserRequest request)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    User user = await _userHelper.GetUserAsync(email);
                    if (user == null)
                    {
                        return NotFound("Error001");
                    }



                    user.FirstName = request.FirstName;
                    user.LastName = request.LastName;
                    user.Address = request.Address;
                    user.PhoneNumber = request.Phone;
                    //TODO: Fix the document on the videos
                    user.Document = request.Document;
                    user.City = city;
                    user.ImageId = imageId;
                    user.Latitude = request.Latitude;
                    user.Logitude = request.Logitude;

                    IdentityResult respose = await _userHelper.UpdateUserAsync(user);
                    if (!respose.Succeeded)
                    {
                        return BadRequest(respose.Errors.FirstOrDefault().Description);
                    }

                    User updatedUser = await _userHelper.GetUserAsync(email);
                    return Ok(updatedUser);
                }


                [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
                [HttpPost]
                [Route("ChangePassword")]
                public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(new Response
                        {
                            IsSuccess = false,
                            Message = "Bad request",
                            Result = ModelState
                        });
                    }

                    string email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
                    User user = await _userHelper.GetUserAsync(email);
                    if (user == null)
                    {
                        return NotFound("Error001");
                    }

                    IdentityResult result = await _userHelper.CHangePasswordAsync(user, request.OldPassword, request.NewPassword);
                    if (!result.Succeeded)
                    {
                        return BadRequest(new Response
                        {
                            IsSuccess = false,
                            Message = "Error005"
                        });
                    }

                    return Ok(new Response { IsSuccess = true });
                }*/
    }
    
}
