using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PointOfSales.Core.Entities;

namespace PointOfSales.Core.AuthorizeSetup
{
    /// <summary>
    /// This version provides:
    /// - Adds Permissions to the user's claims.
    /// </summary>
    // Thanks to https://korzh.com/blogs/net-tricks/aspnet-identity-store-user-data-in-claims
    public class AddPermissionsToUserClaims : UserClaimsPrincipalFactory<ApplicationUser>
    {
        private readonly POSContext _context;

        public AddPermissionsToUserClaims(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> optionsAccessor,
            POSContext context)
            : base(userManager, optionsAccessor)
        {
           _context = context;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            // var userId = identity.Claims.GetUserIdFromClaims();
            // var rtoPCalcer = new CalcAllowedPermissions(_extraAuthDbContext);
            // identity.AddClaim(new Claim(PermissionConstants.PackedPermissionClaimType, await rtoPCalcer.CalcPermissionsForUserAsync(userId)));
            return identity;
        }
    }
}