using System.Linq;
using PhantomNet.Entities;

namespace PhantomNet.AspNetCore.IdentityAccount
{
    public class IdentityAccountSearchDescriptor<TAccount>
        : EntitySearchDescriptorBase<TAccount>,
          IEntitySearchDescriptor<TAccount>
        where TAccount : class
    {
        public override IQueryable<TAccount> Filter(IQueryable<TAccount> entities)
        {
            return entities;
        }

        public override IQueryable<TAccount> DefaultSort(IQueryable<TAccount> entities)
        {
            return entities;
        }
    }
}
