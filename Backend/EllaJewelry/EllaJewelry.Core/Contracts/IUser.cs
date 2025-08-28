using EllaJewelry.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EllaJewelry.Core.Contracts
{
    public interface IUser
    {
        // SIGN UP
        Task<Tuple<IdentityResult, User>> CreateAccountAsync(string firstName, string surname, string email, string password);

        // LOG IN
        Task<Tuple<IdentityResult, User>> LogInUserAsync(string email, string password);

        // READ
        Task<User> ReadUserAsync(string key);
        Task<IEnumerable<User>> ReadAllUsersAsync();

        // ACCOUNT FEATURES
        Task UpdateAccountAsync(string firstName, string surname, string email);
        Task DeleteAccountAsync(string id);

        // ROLES
        Task CreateRoleAsync(IdentityRole role);
    }
}
