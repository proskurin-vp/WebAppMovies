using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebAppSportsLeagueTestTask.WEB.EFModels
{
    public class Director
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Режиссёр")]
        [Required]
        [Index(IsUnique = true)]
        [StringLength(200)]
        public string FullName { get; set; }       

        public virtual ICollection<Movie> Movies { get; set; }
    }
}