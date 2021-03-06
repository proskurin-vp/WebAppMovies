﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppSportsLeagueTestTask.WEB.CustomAttributes;

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
        [Range(1900, 2050)]
        public int Year { get; set; }

        [DisplayName("Режиссёры")]       
        public IEnumerable<DirectorInfo> Directors { get; set; }

        [DisplayName("Постер")]       
        [DataType(DataType.Upload)]
        [ValidateFile(ErrorMessage = "Пожалуйста, выберете постер (*.jpg, *.png, *.gif) размером меньше 1Mb")]       
        public HttpPostedFileBase ImageUpload { get; set; }   
        
        public int CurrentPageNumber { get; set; }

        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

    }

    public class DirectorInfo
    {
        public int Id { get; set; }
        public string FullName { get; set; }
    }

}