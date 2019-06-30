using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppSportsLeagueTestTask.WEB.Models
{
    public class PageViewModel
    {
        public IEnumerable<Movie> Movies { get; set; }
        public PageInfo PageInfo { get; set; }       
    }
}