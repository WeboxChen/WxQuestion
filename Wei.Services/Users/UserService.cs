using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wei.Core.Caching;
using Wei.Core.Data;
using Wei.Core.Domain.Users;
using Wei.Data;
using Wei.Services.Events;

namespace Wei.Services.Users
{
    /// <summary>
    /// User service
    /// </summary>
    public class UserService : IUserService
    {
        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string ROLES_ALL_KEY = "WEI.userrole.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        private const string ROLES_BY_SYSTEMNAME_KEY = "WEI.userrole.systemname-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string ROLES_PATTERN_KEY = "WEI.userrole.";

        #endregion

        #region Fields

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IRepository<LoginPermission> _loginpermissionRepository;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly HttpContextBase _httpContext;

        #endregion

        #region Ctor

        public UserService(ICacheManager cacheManager,
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IRepository<LoginPermission> loginpermissionRepository,
            IDataProvider dataProvider,
            IDbContext dbContext,
            HttpContextBase httpContext,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._userRepository = userRepository;
            this._roleRepository = roleRepository;
            this._loginpermissionRepository = loginpermissionRepository;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._eventPublisher = eventPublisher;
            this._httpContext = httpContext;
        }

        #endregion

        #region Methods

        #region Users

        ///// <summary>
        ///// Gets all users
        ///// </summary>
        ///// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        ///// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        ///// <param name="affiliateId">Affiliate identifier</param>
        ///// <param name="vendorId">Vendor identifier</param>
        ///// <param name="roleIds">A list of user role identifiers to filter by (at least one match); pass null or empty list in order to load all users; </param>
        ///// <param name="email">Email; null to load all users</param>
        ///// <param name="username">Username; null to load all users</param>
        ///// <param name="firstName">First name; null to load all users</param>
        ///// <param name="lastName">Last name; null to load all users</param>
        ///// <param name="dayOfBirth">Day of birth; 0 to load all users</param>
        ///// <param name="monthOfBirth">Month of birth; 0 to load all users</param>
        ///// <param name="company">Company; null to load all users</param>
        ///// <param name="phone">Phone; null to load all users</param>
        ///// <param name="zipPostalCode">Phone; null to load all users</param>
        ///// <param name="loadOnlyWithShoppingCart">Value indicating whether to load users only with shopping cart</param>
        ///// <param name="sct">Value indicating what shopping cart type to filter; userd when 'loadOnlyWithShoppingCart' param is 'true'</param>
        ///// <param name="pageIndex">Page index</param>
        ///// <param name="pageSize">Page size</param>
        ///// <returns>Users</returns>
        //public virtual IPagedList<User> GetAllUsers(DateTime? createdFromUtc = null,
        //    DateTime? createdToUtc = null, int affiliateId = 0, int vendorId = 0,
        //    int[] roleIds = null, string email = null, string username = null,
        //    string firstName = null, string lastName = null,
        //    int dayOfBirth = 0, int monthOfBirth = 0,
        //    string company = null, string phone = null, string zipPostalCode = null,
        //    bool loadOnlyWithShoppingCart = false, ShoppingCartType? sct = null,
        //    int pageIndex = 0, int pageSize = 2147483647)
        //{
        //    var query = _userRepository.Table;
        //    if (createdFromUtc.HasValue)
        //        query = query.Where(c => createdFromUtc.Value <= c.CreatedOnUtc);
        //    if (createdToUtc.HasValue)
        //        query = query.Where(c => createdToUtc.Value >= c.CreatedOnUtc);
        //    if (affiliateId > 0)
        //        query = query.Where(c => affiliateId == c.AffiliateId);
        //    if (vendorId > 0)
        //        query = query.Where(c => vendorId == c.VendorId);
        //    query = query.Where(c => !c.Deleted);
        //    if (roleIds != null && roleIds.Length > 0)
        //        query = query.Where(c => c.Roles.Select(cr => cr.Id).Intersect(roleIds).Any());
        //    if (!String.IsNullOrWhiteSpace(email))
        //        query = query.Where(c => c.Email.Contains(email));
        //    if (!String.IsNullOrWhiteSpace(username))
        //        query = query.Where(c => c.Username.Contains(username));
        //    if (!String.IsNullOrWhiteSpace(firstName))
        //    {
        //        query = query
        //            .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
        //            .Where((z => z.Attribute.KeyGroup == "User" &&
        //                z.Attribute.Key == SystemUserAttributeNames.FirstName &&
        //                z.Attribute.Value.Contains(firstName)))
        //            .Select(z => z.User);
        //    }
        //    if (!String.IsNullOrWhiteSpace(lastName))
        //    {
        //        query = query
        //            .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
        //            .Where((z => z.Attribute.KeyGroup == "User" &&
        //                z.Attribute.Key == SystemUserAttributeNames.LastName &&
        //                z.Attribute.Value.Contains(lastName)))
        //            .Select(z => z.User);
        //    }
        //    //date of birth is stored as a string into database.
        //    //we also know that date of birth is stored in the following format YYYY-MM-DD (for example, 1983-02-18).
        //    //so let's search it as a string
        //    if (dayOfBirth > 0 && monthOfBirth > 0)
        //    {
        //        //both are specified
        //        string dateOfBirthStr = monthOfBirth.ToString("00", CultureInfo.InvariantCulture) + "-" + dayOfBirth.ToString("00", CultureInfo.InvariantCulture);
        //        //EndsWith is not supported by SQL Server Compact
        //        //so let's use the following workaround http://social.msdn.microsoft.com/Forums/is/sqlce/thread/0f810be1-2132-4c59-b9ae-8f7013c0cc00

