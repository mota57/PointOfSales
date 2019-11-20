using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PointOfSales.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSales.Core.Extensions
{
    public static class IdentityExtensions
    {

        
        /// <summary>
        /// Signs in the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to sign-in.</param>
        /// <param name="authenticationProperties">Properties applied to the login and authentication cookie.</param>
        /// <param name="additionalClaims">Additional claims that will be stored in the cookie.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task SignInWithClaimsAsync(this SignInManager<ApplicationUser> signIn,ApplicationUser user, AuthenticationProperties authenticationProperties, IEnumerable<Claim> additionalClaims)
        {
            var userPrincipal =  await signIn.ClaimsFactory.CreateAsync(user);
            userPrincipal.Identities.First().AddClaims(additionalClaims);

            var logger = ((ILogger<SignInManager<ApplicationUser>>)signIn.Logger);
            
            logger?.LogDebug($"@@@ ApplicationScheme at SigninWithClaimsAsync:: {IdentityConstants.ApplicationScheme}");

            if (additionalClaims != null)
            {
                var claimsInfo = additionalClaims?.Select(_ => $"Type::{_.Type}, value:: {_.Value}\n").ToArray();
                logger?.LogDebug($"@@@ claims = {string.Join(',', claimsInfo)}");
                
            }


           await signIn.Context.SignInAsync(IdentityConstants.ApplicationScheme,
                userPrincipal,
                authenticationProperties ?? new AuthenticationProperties());
        }

    }
}
