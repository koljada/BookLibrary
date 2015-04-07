using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Ninject;
using BookStore.DLL.Abstract;
using BookStore.DO.Entities;
using System.Web.Mvc;


namespace BookStore.Infrastructure
{
    
    public class CustomRoleProvider : RoleProvider
    {
        [Inject]
        public IUserService userService { get; set; }
        [Inject]
        public IRoleService roleService{ get; set; }
        public CustomRoleProvider()
        {
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
            string[] role = { };
            try
            {
                // Получаем пользователя
                User user = userService.GetUserByEmail(email);
                if (user != null)
                {
                    // получаем роль
                    var userRoles = userService.GetRoles(user.User_ID);
                    if (userRoles != null)
                    {
                        role = userRoles.Select(u => u.Name).ToArray();
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
                    if (userRoles.Any(role => role != null && role.Name == roleName))
                    {
                        outputResult = true;
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