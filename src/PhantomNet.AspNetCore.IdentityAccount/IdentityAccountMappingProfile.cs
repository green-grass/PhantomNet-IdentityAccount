using AutoMapper;

namespace PhantomNet.AspNetCore.IdentityAccount
{
    public class IdentityAccountMappingProfile<TAccount> : Profile
    {
        public IdentityAccountMappingProfile()
        {
            CreateMap<TAccount, IdentityAccountViewModel>();
            CreateMap<IdentityAccountViewModel, TAccount>();
            CreateMap<IdentityAccountViewModel, IdentityAccountViewModel>();
        }
    }
}
