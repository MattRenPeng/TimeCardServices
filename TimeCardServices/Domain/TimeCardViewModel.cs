using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeCardServices.Domain
{
    public class TimeCardViewModel
    {
        public string UserName { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Week start is required")]
        public DateTime WeekStart { get; set; }

        [Range(0, 24, ErrorMessage = "The cost must be smaller than 24 hours")]
        public decimal? Day1 { get; set; }
        [Range(0, 24, ErrorMessage = "The cost must be smaller than 24 hours")]
        public decimal? Day2 { get; set; }
        [Range(0, 24, ErrorMessage = "The cost must be smaller than 24 hours")]
        public decimal? Day3 { get; set; }
        [Range(0, 24, ErrorMessage = "The cost must be smaller than 24 hours")]
        public decimal? Day4 { get; set; }
        [Range(0, 24, ErrorMessage = "The cost must be smaller than 24 hours")]
        public decimal? Day5 { get; set; }
        [Range(0, 24, ErrorMessage = "The cost must be smaller than 24 hours")]
        public decimal? Day6 { get; set; }
        [Range(0, 24, ErrorMessage = "The cost must be smaller than 24 hours")]
        public decimal? Day7 { get; set; }
        [StringLength(300,ErrorMessage ="The lenght of notes should be smaller than 300  ")]
        public string Notes { get; set; }
        public decimal? TotalHors { get { return (Day1 ?? 0) + (Day2 ?? 0) + (Day3 ?? 0) + (Day4 ?? 0) + (Day5 ?? 0) + (Day6 ?? 0) + (Day7 ?? 0); } }

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
    }
}
