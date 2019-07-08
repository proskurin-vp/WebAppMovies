using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppSportsLeagueTestTask.WEB.EFModels;

namespace WebAppSportsLeagueTestTask.WEB.Models
{
    public class DirectorsViewModel
    {
        public IPagedList<DirectorViewModel> PagedDirectors { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}