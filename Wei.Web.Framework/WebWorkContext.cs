using System;
using System.Web;
using System.Collections.Generic;
using Wei.Core;
using Wei.Core.Domain.Localization;
using Wei.Core.Domain.Users;
using Wei.Services.Users;
using System.Data;

namespace Wei.Web.Framework
{
    /// <summary>
    /// Work context for web application
    /// </summary>
    public partial class WebWorkContext : IWorkContext
    {
        #region Const

        private const string UserCookieName = "WEI.user";

        #endregion

        #region Fields

        private readonly HttpContextBase _httpContext;
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;
        private readonly DBHelp _dbhelp;
        //private readonly ILanguageService _languageService;
        
        public readonly IWebHelper _webHelper;
        
        private User _cachedUser;
        private Language _cachedLanguage;
        private IDictionary<string, IDictionary<string, string>> _colDescription;

        #endregion

        #region Ctor

        public WebWorkContext(HttpContextBase httpContext,
            IUserService userService,
            IAuthenticationService authenticationService,
            DBHelp dbhelp,
            //ILanguageService languageService,
            IWebHelper webHelper)
        {
            this._httpContext = httpContext;
            this._userService = userService;
            this._authenticationService = authenticationService;
            this._dbhelp = dbhelp;
            //this._languageService = languageService;
            this._webHelper = webHelper;
        }

        #endregion

        #region Utilities

        protected virtual HttpCookie GetUserCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;

            return _httpContext.Request.Cookies[UserCookieName];
        }

        protected virtual void SetUserCookie(string userGuid)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(UserCookieName);
                cookie.HttpOnly = true;
                cookie.Value = userGuid.ToString();
                if (string.IsNullOrEmpty(userGuid))
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 365; //TODO make configurable
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(UserCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }

        //protected virtual Language GetLanguageFromUrl()
        //{
        //    if (_httpContext == null || _httpContext.Request == null)
        //        return null;

        //    string virtualPath = _httpContext.Request.AppRelativeCurrentExecutionFilePath;
        //    string applicationPath = _httpContext.Request.ApplicationPath;
        //    if (!virtualPath.IsLocalizedUrl(applicationPath, false))
        //        return null;

        //    var seoCode = virtualPath.GetLanguageSeoCodeFromUrl(applicationPath, false);
        //    if (String.IsNullOrEmpty(seoCode))
        //        return null;

        //    var language = _languageService
        //        .GetAllLanguages()
        //        .FirstOrDefault(l => seoCode.Equals(l.UniqueSeoCode, StringComparison.InvariantCultureIgnoreCase));
        //    if (language != null && language.Published && _storeMappingService.Authorize(language))
        //    {
        //        return language;
        //    }

        //    return null;
        //}

        //protected virtual Language GetLanguageFromBrowserSettings()
        //{
        //    if (_httpContext == null ||
        //        _httpContext.Request == null ||
        //        _httpContext.Request.UserLanguages == null)
        //        return null;

        //    var userLanguage = _httpContext.Request.UserLanguages.FirstOrDefault();
        //    if (String.IsNullOrEmpty(userLanguage))
        //        return null;

        //    var language = _languageService
        //        .GetAllLanguages()
        //        .FirstOrDefault(l => userLanguage.Equals(l.LanguageCulture, StringComparison.InvariantCultureIgnoreCase));
        //    if (language != null && language.Published && _storeMappingService.Authorize(language))
        //    {
        //        return language;
        //    }

        //    return null;
        //}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public virtual User CurrentUser
        {
            get
            {
                if (_cachedUser != null)
                    return _cachedUser;

                User user = _authenticationService.GetAuthenticatedUser();
                
                
                //validation
                if (user != null && user.Status >= 0)
                {
                    SetUserCookie(user.GId);
                    _cachedUser = user;
                }

                return _cachedUser;
            }
            set
            {
                SetUserCookie(value.GId);
                _cachedUser = value;
            }
        }

        /// <summary>
        /// Get or set current user working language
        /// </summary>
        public virtual Language WorkingLanguage
        {
            get
            {
                if (_cachedLanguage != null)
                    return _cachedLanguage;

                //Language detectedLanguage = null;

                //var allLanguages = _languageService.GetAllLanguages();

                //var language = allLanguages.FirstOrDefault();
                //if (language == null)
                //{
                //    //it not specified, then return the first found one
                //    language = _languageService.GetAllLanguages().FirstOrDefault();
                //}
                ////cache
                //_cachedLanguage = language;
                return _cachedLanguage;
            }
            set
            {
                var languageId = value != null ? value.Id : 0;

                //reset cache
                _cachedLanguage = null;
            }
        }

        /// <summary>
        /// Get or set value indicating whether we're in admin area
        /// </summary>
        public virtual bool IsAdmin { get; set; }


        public IDictionary<string, IDictionary<string, string>> ColDescription
        {
            get
            {
                if (_colDescription == null)
                {
                    _colDescription = new Dictionary<string, IDictionary<string, string>>();
                    string sql = @"select sysobj.name 'tname', syscol.name 'colname',[value] 
from sys.extended_properties extend join sysobjects sysobj on extend.major_id = sysobj.id
  join syscolumns syscol on extend.minor_id = syscol.colid and extend.major_id = syscol.id
where extend.name = 'MS_Description' and value<>''";
                    var set = _dbhelp.Select(sql, System.Data.CommandType.Text);
                    if (set == null || set.Tables.Count == 0)
                        throw new Exception("数据库出错");
                    var table = set.Tables[0];
                    foreach (DataRow row in table.Rows)
                    {
                        string tname = row["tname"].ToString();
                        string colname = row["colname"].ToString();
                        string description = row["value"].ToString();
                        if (!_colDescription.ContainsKey(tname))
                            _colDescription.Add(tname, new Dictionary<string, string>());
                        _colDescription[tname].Add(description, colname);
                    }
                }
                return _colDescription;
            }
        }
        #endregion
    }
}
