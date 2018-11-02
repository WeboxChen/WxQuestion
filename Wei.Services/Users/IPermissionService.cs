using System.Collections.Generic;
using Wei.Core.Domain.Users;

namespace Wei.Services.Users
{
    /// <summary>
    /// Permission service interface
    /// </summary>
    public partial interface IPermissionService
    {
        /// <summary>
        /// Delete a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        void DeletePermissionRecord(PermissionRecord permission);

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="permissionId">Permission identifier</param>
        /// <returns>Permission</returns>
        PermissionRecord GetPermissionRecordById(int permissionId);

        /// <summary>
        /// Gets a permission
        /// </summary>
        /// <param name="code">Permission code</param>
        /// <returns>Permission</returns>
        PermissionRecord GetPermissionRecordByCode(string code);

        /// <summary>
        /// Gets all permissions
        /// </summary>
        /// <returns>Permissions</returns>
        IList<PermissionRecord> GetAllPermissionRecords();

        /// <summary>
        /// Inserts a permission
        /// </summary>
        /// <param name="permission">Permission</param>
        void InsertPermissionRecord(PermissionRecord permission);

        /// <summary>
        /// Updates the permission
        /// </summary>
        /// <param name="permission">Permission</param>
        void UpdatePermissionRecord(PermissionRecord permission);

        ///// <summary>
        ///// Install permissions
        ///// </summary>
        ///// <param name="permissionProvider">Permission provider</param>
        //void InstallPermissions(IPermissionProvider permissionProvider);

        ///// <summary>
        ///// Uninstall permissions
        ///// </summary>
        ///// <param name="permissionProvider">Permission provider</param>
        //void UninstallPermissions(IPermissionProvider permissionProvider);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <returns>true - authorized; otherwise, false</returns>
        bool Authorize(PermissionRecord permission);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permission">Permission record</param>
        /// <param name="user">User</param>
        /// <returns>true - authorized; otherwise, false</returns>
        bool Authorize(PermissionRecord permission, User user);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordCode">Permission record code</param>
        /// <returns>true - authorized; otherwise, false</returns>
        bool Authorize(string permissionRecordCode);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordCode">Permission record code</param>
        /// <param name="user">User</param>
        /// <returns>true - authorized; otherwise, false</returns>
        bool Authorize(string permissionRecordCode, User user);
    }
}
