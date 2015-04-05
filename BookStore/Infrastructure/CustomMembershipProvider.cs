using BookStore.DAL;
using BookStore.DLL.Abstract;
using BookStore.DO.Entities;
using Microsoft.Practices.ServiceLocation;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BookStore.Infrastructure
{
     
    public class CustomMembershipProvider : MembershipProvider
    {
        private IUserService userService;
        private IRoleService roleService;
        
        public CustomMembershipProvider()
        {
            userService = ServiceLocator.Current.GetInstance<IUserService>();
            roleService = ServiceLocator.Current.GetInstance<IRoleService>();

        }
        public CustomMembershipProvider(IUserService user_service, IRoleService role_service)
        {
            userService = user_service;
            roleService = role_service;
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
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }
        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            try
            {
                User user = userService.GetUserByEmail(username);
                if (user != null && user.Password == password) isValid = true;
            }
            catch
            {
                isValid = false;
            }
            return isValid;
        }
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public MembershipUser CreateUser(string first_name, string last_name, string password, string email, string avatar_url, DateTime birthday, string sex)
        {
            MembershipUser membership = GetUser(email, false);
            if (membership == null)
            {
                try
                {
                    User user = new User();
                    user.Email = email;
                    user.First_Name = first_name;
                    user.Last_Name = last_name;
                    user.Birthday = birthday;
                    user.Password = password;
                    user.Avatar_Url = avatar_url;
                    user.Sex = sex;
                    user.Rating = 0;
                    Role role = roleService.GetRoleByName("user");
                    if (role != null) user.Roles.Add(role);
                    userService.Save(user);
                    membership = GetUser(email, false);
                    return membership;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        public override MembershipUser GetUser(string email, bool userIsOnline)
        {
            try
            {
                User user = userService.GetUserByEmail(email);
                if (user != null)
                {
                    MembershipUser memberUser = new MembershipUser("MyMembershipProvider", user.Email, null, null, null, null,
                false, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
                    return memberUser;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
    }
}