        //        //we also cannot use Length function in SQL Server Compact (not supported in this context)
        //        //z.Attribute.Value.Length - dateOfBirthStr.Length = 5
        //        //dateOfBirthStr.Length = 5
        //        query = query
        //            .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
        //            .Where((z => z.Attribute.KeyGroup == "User" &&
        //                z.Attribute.Key == SystemUserAttributeNames.DateOfBirth &&
        //                z.Attribute.Value.Substring(5, 5) == dateOfBirthStr))
        //            .Select(z => z.User);
        //    }
        //    else if (dayOfBirth > 0)
        //    {
        //        //only day is specified
        //        string dateOfBirthStr = dayOfBirth.ToString("00", CultureInfo.InvariantCulture);
        //        //EndsWith is not supported by SQL Server Compact
        //        //so let's use the following workaround http://social.msdn.microsoft.com/Forums/is/sqlce/thread/0f810be1-2132-4c59-b9ae-8f7013c0cc00

        //        //we also cannot use Length function in SQL Server Compact (not supported in this context)
        //        //z.Attribute.Value.Length - dateOfBirthStr.Length = 8
        //        //dateOfBirthStr.Length = 2
        //        query = query
        //            .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
        //            .Where((z => z.Attribute.KeyGroup == "User" &&
        //                z.Attribute.Key == SystemUserAttributeNames.DateOfBirth &&
        //                z.Attribute.Value.Substring(8, 2) == dateOfBirthStr))
        //            .Select(z => z.User);
        //    }
        //    else if (monthOfBirth > 0)
        //    {
        //        //only month is specified
        //        string dateOfBirthStr = "-" + monthOfBirth.ToString("00", CultureInfo.InvariantCulture) + "-";
        //        query = query
        //            .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
        //            .Where((z => z.Attribute.KeyGroup == "User" &&
        //                z.Attribute.Key == SystemUserAttributeNames.DateOfBirth &&
        //                z.Attribute.Value.Contains(dateOfBirthStr)))
        //            .Select(z => z.User);
        //    }
        //    //search by company
        //    if (!String.IsNullOrWhiteSpace(company))
        //    {
        //        query = query
        //            .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
        //            .Where((z => z.Attribute.KeyGroup == "User" &&
        //                z.Attribute.Key == SystemUserAttributeNames.Company &&
        //                z.Attribute.Value.Contains(company)))
        //            .Select(z => z.User);
        //    }
        //    //search by phone
        //    if (!String.IsNullOrWhiteSpace(phone))
        //    {
        //        query = query
        //            .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
        //            .Where((z => z.Attribute.KeyGroup == "User" &&
        //                z.Attribute.Key == SystemUserAttributeNames.Phone &&
        //                z.Attribute.Value.Contains(phone)))
        //            .Select(z => z.User);
        //    }
        //    //search by zip
        //    if (!String.IsNullOrWhiteSpace(zipPostalCode))
        //    {
        //        query = query
        //            .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
        //            .Where((z => z.Attribute.KeyGroup == "User" &&
        //                z.Attribute.Key == SystemUserAttributeNames.ZipPostalCode &&
        //                z.Attribute.Value.Contains(zipPostalCode)))
        //            .Select(z => z.User);
        //    }

