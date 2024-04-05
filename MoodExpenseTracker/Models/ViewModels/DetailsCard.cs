using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MoodExpenseTracker.Models.ViewModels
{
    public class DetailsCard
    {
        public CardDto SelectedCard { get; set; }
        public IEnumerable<ExpenseDto> RelatedExpensestoCard { get; set; }
    }
}