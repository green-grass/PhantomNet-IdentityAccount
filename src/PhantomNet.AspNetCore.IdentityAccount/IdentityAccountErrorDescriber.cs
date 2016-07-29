using Microsoft.Extensions.Localization;

namespace PhantomNet.AspNetCore.IdentityAccount
{
    public class IdentityAccountErrorDescriber
    {
        private readonly IStringLocalizer<IdentityAccountErrorDescriberResources> _localizer;

        public IdentityAccountErrorDescriber(IStringLocalizer<IdentityAccountErrorDescriberResources> localizer)
        {
            _localizer = localizer;
        }

        public virtual GenericError AccountNotFound(string accountEmail)
        {
            return new GenericError {
                Code = nameof(AccountNotFound),
                Description = _localizer[nameof(AccountNotFound), accountEmail]
            };
        }
    }
}