using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MoodExpenseTracker.Models
{
    public class Expense
    {
        //What describes an expense

        [Key]
        public int ExpenseId { get; set; }
        public string ExpenseName { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; }

        //An expense has a Card Id
        //Each card has multiple expenses

        [ForeignKey("Card")]
        public int CardId { get; set; }
        public virtual Card Card { get; set; }

        //An expense has a Category Id
        //Each Category has multiple expenses

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        //An expense has a Mood Id
        //Each Mood has multiple expenses
        [ForeignKey("Mood")]
        public int MoodId { get; set; }
        public virtual Mood Mood { get; set; }

        //An expense has a Weather Id
        //Each Weather has multiple expenses
        [ForeignKey("Weather")]
        public int WeatherId { get; set; }
        public virtual Weather Weather { get; set; }
    }

    public class ExpenseDto
    {
        public int ExpenseId { get; set; }
        public string ExpenseName { get; set; }
        public decimal Amount { get; set; }
        public DateTime ExpenseDate { get; set; }
        public int CardId { get; set; }
        public string CardName { get; set; }
        public string CardType { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int MoodId { get; set; }
        public string MoodName { get; set; }
        public int WeatherId { get; set; }
        public string WeatherName { get; set; }
    }
}