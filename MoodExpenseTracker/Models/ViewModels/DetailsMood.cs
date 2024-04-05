using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoodExpenseTracker.Models.ViewModels
{
    public class DetailsMood
    {
        public MoodDto SelectedMood { get; set; }
        public IEnumerable<ExpenseDto> RelatedExpensestoMood { get; set; }
    }
}