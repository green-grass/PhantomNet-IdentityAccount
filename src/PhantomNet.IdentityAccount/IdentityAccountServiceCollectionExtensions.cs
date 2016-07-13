using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PhantomNet.IdentityAccount;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityAccountServiceCollectionExtensions
    {
        public static IdentityBuilder AddIdentityAccount<TAccount, TContext>(this IServiceCollection services)
            where TAccount : IdentityUser, new()
            where TContext : DbContext
        {
            services.TryAddScoped<IdentityAccountManager<TAccount>>();

            services.AddScoped<IdentityAccountErrorDescriber>();

            Mapper.Initialize(cfg => cfg.AddProfile<IdentityAccountMappingProfile<TAccount>>());

            return services.AddIdentity<TAccount, IdentityRole>()
                           .AddEntityFrameworkStores<TContext>()
                           .AddDefaultTokenProviders();
        }
    }
}
