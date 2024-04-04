using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MoodExpenseTracker.Models;

namespace MoodExpenseTracker.Controllers
{
    public class ExpenseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// A list that returns all the Expenses in the system
        /// </summary>
        /// <returns>
        /// All expenses in the system 
        /// </returns>
        ///<example>
        /// GET: api/ExpenseData/ListExpenses
        /// CURL: curl https://localhost:44307/api/expensedata/listexpenses
        /// </example>

        // List Expenses
        [HttpGet]
        [Route("api/ExpenseData/ListExpenses")]
        public IEnumerable<ExpenseDto> ListExpenses()
        {
            List<Expense> Expenses = db.Expenses.ToList();
            List<ExpenseDto> ExpenseDtos = new List<ExpenseDto>();

            Expenses.ForEach(e => ExpenseDtos.Add(new ExpenseDto()
            {

                ExpenseId = e.ExpenseId,
                ExpenseName = e.ExpenseName,
                Amount = e.Amount,
                ExpenseDate = e.ExpenseDate,
                CardId = e.CardId,
                CardName = e.Card.CardName,
                CardType = e.Card.CardType,
                CategoryId = e.CategoryId,
                CategoryName = e.Category.CategoryName,
                MoodId = e.MoodId,
                MoodName = e.Mood.MoodName,
                WeatherId = e.WeatherId,
                WeatherName = e.Weather.WeatherName
               

            }));
            return ExpenseDtos;
        }


        /// <summary>
        /// Returns a selected expense in the system
        /// </summary>
        /// <param name="id">A selected expense id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: An expense in the system that matches the ExpenseId
        /// </returns>
        /// <example>
        /// GET: api/ExpenseData/FindExpense/2
        /// CURL: curl https://localhost:44307/api/ExpenseData/FindExpense/2
        /// </example>

        // Find an Expense


        [ResponseType(typeof(Expense))]
        [HttpGet]
        [Route("api/ExpenseData/FindExpense/{id}")]

        public IHttpActionResult FindExpense(int id)
        {
            Expense Expense = db.Expenses.Find(id);
            ExpenseDto ExpenseDto = new ExpenseDto()
            {
                ExpenseId = Expense.ExpenseId,
                ExpenseName = Expense.ExpenseName,
                Amount = Expense.Amount,
                ExpenseDate = Expense.ExpenseDate,
                CardId = Expense.Card.CardId,
                CardName = Expense.Card.CardName,
                CardType = Expense.Card.CardType,
                CategoryId = Expense.CategoryId,
                CategoryName = Expense.Category.CategoryName,
                MoodId = Expense.Mood.MoodId,
                MoodName = Expense.Mood.MoodName,
                WeatherId = Expense.Weather.WeatherId,
                WeatherName = Expense.Weather.WeatherName

            };
            if (Expense == null)
            {
                return NotFound();
            }
            return Ok(ExpenseDto);
        }

        /// <summary>
        /// Updates a particular expense in the system with Post data input
        /// </summary>
        /// <param name="id">Represents the expense id (primary key)</param>
        /// <param name="expense">JSON form data of an expense</param>
        /// <returns>
        /// HEADER: 204 (Success, No Conetent Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ExpenseData/UpdateExpense/5
        /// FORM Data: Expense JSON Object
        /// CURL: curl -d @expense.json -H "Content-Type:application/json" https://localhost:44307/api/ExpenseData/UpdateExpense/3
        /// </example>


        // PUT: api/ExpenseData/5
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateExpense(int id, Expense expense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != expense.ExpenseId)
            {
                return BadRequest();
            }

            db.Entry(expense).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Add an expense to the system with Post data input
        /// </summary>
        /// <param name="expense">JSON form data of an expense</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Expense Id, Expense Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/ExpenseData/AddExpense
        /// FORM Data: Expense JSON Object
        /// CURL: curl -d @expense.json -H "Content-Type:application/json" https://localhost:44307/api/ExpenseData/AddExpense
        /// </example>

        [ResponseType(typeof(Expense))]
        [HttpPost]
        public IHttpActionResult AddExpense(Expense expense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Expenses.Add(expense);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = expense.ExpenseId }, expense);
        }


        /// <summary>
        /// Deletes an expense from the system by it's ID
        /// </summary>
        /// <param name="id">Represents the expense id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/ExpenseData/DeleteExpense/8
        /// CURL: curl -d "" https://localhost:44307/api/ExpenseData/DeleteExpense/12
        /// </example>

        // Delete an Expense

        [ResponseType(typeof(Expense))]
        [HttpPost]
        public IHttpActionResult DeleteExpense(int id)
        {
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }

            db.Expenses.Remove(expense);
            db.SaveChanges();

            return Ok(expense);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ExpenseExists(int id)
        {
            return db.Expenses.Count(e => e.ExpenseId == id) > 0;
        }
    }
}