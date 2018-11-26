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

        public virtual User GetUserById(string openid)
        {
            if (string.IsNullOrEmpty(openid))
                throw new ArgumentNullException("User");
            return this._userRepository.Table.FirstOrDefault(x => x.OpenId == openid);
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
