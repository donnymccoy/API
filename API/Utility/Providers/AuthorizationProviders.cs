using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http.Cors;

namespace API.Utility.Providers
{

    /// <summary>Custom authorization provider to implement tokenization via database-stored User/Password</summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated(); //   
        }

        /// <summary>Validate provided User/Password against database and provide a token in return</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            string userName = context.UserName;
            string passWord = context.Password;

            // TODO:  Add DB validation of User & Password //
            // If valid authentication then we can move forward //
            identity.AddClaim(new Claim("Age", "16"));

            var props = new AuthenticationProperties(new Dictionary<string, string>
                            {
                                {
                                    "userdisplayname", context.UserName
                                },
                                {
                                     "role", "admin"
                                }
                             });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(ticket);



            //using (var db = new TESTEntities())
            //{
            //    if (db != null)
            //    {
            //        var empl = db.Employees.ToList();
            //        var user = db.Users.ToList();
            //        if (user != null)
            //        {
            //            if (!string.IsNullOrEmpty(user.Where(u => u.UserName == context.UserName && u.Password == context.Password).FirstOrDefault().Name))
            //            {
            //                // If valid authentication then we can move forward //
            //                identity.AddClaim(new Claim("Age", "16"));

            //                var props = new AuthenticationProperties(new Dictionary<string, string>
            //                {
            //                    {
            //                        "userdisplayname", context.UserName
            //                    },
            //                    {
            //                         "role", "admin"
            //                    }
            //                 });

            //                var ticket = new AuthenticationTicket(identity, props);
            //                context.Validated(ticket);
            //            }
            //            else
            //            {
            //                // If invalid authentication //
            //                context.SetError("invalid_grant", "Provided username and password is incorrect");
            //                context.Rejected();
            //            }
            //        }
            //    }
            //    else
            //    {
            //        // Catch-all for DB errors //
            //        context.SetError("invalid_grant", "Provided username and password is incorrect");
            //        context.Rejected();
            //    }
            //    return;
            //}
        }
    }
}