using System.Reflection;

namespace PhantomNet.IdentityAccount.Dashboard
{
    public static class IdentityAccountDashboardViews
    {
        public static Assembly Assembly => typeof(IdentityAccountDashboardViews).GetTypeInfo().Assembly;

        public static string Namespace => typeof(IdentityAccountDashboardViews).GetTypeInfo().Namespace;
    }
}
