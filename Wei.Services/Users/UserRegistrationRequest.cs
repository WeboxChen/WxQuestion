using Wei.Core.Domain.Users;

namespace Wei.Services.Users
{
    /// <summary>
    /// Customer registration request
    /// </summary>
    public class UserRegistrationRequest
    {

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="email">Email</param>
        /// <param name="loginname">loginname</param>
        /// <param name="password">Password</param>
        public UserRegistrationRequest(User user, string email, string loginname, string password)
        {
            this.User = user;
            this.Email = email;
            this.LoginName = loginname;
            this.Password = password;
        }

        /// <summary>
        /// Customer
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// LoginName
        /// </summary>
        public string LoginName { get; set; }
        /// <summary>
        /// Password
        /// </summary>
        public string Password { get; set; }
    }
}
