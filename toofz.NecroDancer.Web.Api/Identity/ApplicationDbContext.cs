using System;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace toofz.NecroDancer.Web.Api.Identity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(null);
        }

        public static ApplicationDbContext Create() => new ApplicationDbContext();

        static string GetConnectionString()
        {
            return Environment.GetEnvironmentVariable("toofzApiConnectionString", EnvironmentVariableTarget.Machine);
        }

        public ApplicationDbContext() : base(GetConnectionString(), throwIfV1Schema: false) { }
    }
}