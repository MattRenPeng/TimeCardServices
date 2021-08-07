using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TimeCardServices.Model
{
    public class TimeCard
    {
        public string UserName { get; set; }
        public DateTime WeekStart { get; set; }

        public decimal? Day1 { get; set; }
        public decimal? Day2 { get; set; }
        public decimal? Day3 { get; set; }
        public decimal? Day4 { get; set; }
        public decimal? Day5 { get; set; }
        public decimal? Day6 { get; set; }
        public decimal? Day7 { get; set; }

        public string Notes { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }

    }
}
