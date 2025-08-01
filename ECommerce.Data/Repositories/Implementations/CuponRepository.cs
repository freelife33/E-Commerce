﻿using ECommerce.Data.Entities;
using ECommerce.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Data.Repositories.Implementations
{
    public class CuponRepository(AppDbContext context) : Repository<Cupon>(context), ICuponRepository
    {
        public async Task<Cupon> GetByCodeAsync(string code)
        {
            return await _context.Cupons.FirstOrDefaultAsync(c => c.Code == code);
        }
    }
}
