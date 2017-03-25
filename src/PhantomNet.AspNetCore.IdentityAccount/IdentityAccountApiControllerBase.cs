using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using PhantomNet.AspNetCore.IdentityAccount.Resources;
using PhantomNet.Entities.Mvc;

namespace PhantomNet.AspNetCore.IdentityAccount
{
    [Authorize]
    [Route("api/accounts")]
    public abstract class IdentityAccountApiControllerBase<TAccount>
        : EntityApiControllerBase<IdentityAccountViewModel, IdentityAccountViewModel, IdentityAccountSearchDescriptor<IdentityAccountViewModel>, IdentityAccountManager<TAccount>>
        where TAccount : IdentityUser, new()
    {
        public IdentityAccountApiControllerBase(
            IdentityAccountManager<TAccount> manager,
            IStringLocalizer<IdentityAccountApiControllerResources> localizer,
            IdentityAccountErrorDescriber errorDescriber)
            : base(manager, localizer)
        {
            ErrorDescriber = errorDescriber;
        }

        private IdentityAccountErrorDescriber ErrorDescriber { get; set; }

        protected override GenericError DescribeModelNotFoundError(IdentityAccountViewModel viewModel)
        {
            return ErrorDescriber.AccountNotFound(viewModel.Email);
        }
    }
}