        //    if (loadOnlyWithShoppingCart)
        //    {
        //        int? sctId = null;
        //        if (sct.HasValue)
        //            sctId = (int)sct.Value;

        //        query = sct.HasValue ?
        //            query.Where(c => c.ShoppingCartItems.Any(x => x.ShoppingCartTypeId == sctId)) :
        //            query.Where(c => c.ShoppingCartItems.Any());
        //    }

        //    query = query.OrderByDescending(c => c.CreatedOnUtc);

        //    var users = new PagedList<User>(query, pageIndex, pageSize);
        //    return users;
        //}

        ///// <summary>
        ///// Gets all users by user format (including deleted ones)
        ///// </summary>
        ///// <param name="passwordFormat">Password format</param>
        ///// <returns>Users</returns>
        //public virtual IList<User> GetAllUsersByPasswordFormat(PasswordFormat passwordFormat)
        //{
        //    var passwordFormatId = (int)passwordFormat;

        //    var query = _userRepository.Table;
        //    query = query.Where(c => c.PasswordFormatId == passwordFormatId);
        //    query = query.OrderByDescending(c => c.CreatedOnUtc);
        //    var users = query.ToList();
        //    return users;
        //}

        ///// <summary>
        ///// Gets online users
        ///// </summary>
        ///// <param name="lastActivityFromUtc">User last activity date (from)</param>
        ///// <param name="roleIds">A list of user role identifiers to filter by (at least one match); pass null or empty list in order to load all users; </param>
        ///// <param name="pageIndex">Page index</param>
        ///// <param name="pageSize">Page size</param>
        ///// <returns>Users</returns>
        //public virtual IPagedList<User> GetOnlineUsers(DateTime lastActivityFromUtc,
        //    int[] roleIds, int pageIndex = 0, int pageSize = int.MaxValue)
        //{
        //    var query = _userRepository.Table;
        //    query = query.Where(c => lastActivityFromUtc <= c.LastActivityDateUtc);
        //    query = query.Where(c => !c.Deleted);
        //    if (roleIds != null && roleIds.Length > 0)
        //        query = query.Where(c => c.Roles.Select(cr => cr.Id).Intersect(roleIds).Any());

        //    query = query.OrderByDescending(c => c.LastActivityDateUtc);
        //    var users = new PagedList<User>(query, pageIndex, pageSize);
        //    return users;
        //}

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            user.Status = -1;

            //if (_userSettings.SuffixDeletedUsers)
            //{
            //    if (!String.IsNullOrEmpty(user.Email))
            //        user.Email += "-DELETED";
            //    if (!String.IsNullOrEmpty(user.LoginName))
            //        user.LoginName += "-DELETED";
            //}

            UpdateUser(user);
        }

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>A user</returns>
        public virtual User GetUserById(int userId)
        {
            if (userId == 0)
                return null;

            return _userRepository.GetById(userId);
        }

        public virtual User GetUserById(string gid)
        {
            if (string.IsNullOrEmpty(gid))
                throw new ArgumentNullException("User");
            return this._userRepository.Table.FirstOrDefault(x => x.GId == gid);
        }

        /// <summary>
        /// Get users by identifiers
        /// </summary>
        /// <param name="userIds">User identifiers</param>
        /// <returns>Users</returns>
        public virtual IList<User> GetUsersByIds(int[] userIds)
        {
            if (userIds == null || userIds.Length == 0)
                return new List<User>();

            var query = from c in _userRepository.Table
                        where userIds.Contains(c.Id)
                        select c;
            var users = query.ToList();
            //sort by passed identifiers
            var sortedUsers = new List<User>();
            foreach (int id in userIds)
            {
                var user = users.Find(x => x.Id == id);
                if (user != null)
                    sortedUsers.Add(user);
            }
            return sortedUsers;
        }

