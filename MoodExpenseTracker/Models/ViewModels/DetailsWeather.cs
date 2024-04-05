using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoodExpenseTracker.Models.ViewModels
{
    public class DetailsWeather
    {
        public WeatherDto SelectedWeather { get; set; }
        public IEnumerable<ExpenseDto> RelatedExpensestoWeather { get; set; }
    }
}