using Microsoft.AspNetCore.Identity;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.Admin.Application.ServiceInterface
{
    public interface IUserService
    {
        Task<string> Login(string username, string password, bool rememberMe);
        Task<bool> Register(ApplicationUser applicationUser, string password);
        Task Logout();
        Task<List<object>> GetAllUsersInfo();
        Task DeleteUser(UserInfo userInfo);
        Task UpdateRol(UserInfo userInfo);
        IEnumerable<IdentityRole> getRoles();

        Task<ApplicationUser> GetById(string id);
    }
}
