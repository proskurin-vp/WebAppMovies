using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebAppSportsLeagueTestTask.WEB.Models;

namespace WebAppSportsLeagueTestTask.WEB.EFModels
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Название")]
        [Required]
        public string Name { get; set; }

        [DisplayName("Описание")]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [DisplayName("Год выпуска")]
        [Required]
        public int Year { get; set; }

        [DisplayName("Постер")]
        public string Poster { get; set; } 
        
        public int DirectorId { get; set; }

        [DisplayName("Режиссёр")]       
        public virtual Director Director { get; set; }      
        
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}