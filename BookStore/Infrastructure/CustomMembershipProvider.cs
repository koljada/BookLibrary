using System;
using System.Collections.Generic;
using System.Web.Security;
using BookStore.DLL.Interface.Abstract;
using BookStore.DO.Entities;
using Ninject;

namespace BookStore.Infrastructure
{
    
   
    public class CustomMembershipProvider : MembershipProvider
    {
        private readonly string _applicationName = "CustomMembershipProvider";
        private readonly bool _enablePasswordReset = true;
        private readonly bool _enablePasswordRetrieval = false;
        private readonly int _maxInvalidPasswordAttempts = 5;
        private readonly int _minRequiredPasswordLength = 4;
        private readonly int _passwordAttemptWindow = 10;
        private readonly bool _requiresQuestionAndAnswer = false;
        private readonly bool _requiresUniqueEmail = true;

        [Inject]
        public IRoleService RoleService { get; set; }
        [Inject]
        public  IUserService UserService { get; set; }
        //public static class ExtentMembershipUser
        //{
        //    public static int GetUserId(this MembershipUser,string name)
        //    {
        //        return UserService.GetUserByEmail(name).User_ID;
        //    }
        //}
        public CustomMembershipProvider()
        {
        }
        
        public override MembershipUser CreateUser(string username, string password, string email,
            string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey,
            out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            var isValid = false;
            try
            {
                var user = UserService.GetUserByEmail(username);
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

        public override bool ChangePasswordQuestionAndAnswer(string username, string password,
            string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public MembershipUser CreateUser(string firstName, string lastName, string password, string email,
            string avatarUrl, DateTime birthday, string sex)
        {
            var membership = GetUser(email, false);
            if (membership == null)
            {
                try
                {
                    var user = new User();
                    user.Email = email;
                    user.Profile.First_Name = firstName;
                    user.Profile.Last_Name = lastName;
                    user.Profile.Birthday = birthday;
                    user.Password = password;
                    user.Profile.Avatar_Url = avatarUrl;
                    user.Profile.Sex = sex;
                    UserService.Create(user);
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
                var user = UserService.GetUserByEmail(email);
                if (user != null)
                {
                    var memberUser = new MembershipUser("MyMembershipProvider", user.Email, null, null, null, null,
                        false, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue,
                        DateTime.MinValue);
                    
                    return memberUser;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        public override string ApplicationName
        {
            get { return _applicationName; }
            set { value = _applicationName; }
        }

        public override int PasswordAttemptWindow
        {
            get { return _passwordAttemptWindow; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return _requiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return _requiresUniqueEmail; }
        }

        public override bool EnablePasswordReset
        {
            get { return _enablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return _enablePasswordRetrieval; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return _maxInvalidPasswordAttempts; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return _minRequiredPasswordLength; }
        }

        public override int MinRequiredNonAlphanumericCharacters
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


        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
            out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
            out int totalRecords)
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