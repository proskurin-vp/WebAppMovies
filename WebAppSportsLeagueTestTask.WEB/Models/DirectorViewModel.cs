using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebAppSportsLeagueTestTask.WEB.Models
{
    public class DirectorViewModel
    {       
        public int Id { get; set; }

        [DisplayName("Режиссёр")]
        [Required]       
        public string FullName { get; set; }

        public int CountMovies { get; set; }

        public List<Movie> Movies { get; set; }
    }
       
}