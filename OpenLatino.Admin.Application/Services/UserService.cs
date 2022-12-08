using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Openlatino.Admin.Infrastucture.DataContexts;
using OpenLatino.Admin.Application.ServiceInterface;
using OpenLatino.Core.Domain.Entities;
using OpenLatino.Core.Domain.Interfaces;
using OpenLatino.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.Admin.Application.Services
{
    public class UserService : IUserService
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private RoleManager<IdentityRole> _roleManager;
        private AdminDb _dbContext;
        private IUnitOfWork _unitOfWork;

        public UserService(UserManager<ApplicationUser> userManager, AdminDb db,
                            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager,
                            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _dbContext = db;
            _unitOfWork = unitOfWork;
        }

        #region Auxiliar Methods
        public string GetRolName(IList<string> rols)
        {
            string result = "";
            for (int i = 0; i < rols.Count - 1; i++)
            {
                result += rols[i];
                result += ", ";
            }
            result += (rols.Count > 1) ? rols[rols.Count - 1] : rols[0];
            return result;
        }

        public List<IdentityUser> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }
        #endregion

        public async Task<string> Login(string username, string password, bool rememberMe)
        {
            var loginResults = await _signInManager.PasswordSignInAsync(username, password, rememberMe, lockoutOnFailure: false);
            string tokenString = null;

            if (loginResults.Succeeded)
            {
                var user = await _userManager.FindByNameAsync(username);
                var roles = await _userManager.GetRolesAsync(user);
                var role = roles.Contains("Admin") ? "Admin" : "RegularUser";

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super_SecretKEY.@@@$$$!!!92hwekjfn39urhu2n3d92ue923h3dun239d8jd982hf982hf28jd93hd92hf"));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Role, role),
                        new Claim(ClaimTypes.NameIdentifier, user.Id)
                    };

                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:5001",
                    audience: "https://localhost:5001",
                    claims: claims,
                    expires: DateTime.Now.AddHours(24),
                    signingCredentials: signinCredentials
                );
                tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            }

            return tokenString;
        }

        public async Task<bool> Register(ApplicationUser applicationUser, string password)
        {
            var identityResults = await _userManager.CreateAsync(applicationUser, password);
            if (identityResults.Succeeded)
            {
                await _signInManager.SignInAsync(applicationUser, isPersistent: false);
                await _userManager.AddToRoleAsync(applicationUser, "RegularUser");

                return true;
            }
            return false;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<List<object>> GetAllUsersInfo()
        {
            ClientService cs = new ClientService(_unitOfWork);
            WorkspaceService ws = new WorkspaceService(_unitOfWork);

            var aux = new List<object>();
            foreach (var item in GetAllUsers())
            {
                aux.Add(new
                {
                    Id = item.Id,
                    Email = item.Email,
                    Roles = (await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(item.Id))).ToList(),
                    Workspaces = ws.GetWorkSpacesForUser(item.Id).Select(w=>new { w.Id, w.Name}).ToList(),
                    ClientApps = cs.GetAllClientsForUser(item.Id).Select(c => new{ c.Id, c.Name}).ToList()
                });
            }
            return aux;
        }

        public async Task DeleteUser(UserInfo userInfo)
        {
            ClientService cs = new ClientService(_unitOfWork);
            WorkspaceService ws = new WorkspaceService(_unitOfWork);

            foreach (var item in cs.GetAllClientsForUser(userInfo.Id).ToArray())
                cs.Remove(item.Id);
            foreach (var item in ws.GetWorkSpacesForUser(userInfo.Id).ToArray())
                ws.Remove(item.Id);

            await _userManager.DeleteAsync(await _userManager.FindByIdAsync(userInfo.Id));
        }

        public async Task UpdateRol(UserInfo userInfo)
        {
            var user = await _userManager.FindByIdAsync(userInfo.Id);
            await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
            await _userManager.AddToRolesAsync(user, userInfo.RolsIds);
        }

        public IEnumerable<IdentityRole> getRoles()
        {
            return _roleManager.Roles.ToList();
        }

        public async Task<ApplicationUser> GetById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }
    }
}
