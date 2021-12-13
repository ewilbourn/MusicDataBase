using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MusicWebsite.Models;

namespace MusicWebsite.ViewModels
{
    public class MusicGridViewModel
    {
        [Key]
        public int RecordNumber { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "FullName")]
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string FullName { get; set; }

        [Display(Name = "Piece Name")]
        public string PieceName { get; set; }

        public int? Year { get; set; }

        public int? Difficulty { get; set; }

        public MusicGridViewModel(view_MusicPieces source)
        {
            this.RecordNumber = source.RecordNumber;
            this.FirstName = source.FirstName;
            this.LastName = source.LastName;
            this.FullName = source.FullName;
            this.PieceName = source.PieceName;
            this.Year = source.Year;
            this.Difficulty = source.Difficulty;
        }

    }
}