        ///// <summary>
        ///// Gets a user by GUID
        ///// </summary>
        ///// <param name="userGuid">User GUID</param>
        ///// <returns>A user</returns>
        //public virtual User GetUserByGuid(Guid userGuid)
        //{
        //    if (userGuid == Guid.Empty)
        //        return null;

        //    var query = from c in _userRepository.Table
        //                where c.UserGuid == userGuid
        //                orderby c.Id
        //                select c;
        //    var user = query.FirstOrDefault();
        //    return user;
        //}

        ///// <summary>
        ///// Get user by email
        ///// </summary>
        ///// <param name="email">Email</param>
        ///// <returns>User</returns>
        //public virtual User GetUserByEmail(string email)
        //{
        //    if (string.IsNullOrWhiteSpace(email))
        //        return null;

        //    var query = from c in _userRepository.Table
        //                orderby c.Id
        //                where c.Email == email
        //                select c;
        //    var user = query.FirstOrDefault();
        //    return user;
        //}

        ///// <summary>
        ///// Get user by system name
        ///// </summary>
        ///// <param name="systemName">System name</param>
        ///// <returns>User</returns>
        //public virtual User GetUserBySystemName(string systemName)
        //{
        //    if (string.IsNullOrWhiteSpace(systemName))
        //        return null;

        //    var query = from c in _userRepository.Table
        //                orderby c.Id
        //                where c.SystemName == systemName
        //                select c;
        //    var user = query.FirstOrDefault();
        //    return user;
        //}

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User</returns>
        public virtual User GetUserByLoginName(string loginname)
        {
            if (string.IsNullOrWhiteSpace(loginname))
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.LoginName == loginname
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Insert a user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.Insert(user);

            //event notification
            _eventPublisher.EntityInserted(user);
        }

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.Update(user);

