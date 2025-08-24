using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Repositories.Implementations
{
    public class SocialLinkRepository : Repository<SocialLink>, ISocialLinkRepository
    {
        public SocialLinkRepository(AppDbContext context) : base(context)
        {
        }
    }
}
