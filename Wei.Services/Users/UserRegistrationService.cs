using System;
using Wei.Core;
using Wei.Core.Domain.Users;

namespace Wei.Services.Users
{
    public class UserRegistrationService : IUserRegistrationService
    {
        #region fields
        private readonly IUserService _userService;

        #endregion

        #region ctor
        public UserRegistrationService(IUserService userService)
        {
            this._userService = userService;
        }
        #endregion


        public ChangePasswordResult ChangePassword(ChangePasswordRequest request)
        {
            throw new NotImplementedException();
        }

        public UserRegistrationResult RegisterUser(UserRegistrationRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            if (request.User == null)
                throw new ArgumentException("Can't load current customer");

            var result = new UserRegistrationResult();
            //if (request.User.IsSearchEngineAccount())
            //{
            //    result.AddError("Search engine can't be registered");
            //    return result;
            //}
            //if (request.Customer.IsBackgroundTaskAccount())
            //{
            //    result.AddError("Background task account can't be registered");
            //    return result;
            //}
            //if (request.Customer.IsRegistered())
            //{
            //    result.AddError("Current customer is already registered");
            //    return result;
            //}
            //if (String.IsNullOrEmpty(request.Email))
            //{
            //    result.AddError(_localizationService.GetResource("Account.Register.Errors.EmailIsNotProvided"));
            //    return result;
            //}
            //if (!CommonHelper.IsValidEmail(request.Email))
            //{
            //    result.AddError(_localizationService.GetResource("Common.WrongEmail"));
            //    return result;
            //}
            //if (String.IsNullOrWhiteSpace(request.Password))
            //{
            //    result.AddError(_localizationService.GetResource("Account.Register.Errors.PasswordIsNotProvided"));
            //    return result;
            //}
            //if (_customerSettings.UsernamesEnabled)
            //{
            //    if (String.IsNullOrEmpty(request.Username))
            //    {
            //        result.AddError(_localizationService.GetResource("Account.Register.Errors.UsernameIsNotProvided"));
            //        return result;
            //    }
            //}

            //validate unique user
            //if (_customerService.GetCustomerByEmail(request.Email) != null)
            //{
            //    result.AddError(_localizationService.GetResource("Account.Register.Errors.EmailAlreadyExists"));
            //    return result;
            //}
            //if (_customerSettings.UsernamesEnabled)
            //{
            //    if (_customerService.GetCustomerByUsername(request.Username) != null)
            //    {
            //        result.AddError(_localizationService.GetResource("Account.Register.Errors.UsernameAlreadyExists"));
            //        return result;
            //    }
            //}

            //at this point request is valid
            request.User.LoginName = request.LoginName;
            request.User.Email = request.Email;

            request.User.Password = CommonHelper.GetMD5(request.Password, request.User.PasswordSalt);

            ////add to 'Registered' role
            //var registeredRole = _userService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered);
            //if (registeredRole == null)
            //    throw new WeiException("'Registered' role could not be loaded");
            //request.User.CustomerRoles.Add(registeredRole);
            ////remove from 'Guests' role
            //var guestRole = request.User.CustomerRoles.FirstOrDefault(cr => cr.SystemName == SystemCustomerRoleNames.Guests);
            //if (guestRole != null)
            //    request.User.CustomerRoles.Remove(guestRole);

            ////Add reward points for customer registration (if enabled)
            //if (_rewardPointsSettings.Enabled &&
            //    _rewardPointsSettings.PointsForRegistration > 0)
            //{
            //    _rewardPointService.AddRewardPointsHistoryEntry(request.User,
            //        _rewardPointsSettings.PointsForRegistration,
            //        request.StoreId,
            //        _localizationService.GetResource("RewardPoints.Message.EarnedForRegistration"));
            //}

            //_userService.UpdateCustomer(request.User);
            return result;
        }

        public void SetEmail(User user, string newEmail)
        {
            throw new NotImplementedException();
        }

        public void SetUsername(User user, string newUsername)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取登录结果
        /// </summary>
        /// <param name="loginname"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public UserLoginResults ValidateUser(string loginname, string password)
        {
            var user = _userService.GetUserByLoginName(loginname);

            if (user == null)
                return UserLoginResults.CustomerNotExist;
            if (user.Status == -1)
                return UserLoginResults.Deleted;
            if (user.Status == 0)
                return UserLoginResults.NotActive;
            //only registered can login
            //if (!user.IsRegistered())
            //    return UserLoginResults.NotRegistered;

            string pwd = CommonHelper.GetMD5(password, user.PasswordSalt);

            bool isValid = pwd == user.Password;
            if (!isValid)
                return UserLoginResults.WrongPassword;

            //save last login date
            user.LastLoginTime = DateTime.UtcNow;
            _userService.UpdateUser(user);
            return UserLoginResults.Successful;
        }
    }
}
