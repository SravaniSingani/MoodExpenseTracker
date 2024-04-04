using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoodExpenseTracker.Models
{
    public class Weather
    {
        [Key]
        public int WeatherId { get; set; }
        public string WeatherName { get; set; }

        // A weather has multiple expenses
        public ICollection<Expense> Expenses { get; set; }
    }

    public class WeatherDto
    {
        public int WeatherId { get; set; }
        public string WeatherName { get; set; }
    }
}