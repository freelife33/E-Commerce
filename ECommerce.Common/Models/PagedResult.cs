﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Common.Models
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int TotalCount { get; set; }     
        public int PageNumber { get; set; }     
        public int PageSize { get; set; }       

        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
