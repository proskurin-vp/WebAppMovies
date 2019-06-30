using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppSportsLeagueTestTask.WEB.Models
{
    public class MovieViewModel
    {
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

        [DisplayName("Режиссёры")]       
        public IEnumerable<DirectorInfo> Directors { get; set; }

        [DisplayName("Постер")]
        [DataType(DataType.Upload)]        
        public HttpPostedFileBase ImageUpload { get; set; }      

    }

    public class DirectorInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }

}