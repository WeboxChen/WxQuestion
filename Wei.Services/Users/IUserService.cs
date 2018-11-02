using System.Collections.Generic;
using Wei.Core.Domain.Users;

namespace Wei.Services.Users
{
    public interface IUserService
    {
        #region Users
        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user">User</param>
        void DeleteUser(User user);

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>A user</returns>
        User GetUserById(int userId);

        User GetUserById(string gid);

        /// <summary>
        /// Get users by identifiers
        /// </summary>
        /// <param name="userIds">User identifiers</param>
        /// <returns>Users</returns>
        IList<User> GetUsersByIds(int[] userIds);

        /// <summary>
        /// Get user by loginname
        /// </summary>
        /// <param name="username">Loginname</param>
        /// <returns>User</returns>
        User GetUserByLoginName(string loginname);

        /// <summary>
        /// Insert a user
        /// </summary>
        /// <param name="user">User</param>
        void InsertUser(User user);

        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        void UpdateUser(User user);

        #endregion

        #region User roles

        /// <summary>
        /// Delete a user role
        /// </summary>
        /// <param name="userRole">User role</param>
        void DeleteRole(Role userRole);

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="userRoleId">User role identifier</param>
        /// <returns>User role</returns>
        Role GetRoleById(int userRoleId);

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="code">User role code</param>
        /// <returns>User role</returns>
        Role GetRoleByCode(string code);

        /// <summary>
        /// Gets all user roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>User roles</returns>
        IList<Role> GetAllRoles(bool showHidden = false);

        /// <summary>
        /// Inserts a user role
        /// </summary>
        /// <param name="userRole">User role</param>
        void InsertRole(Role userRole);

        /// <summary>
        /// Updates the user role
        /// </summary>
        /// <param name="userRole">User role</param>
        void UpdateRole(Role userRole);

        #endregion

        #region User permission

        /// <summary>
        /// 根据session判断是否有效
        /// 有效: 为用户获取新的LoginPermission
        /// 无效: return null;
        /// </summary>
        /// <param name="session"></param>
        /// <returns>null:无效，  LoginPermission:有效</returns>
        LoginPermission CheckPermission();

        User GetAPICurrentUser();

        void NewLoginPermission(User user);
        #endregion
    }
}
