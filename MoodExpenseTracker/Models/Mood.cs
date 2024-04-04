using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MoodExpenseTracker.Models
{
    public class Mood
    {
        [Key]
        public int MoodId { get; set; }
        public string MoodName { get; set;}

        // A mood has multiple expenses
        public ICollection<Expense> Expenses { get; set; }
    }

    public class MoodDto
    {
        public int MoodId { get; set; }
        public string MoodName { get; set;}
    }


}