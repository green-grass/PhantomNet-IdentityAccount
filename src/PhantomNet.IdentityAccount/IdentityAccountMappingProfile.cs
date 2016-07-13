using AutoMapper;

namespace PhantomNet.IdentityAccount
{
    public class IdentityAccountMappingProfile<TAccount> : Profile
    {
        public IdentityAccountMappingProfile()
        {
            CreateMap<TAccount, IdentityAccountViewModel>();
            CreateMap<IdentityAccountViewModel, TAccount>();
        }
    }
}
