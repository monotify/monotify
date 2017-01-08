using System;
using System.Collections.Generic;

namespace Monotify.Models.Base
{
    public class MonoPagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int PageCount => (int)Math.Ceiling((double)ItemCount / PageSize);
        public int ItemCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; } = 20;
    }
}