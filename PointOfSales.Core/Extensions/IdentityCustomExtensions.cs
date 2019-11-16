using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
            var identity = userPrincipal.Identities.First();
            foreach (var claim in additionalClaims)
            {
                identity.AddClaim(claim);
            }
            await signIn.Context.SignInAsync(IdentityConstants.ApplicationScheme,
                userPrincipal,
                authenticationProperties ?? new AuthenticationProperties());
        }

    }
}
