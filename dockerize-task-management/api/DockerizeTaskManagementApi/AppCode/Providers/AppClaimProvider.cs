using DockerizeTaskManagementApi.AppCode.Extensions;
using DockerizeTaskManagementApi.Models.DataContexts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DockerizeTaskManagementApi.AppCode.Providers
{
    public class AppClaimProvider : IClaimsTransformation
    {
        readonly TaskManagementDbContext db;
        public AppClaimProvider(TaskManagementDbContext db)
        {
            this.db = db;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity is ClaimsIdentity claimIdentity
                && claimIdentity.IsAuthenticated)
            {

                int? currentUserId = principal.GetPrincipalId();

                var roleClaim = claimIdentity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role));
                while (roleClaim != null)
                {
                    claimIdentity.RemoveClaim(roleClaim);
                    roleClaim = claimIdentity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role));
                }

                var _role = claimIdentity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role));
                while (_role != null)
                {
                    claimIdentity.RemoveClaim(_role);
                    _role = claimIdentity.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Role));
                }

                var roles = await (from ur in db.UserRoles
                                   join r in db.Roles on ur.RoleId equals r.Id
                                   where ur.UserId == currentUserId
                                   select r.Name).ToArrayAsync();

                foreach (var role in roles)
                {
                    claimIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
                }


                var currentUser = await db.Users.FirstOrDefaultAsync(u => u.Id == currentUserId);

                if (!string.IsNullOrWhiteSpace(currentUser.Name) || !string.IsNullOrWhiteSpace(currentUser.Surname))
                {
                    claimIdentity.AddClaim(new Claim("FullName", $"{currentUser.Name} {currentUser.Surname}"));
                }
                else if (!string.IsNullOrWhiteSpace(currentUser.PhoneNumber))
                {
                    claimIdentity.AddClaim(new Claim("FullName", $"{currentUser.PhoneNumber}"));
                }
                else
                {
                    claimIdentity.AddClaim(new Claim("FullName", $"{currentUser.Email}"));
                }
            }


            return principal;
        }
    }
}
