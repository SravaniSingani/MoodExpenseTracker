using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoodExpenseTracker.Models.ViewModels
{
    public class DetailsCategory
    {
        public CategoryDto SelectedCategory { get; set; }
        public IEnumerable<ExpenseDto> RelatedExpensestoCategory { get; set; }
    }
}