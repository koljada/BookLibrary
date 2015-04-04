using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data.Entity;
using Ninject;
using BookStore.DLL.Abstract;
using BookStore.DLL.Concrete;
using BookStore.DAL.Abstract;
using BookStore.DO.Entities;

namespace BookStore.Infrastructure
{
    public class CustomRoleProvider : RoleProvider
    {
        //IBookService _db = new BookService(IBookRepository repo);
        private IUserService userService;
        private IRoleService roleService;

        public CustomRoleProvider()
        {
        }
        public CustomRoleProvider(IUserService user_service, IRoleService role_service)
        {
            userService = user_service;
            roleService = role_service;

        }
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
        public override string[] GetRolesForUser(string email)
        {
            string[] role = new string[] { };
            try
            {
                // List<Role> listrole = _db.Roles.ToList();
                // Получаем пользователя
                User user = userService.GetUserByEmail(email);
                if (user != null)
                {
                    // получаем роль
                    var userRoles = userService.GetRoles(user.User_ID);
                    if (userRoles != null)
                    {
                        role = userRoles.Select(u => u.Name).ToArray<string>();
                    }
                }
            }
            catch
            {
                role = new string[] { };
            }
            return role;
        }
        public override void CreateRole(string roleName)
        {
            Role newRole = new Role() { Name = roleName };
            // EFDbContext db = new EFDbContext();
            roleService.Save(newRole);
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            bool outputResult = false;
            // Находим пользователя
            try
            {
                // Получаем пользователя
                User user = userService.GetUserByEmail(username);
                if (user != null)
                {
                    // получаем роль
                    List<Role> userRoles = userService.GetRoles(user.User_ID).ToList();
                    foreach (Role role in userRoles)
                    {
                        if (role != null && role.Name == roleName)
                        {
                            outputResult = true;
                            break;
                        }
                    }
                }
            }
            catch
            {
                outputResult = false;
            }
            return outputResult;
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}