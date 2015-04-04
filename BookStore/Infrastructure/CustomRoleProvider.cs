using BookStore.DAL;
using BookStore.DAL.Abstract;
using BookStore.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Data.Entity;
using Ninject;
using BookStore.DAL.Concrete;

namespace BookStore.Infrastructure
{
    public class CustomRoleProvider : RoleProvider
    {
        IBookService _db = new EFBookRepository();
        public CustomRoleProvider()
        { 
        }
        public CustomRoleProvider(IBookService repository)
        {
            _db = repository;
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
                    
                    User user = _db.Users.Include(e => e.Role).FirstOrDefault(e => e.Email == email);
                            if (user != null)
                    {
                        // получаем роль
                        Role userRole = _db.Roles.FirstOrDefault(r => r.Role_ID == user.Role.Role_ID);
 
                        if (userRole != null)
                        {
                            role = new string[] { userRole.Name };
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
            EFDbContext db = new EFDbContext();
            db.Roles.Add(newRole);
            db.SaveChanges();
        }
        public override bool IsUserInRole(string username, string roleName)
        {
            bool outputResult = false;
            // Находим пользователя
            using (EFDbContext _db = new EFDbContext())
            {
                try
                {
                    // Получаем пользователя
                    User user = (from u in _db.Users
                                 where u.Email == username
                                 select u).FirstOrDefault();
                    if (user != null)
                    {
                        // получаем роль
                        Role userRole = _db.Roles.Find(user.Role.Role_ID);
 
                        //сравниваем
                        if (userRole != null && userRole.Name == roleName)
                        {
                            outputResult = true;
                        }
                    }
                }
                catch
                {
                    outputResult = false;
                }
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