using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using PhantomNet.IdentityAccount.Dashboard;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityAccountDashboardViewsServiceCollectionExtensions
    {
        public static IdentityBuilder AddIdentityAccountDashboardViews(this IdentityBuilder builder)
        {
            builder.Services.Configure<RazorViewEngineOptions>(options => {
                options.FileProviders.Add(new EmbeddedFileProvider(IdentityAccountDashboardViews.Assembly, IdentityAccountDashboardViews.Namespace));
            });

            return builder;
        }
    }
}
