using System;
using System.Collections.Generic;

namespace RankedTyping.Utils
{
    public abstract class PagedResultBase
    {
        public int CurrentPage { get; set; } 
        public int FirstPage { get; set; } 
        public int LastPage { get; set; } 
        public int NextPage { get; set; }
        public int PreviousPage { get; set; } 
        
        public int PageCount { get; set; } 
        public int PageSize { get; set; }
    }
 
    public class PagedResult<T> : PagedResultBase where T : class
    {
        public IList<T> Results { get; set; }
 
        public PagedResult()
        {
            Results = new List<T>();
        }
    }
}