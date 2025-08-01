﻿using ECommerce.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Repositories.Interfaces
{
    public  interface ICuponRepository : IRepository<Cupon>
    {
        Task<Cupon> GetByCodeAsync(string code);
    }
}
