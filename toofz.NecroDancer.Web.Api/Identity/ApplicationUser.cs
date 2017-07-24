using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace toofz.NecroDancer.Web.Api.Identity
{
    public sealed class ApplicationUser : IdentityUser
    {
        public Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            return manager.CreateIdentityAsync(this, authenticationType);
        }
    }
}