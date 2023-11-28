using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MyBudgetPlanner.DataBase
{
    public static class ModelBuilderExtention
    {
        public static void SeedRoles(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                    new IdentityRole() { Id = "da576a70-2276-4872-a496-6765a07534e6", ConcurrencyStamp = "da576a70-2276-4872-a496-6765a07534e6", Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
                    new IdentityRole() { Id = "44730987-28b6-4c76-8411-30d9429cb2a1", ConcurrencyStamp = "44730987-28b6-4c76-8411-30d9429cb2a1", Name = "User", NormalizedName = "USER" }
                //                    new IdentityRole() { Id = "f8f904ad-8f56-453b-b96f-d0478badcea2", ConcurrencyStamp = "f8f904ad-8f56-453b-b96f-d0478badcea2", Name = "ServiceProvider", NormalizedName = "SERVICEPROVIDER" }
                );
        }
    }
}
