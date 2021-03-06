﻿using Microsoft.AspNetCore.Identity;
using ProjFinalCinelAir.CommonCore.Data.Entities;
using ProjFinalCinelAir.CommonCore.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjFinalCinelAir.CommonCore.Helper
{
    public interface IUserHelper
    {

       
            bool UpdateUserRole(string userId, string roleId);

            Task<User> FindUser(string email, int taxNumber, string identification);

            Task<string> GetRoleNameByIdAsync(string id);

            Task<IdentityResult> AddUserAsync(User user, string password); //permite criar um utilizador

            Task<string> GetRoleNameAsync(User user);

            Task<User> GetUserByEmailAsync(string email); //vai procurar o email e retorna-lo


            Task<SignInResult> LoginAsyncWithEmail(LoginViewModel model);

            Task<SignInResult> LoginAsyncWithClientNumber(LoginViewModel model);

            Task LogoutAsync();

            Task<IdentityResult> UpdateUserAsync(User user);

            Task<IdentityResult> CHangePasswordAsync(User user, string oldPassword, string newPassword);

            Task<SignInResult> ValidatePasswordAsync(User user, string password);


            Task CheckRoleAsync(string roleName);

            Task<bool> IsUserInRoleAsync(User user, string roleName);

            Task AddUserToRoleAsync(User user, string roleName);

            Task<string> GenerateEmailConfirmationTokenAsync(User user);

            Task<IdentityResult> ConfirmEmailAsync(User user, string token);

            Task<User> GetUserByIdAsync(string userId);

            Task<string> GeneratePasswordResetTokenAsync(User user);

            Task<IdentityResult> ResetPasswordAsync(User user, string token, string password);

            Task<IList<User>> GetUsersInRoleAsync(string role);
        
    }
}
