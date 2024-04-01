using BackendService.Domain;
using BackendService.Helper.Security;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Infrastructure.Persistence
{
    public class ApplicationContextInitialiser
    {
        public static void SeedData(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            context.Database.Migrate();

            if (!context.MsUsers.Any())
            {
                context.MsUsers.Add(new MsUser
                {
                    Fullname = "Admin",
                    Username = "admin",
                    Password = Security.CreateHash("P@ssw0rd")
                });

                context.SaveChanges();
            }
        }
    }
}
