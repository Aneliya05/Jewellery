using EllaJewelry.Infrastructure.Data;
using EllaJewelry.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Core.DbServices
{
    public class UserServices
    {
        private readonly UserManager<User> _userManager;
        private readonly EllaJewelryDbContext _dbContext;
        public UserServices(EllaJewelryDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        #region SIGN UP
        public async Task<Tuple<IdentityResult, User>> CreateAccountAsync(string firstName, string surname, string email, string password)
        {
            try
            {
                User user = new User(email, firstName, surname);
                IdentityResult result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    return new Tuple<IdentityResult, User>(result, user);
                }
                await _userManager.AddToRoleAsync(user, "User");
                return new Tuple<IdentityResult, User>(result, user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region LOG IN
        public async Task<Tuple<IdentityResult, User>> LogInUserAsync(string email, string password)
        {
            IdentityResult result;
            try
            {
                User user = await _userManager.FindByEmailAsync(email);

                if (user == null)
                {
                    // create IdentityError => "Username not found!"
                    result = IdentityResult.Failed(new IdentityError() { Code = "Login", Description = "User with that name does not exist!" });
                    return new Tuple<IdentityResult, User>(result, user);
                }


                //result = await _userManager.PasswordValidators[0].ValidateAsync(_userManager, user, password);
                bool isPasswordValid = await _userManager.CheckPasswordAsync(user, password);


                if (isPasswordValid)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);

                    result = IdentityResult.Success;
                    return new Tuple<IdentityResult, User>(result, user);
                }
                else
                {
                    await _userManager.AccessFailedAsync(user);

                    if (user.AccessFailedCount == _userManager.Options.Lockout.MaxFailedAccessAttempts)
                    {
                        result = IdentityResult.Failed(new IdentityError()
                        {
                            Code = "Login",
                            Description = $"Too many fails! :[ Try again after " +
                            $"{_userManager.Options.Lockout.DefaultLockoutTimeSpan} minutes!"
                        });
                    }
                    else
                    {
                        result = IdentityResult.Failed(new IdentityError()
                        {
                            Code = "Login",
                            Description = $"Password is not correct!" +
                            $"{_userManager.Options.Lockout.MaxFailedAccessAttempts - user.AccessFailedCount} attempts left!"
                        });
                    }

                    return new Tuple<IdentityResult, User>(result, user);
                }
            }
            catch (Exception ex)
            {
                result = IdentityResult.Failed(new IdentityError()
                {
                    Code = "Login",
                    Description = ex.Message
                });
                return new Tuple<IdentityResult, User>(result, null);
            }
        }
        #endregion
        public async Task<User> ReadUserAsync(string key)
        {
            return await _userManager.FindByIdAsync(key);
        }

        public async Task<IEnumerable<User>> ReadAllUsersAsync()
        {
            return await _dbContext.Users.ToListAsync();
        }
        #region Account Features
        public async Task UpdateAccountAsync(string firstName, string surname, string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                User userToBeEdited = await _userManager.FindByEmailAsync(email);
                userToBeEdited.FirstName = firstName;
                userToBeEdited.LastName = surname;
                userToBeEdited.Email = email;
                await _userManager.UpdateAsync(userToBeEdited);
            }


            //userToBeEdited.Password = editedUser.Password;
            //userToBeEdited.Role = editedUser.Role;

        }

        //public async Task ResetPasswordAsync(string id, string newPassword)
        //{
        //    User user = await _userManager.FindByIdAsync(id);
        //    await _userManager.ResetPasswordAsync(user, newPassword);
        //    await _userManager.UpdateAsync(user);
        //}
        public async Task DeleteAccountAsync(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                throw new InvalidOperationException("User not found for deletion");
            }
            await _userManager.DeleteAsync(user);
        }

        #endregion
        #region CRUD for Roles

        public async Task CreateRoleAsync(IdentityRole role)
        {
            try
            {
                _dbContext.Roles.Add(role);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new ArgumentException("The role already exists");
            }
        }
        #endregion
    }
}
