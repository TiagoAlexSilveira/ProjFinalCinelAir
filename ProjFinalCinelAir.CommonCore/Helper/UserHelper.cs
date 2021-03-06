﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjFinalCinelAir.CommonCore.Data;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAir.CommonCore.Helper;
using ProjFinalCinelAir.CommonCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjFinalCinelAir.CommonCore.Helper
{
    public class UserHelper : IUserHelper
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DataContext _context;

        public UserHelper(UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            DataContext dataContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = dataContext;
        }


        public bool UpdateUserRole(string userId, string roleId)
        {
            var userRole = _context.UserRoles.Where(x => x.UserId == userId).FirstOrDefault();

            userRole.RoleId = roleId;

            _context.SaveChanges();

            return true;

        }



        public async Task<User> FindUser(string email, int taxNumber, string identification)
        {
            return await _context.Users.Where(x => x.Email == email || x.TaxNumber == taxNumber || x.Identification == identification).FirstAsync();

        }

        public async Task<string> GetRoleNameByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            return role.Name;

        }



        public async Task<string> GetRoleNameAsync(User user)
        {
            var roleUser = await _context.UserRoles.Where(r => r.UserId == user.Id).FirstOrDefaultAsync();

            var role = await _roleManager.FindByIdAsync(Convert.ToString(roleUser.RoleId));
            return role.Name;

        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task AddUserToRoleAsync(User user, string roleName)
        {
            await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> CHangePasswordAsync(User user, string oldPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        }

        public async Task CheckRoleAsync(string roleName)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await _roleManager.CreateAsync(new IdentityRole
                {
                    Name = roleName
                });
            }

        }

        public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<bool> IsUserInRoleAsync(User user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, "Admin");
        }

 

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
        {
            return await _userManager.ResetPasswordAsync(user, token, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(
                user,
                password,
                false);
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string role)
        {

            return await _userManager.GetUsersInRoleAsync(role);

        }

        public async Task<SignInResult> LoginAsyncWithEmail(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
                model.Username,
                model.Password,
                model.RememberMe,
                false);
        }

        public async Task<SignInResult> LoginAsyncWithClientNumber(LoginViewModel model)
        {
            return await _signInManager.PasswordSignInAsync(
             model.Client_Number,
             model.Password,
             model.RememberMe,
             false);
        }
    }
}