            //event notification
            _eventPublisher.EntityUpdated(user);
        }

        ///// <summary>
        ///// Reset data required for checkout
        ///// </summary>
        ///// <param name="user">User</param>
        ///// <param name="storeId">Store identifier</param>
        ///// <param name="clearCouponCodes">A value indicating whether to clear coupon code</param>
        ///// <param name="clearCheckoutAttributes">A value indicating whether to clear selected checkout attributes</param>
        ///// <param name="clearRewardPoints">A value indicating whether to clear "Use reward points" flag</param>
        ///// <param name="clearShippingMethod">A value indicating whether to clear selected shipping method</param>
        ///// <param name="clearPaymentMethod">A value indicating whether to clear selected payment method</param>
        //public virtual void ResetCheckoutData(User user, int storeId,
        //    bool clearCouponCodes = false, bool clearCheckoutAttributes = false,
        //    bool clearRewardPoints = true, bool clearShippingMethod = true,
        //    bool clearPaymentMethod = true)
        //{
        //    if (user == null)
        //        throw new ArgumentNullException();

        //    //clear entered coupon codes
        //    if (clearCouponCodes)
        //    {
        //        _genericAttributeService.SaveAttribute<ShippingOption>(user, SystemUserAttributeNames.DiscountCouponCode, null);
        //        _genericAttributeService.SaveAttribute<ShippingOption>(user, SystemUserAttributeNames.GiftCardCouponCodes, null);
        //    }

        //    //clear checkout attributes
        //    if (clearCheckoutAttributes)
        //    {
        //        _genericAttributeService.SaveAttribute<ShippingOption>(user, SystemUserAttributeNames.CheckoutAttributes, null, storeId);
        //    }

        //    //clear reward points flag
        //    if (clearRewardPoints)
        //    {
        //        _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.UseRewardPointsDuringCheckout, false, storeId);
        //    }

        //    //clear selected shipping method
        //    if (clearShippingMethod)
        //    {
        //        _genericAttributeService.SaveAttribute<ShippingOption>(user, SystemUserAttributeNames.SelectedShippingOption, null, storeId);
        //        _genericAttributeService.SaveAttribute<ShippingOption>(user, SystemUserAttributeNames.OfferedShippingOptions, null, storeId);
        //        _genericAttributeService.SaveAttribute(user, SystemUserAttributeNames.SelectedPickUpInStore, false, storeId);
        //    }

        //    //clear selected payment method
        //    if (clearPaymentMethod)
        //    {
        //        _genericAttributeService.SaveAttribute<string>(user, SystemUserAttributeNames.SelectedPaymentMethod, null, storeId);
        //    }

        //    UpdateUser(user);
        //}

        ///// <summary>
        ///// Delete guest user records
        ///// </summary>
        ///// <param name="createdFromUtc">Created date from (UTC); null to load all records</param>
        ///// <param name="createdToUtc">Created date to (UTC); null to load all records</param>
        ///// <param name="onlyWithoutShoppingCart">A value indicating whether to delete users only without shopping cart</param>
        ///// <returns>Number of deleted users</returns>
        //public virtual int DeleteGuestUsers(DateTime? createdFromUtc, DateTime? createdToUtc, bool onlyWithoutShoppingCart)
        //{
        //    if (_commonSettings.UseStoredProceduresIfSupported && _dataProvider.StoredProceduredSupported)
        //    {
        //        //stored procedures are enabled and supported by the database. 
        //        //It's much faster than the LINQ implementation below 

        //        #region Stored procedure

        //        //prepare parameters
        //        var pOnlyWithoutShoppingCart = _dataProvider.GetParameter();
        //        pOnlyWithoutShoppingCart.ParameterName = "OnlyWithoutShoppingCart";
        //        pOnlyWithoutShoppingCart.Value = onlyWithoutShoppingCart;
        //        pOnlyWithoutShoppingCart.DbType = DbType.Boolean;

        //        var pCreatedFromUtc = _dataProvider.GetParameter();
        //        pCreatedFromUtc.ParameterName = "CreatedFromUtc";
        //        pCreatedFromUtc.Value = createdFromUtc.HasValue ? (object)createdFromUtc.Value : DBNull.Value;
        //        pCreatedFromUtc.DbType = DbType.DateTime;

        //        var pCreatedToUtc = _dataProvider.GetParameter();
        //        pCreatedToUtc.ParameterName = "CreatedToUtc";
        //        pCreatedToUtc.Value = createdToUtc.HasValue ? (object)createdToUtc.Value : DBNull.Value;
        //        pCreatedToUtc.DbType = DbType.DateTime;

        //        var pTotalRecordsDeleted = _dataProvider.GetParameter();
        //        pTotalRecordsDeleted.ParameterName = "TotalRecordsDeleted";
        //        pTotalRecordsDeleted.Direction = ParameterDirection.Output;
        //        pTotalRecordsDeleted.DbType = DbType.Int32;

        //        //invoke stored procedure
        //        _dbContext.ExecuteSqlCommand(
        //            "EXEC [DeleteGuests] @OnlyWithoutShoppingCart, @CreatedFromUtc, @CreatedToUtc, @TotalRecordsDeleted OUTPUT",
        //            false, null,
        //            pOnlyWithoutShoppingCart,
        //            pCreatedFromUtc,
        //            pCreatedToUtc,
        //            pTotalRecordsDeleted);

        //        int totalRecordsDeleted = (pTotalRecordsDeleted.Value != DBNull.Value) ? Convert.ToInt32(pTotalRecordsDeleted.Value) : 0;
        //        return totalRecordsDeleted;

        //        #endregion
        //    }
        //    else
        //    {
        //        //stored procedures aren't supported. Use LINQ

        //        #region No stored procedure

        //        var guestRole = GetRoleBySystemName(SystemRoleNames.Guests);
        //        if (guestRole == null)
        //            throw new YPuException("'Guests' role could not be loaded");

        //        var query = _userRepository.Table;
        //        if (createdFromUtc.HasValue)
        //            query = query.Where(c => createdFromUtc.Value <= c.CreatedOnUtc);
        //        if (createdToUtc.HasValue)
        //            query = query.Where(c => createdToUtc.Value >= c.CreatedOnUtc);
        //        query = query.Where(c => c.Roles.Select(cr => cr.Id).Contains(guestRole.Id));
        //        if (onlyWithoutShoppingCart)
        //            query = query.Where(c => !c.ShoppingCartItems.Any());
        //        //no orders
        //        query = from c in query
        //                join o in _orderRepository.Table on c.Id equals o.UserId into c_o
        //                from o in c_o.DefaultIfEmpty()
        //                where !c_o.Any()
        //                select c;
        //        //no blog comments
        //        query = from c in query
        //                join bc in _blogCommentRepository.Table on c.Id equals bc.UserId into c_bc
        //                from bc in c_bc.DefaultIfEmpty()
        //                where !c_bc.Any()
        //                select c;
        //        //no news comments
        //        query = from c in query
        //                join nc in _newsCommentRepository.Table on c.Id equals nc.UserId into c_nc
        //                from nc in c_nc.DefaultIfEmpty()
        //                where !c_nc.Any()
        //                select c;
        //        //no product reviews
        //        query = from c in query
        //                join pr in _productReviewRepository.Table on c.Id equals pr.UserId into c_pr
        //                from pr in c_pr.DefaultIfEmpty()
        //                where !c_pr.Any()
        //                select c;
        //        //no product reviews helpfulness
        //        query = from c in query
        //                join prh in _productReviewHelpfulnessRepository.Table on c.Id equals prh.UserId into c_prh
        //                from prh in c_prh.DefaultIfEmpty()
        //                where !c_prh.Any()
        //                select c;
        //        //no poll voting
        //        query = from c in query
        //                join pvr in _pollVotingRecordRepository.Table on c.Id equals pvr.UserId into c_pvr
        //                from pvr in c_pvr.DefaultIfEmpty()
        //                where !c_pvr.Any()
        //                select c;
        //        //no forum posts 
        //        query = from c in query
        //                join fp in _forumPostRepository.Table on c.Id equals fp.UserId into c_fp
        //                from fp in c_fp.DefaultIfEmpty()
        //                where !c_fp.Any()
        //                select c;
        //        //no forum topics
        //        query = from c in query
        //                join ft in _forumTopicRepository.Table on c.Id equals ft.UserId into c_ft
        //                from ft in c_ft.DefaultIfEmpty()
        //                where !c_ft.Any()
        //                select c;
        //        //don't delete system accounts
        //        query = query.Where(c => !c.IsSystemAccount);

        //        //only distinct users (group by ID)
        //        query = from c in query
        //                group c by c.Id
        //                    into cGroup
        //                orderby cGroup.Key
        //                select cGroup.FirstOrDefault();
        //        query = query.OrderBy(c => c.Id);
        //        var users = query.ToList();


        //        int totalRecordsDeleted = 0;
        //        foreach (var c in users)
        //        {
        //            try
        //            {
        //                //delete attributes
        //                var attributes = _genericAttributeService.GetAttributesForEntity(c.Id, "User");
        //                foreach (var attribute in attributes)
        //                    _genericAttributeService.DeleteAttribute(attribute);


        //                //delete from database
        //                _userRepository.Delete(c);
        //                totalRecordsDeleted++;
        //            }
        //            catch (Exception exc)
        //            {
        //                Debug.WriteLine(exc);
        //            }
        //        }
        //        return totalRecordsDeleted;

        //        #endregion
        //    }
        //}

        #endregion

        #region User roles

        /// <summary>
        /// Delete a user role
        /// </summary>
        /// <param name="role">User role</param>
        public virtual void DeleteRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            _roleRepository.Delete(role);

            _cacheManager.RemoveByPattern(ROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(role);
        }

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="roleId">User role identifier</param>
        /// <returns>User role</returns>
        public virtual Role GetRoleById(int roleId)
        {
            if (roleId == 0)
                return null;

            return _roleRepository.GetById(roleId);
        }

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="systemName">User role system name</param>
        /// <returns>User role</returns>
        public virtual Role GetRoleByCode(string code)
        {
            if (String.IsNullOrWhiteSpace(code))
                return null;

            string key = string.Format(ROLES_BY_SYSTEMNAME_KEY, code);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _roleRepository.Table
                            orderby cr.Id
                            where cr.Code == code
                            select cr;
                var role = query.FirstOrDefault();
                return role;
            });
        }

        /// <summary>
        /// Gets all user roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>User roles</returns>
        public virtual IList<Role> GetAllRoles(bool showHidden = false)
        {
            string key = string.Format(ROLES_ALL_KEY, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _roleRepository.Table
                            orderby cr.Name
                            where (showHidden || cr.Status >= 0)
                            select cr;
                var roles = query.ToList();
                return roles;
            });
        }

        /// <summary>
        /// Inserts a user role
        /// </summary>
        /// <param name="role">User role</param>
        public virtual void InsertRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            _roleRepository.Insert(role);

            _cacheManager.RemoveByPattern(ROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(role);
        }

        /// <summary>
        /// Updates the user role
        /// </summary>
        /// <param name="role">User role</param>
        public virtual void UpdateRole(Role role)
        {
            if (role == null)
                throw new ArgumentNullException("role");

            _roleRepository.Update(role);

            _cacheManager.RemoveByPattern(ROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(role);
        }

        #endregion

        #region LoginPermission
        public LoginPermission CheckPermission()
        {
            string session = GetUserSession();

            if (string.IsNullOrEmpty(session))
                throw new ArgumentNullException("loginPermission.ssession");
            // 获取 loginpermission
            var oldP = (from loginpermission in _loginpermissionRepository.Table
                       where loginpermission.Session == session
                       select loginpermission).FirstOrDefault();
            if (oldP == null || oldP.IsEffectived == 0)
                return null;
            // 获取用户的最后访问时间
            var lastAccess = (from loginpermission in _loginpermissionRepository.Table
                              where loginpermission.UserId == oldP.UserId
                              orderby loginpermission.AccessTime descending
                              select loginpermission.AccessTime).FirstOrDefault();

            // 标识旧令牌失效
            oldP.IsEffectived = 0;
            _loginpermissionRepository.Update(oldP);

            if (lastAccess == oldP.AccessTime)
            {
                // 生成新令牌 并返回
                LoginPermission newP = new LoginPermission() {
                    AccessTime = DateTime.Now,
                    IsEffectived = 1,
                    Session = Guid.NewGuid().ToString("N"),
                    UserId = oldP.UserId
                };
                _loginpermissionRepository.Insert(newP);
                SetUserSession(newP);
                return newP;
            }
            
            return null;
        }

        public User GetAPICurrentUser()
        {
            string session = GetUserSession();
            if (!string.IsNullOrEmpty(session))
            {
                var oldP = (from loginpermission in _loginpermissionRepository.Table
                            where loginpermission.Session == session
                            select loginpermission).FirstOrDefault();
                if (oldP != null)
                    return oldP.User;
            }
            return null;
        }

        public void NewLoginPermission(User user)
        {
            // 标识之前的令牌无效
            var lastAccess = (from loginpermission in _loginpermissionRepository.Table
                              where loginpermission.UserId == user.Id && loginpermission.IsEffectived == 1
                              orderby loginpermission.AccessTime descending
                              select loginpermission).FirstOrDefault();
            if(lastAccess != null)
            {
                lastAccess.IsEffectived = 0;
                _loginpermissionRepository.Update(lastAccess);
            }

            // 生成新令牌 并返回
            LoginPermission newP = new LoginPermission()
            {
                AccessTime = DateTime.Now,
                IsEffectived = 1,
                Session = Guid.NewGuid().ToString("N"),
                UserId = user.Id
            };
            _loginpermissionRepository.Insert(newP);
            SetUserSession(newP);
        }

        private string GetUserSession()
        {
            var cookie = _httpContext.Request.Cookies["WEI_SESSION"];
            if(cookie!=null)
            return cookie.Value;
            return "";
        }
        private void SetUserSession(LoginPermission loginPermission)
        {
            HttpCookie cookie = new HttpCookie("WEI_SESSION");
            cookie.HttpOnly = true;
            cookie.Value = loginPermission.Session;
            _httpContext.Response.SetCookie(cookie);
        }
        #endregion

        #endregion

    }
}
