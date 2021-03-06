﻿using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppSportsLeagueTestTask.WEB.EFModels;

namespace WebAppSportsLeagueTestTask.WEB.Models
{
    public class MoviesViewModel
    {
        public IPagedList<Movie> PagedMovies { get; set; }
        public ApplicationUser ApplicationUser { get; set; }       
    }
}