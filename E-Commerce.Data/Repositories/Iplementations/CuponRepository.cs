using E_Commerce.Data.Entities;
using E_Commerce.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Data.Repositories.Iplementations
{
    public class CuponRepository(AppDbContext context) : Repository<Cupon>(context), ICuponRepository
    {
    }
}
