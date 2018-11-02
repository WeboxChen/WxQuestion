using System.Collections.Generic;
using System.Web;
using Wei.Core.Infrastructure;

namespace Wei.Services.Users.External
{
    /// <summary>
    /// External authorizer helper
    /// </summary>
    public static partial class ExternalAuthorizerHelper
    {
        private static HttpSessionStateBase GetSession()
        {
            var session = EngineContext.Current.Resolve<HttpSessionStateBase>();
            return session;
        }

        //public static void StoreParametersForRoundTrip(OpenAuthenticationParameters parameters)
        //{
        //    var session = GetSession();
        //    session["nop.externalauth.parameters"] = parameters;
        //}
        //public static OpenAuthenticationParameters RetrieveParametersFromRoundTrip(bool removeOnRetrieval)
        //{
        //    var session = GetSession();
        //    var parameters = session["nop.externalauth.parameters"];
        //    if (parameters != null && removeOnRetrieval)
        //        RemoveParameters();

        //    return parameters as OpenAuthenticationParameters;
        //}

        public static void RemoveParameters()
        {
            var session = GetSession();
            session.Remove("wei.externalauth.parameters");
        }

        public static void AddErrorsToDisplay(string error)
        {
            var session = GetSession();
            var errors = session["wei.externalauth.errors"] as IList<string>;
            if (errors == null)
            {
                errors = new List<string>();
                session.Add("wei.externalauth.errors", errors);
            }
            errors.Add(error);
        }

        public static IList<string> RetrieveErrorsToDisplay(bool removeOnRetrieval)
        {
            var session = GetSession();
            var errors = session["wei.externalauth.errors"] as IList<string>;
            if (errors != null && removeOnRetrieval)
                session.Remove("wei.externalauth.errors");
            return errors;
        }
    }
}
