using MoodExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MoodExpenseTracker.Controllers
{
    public class ExpenseController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static ExpenseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44307/api/ExpenseData/");

        }

        /// <summary>
        /// Displays a list of expenses in the system
        /// </summary>
        /// <returns>
        /// Returns a view for Expense List
        /// </returns>
        /// <example>
        /// GET: Expense/List
        /// curl: curl https://localhost:44307/api/ExpenseData/ListExpenses
        /// </example>

        public ActionResult List()
        {
            // Objective: Access Expense Data API and retrieve list of expenses

            // HttpClient client = new HttpClient() { };
            string url = "ListExpenses";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //  Debug.WriteLine("The response is: ");
            //  Debug.WriteLine(response.StatusCode);

            IEnumerable<ExpenseDto> expenses = response.Content.ReadAsAsync<IEnumerable<ExpenseDto>>().Result;

            // Debug.WriteLine("Number of expenses: " + expenses.Count());

            return View(expenses);
        }


        /// <summary>
        /// Retreieves a selcted expense from the list of expenses
        /// </summary>
        /// <param name="id">Represents the selcted expense id</param>
        /// <returns>
        /// Reeturns a view with details of the selected expense
        /// </returns>
        /// <example>
        ///  GET: Expense/Details/5
        ///  curl https://localhost:44307/api/ExpenseData/FindExpense/{id}
        /// </example>

        public ActionResult Details(int id)
        {

            // Objective: Access Expense Data API and find an expense by its id


            // HttpClient client = new HttpClient() { };
            string url = "FindExpense/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The response is: ");
            // Debug.WriteLine(response.StatusCode);

            ExpenseDto selectedexpense = response.Content.ReadAsAsync<ExpenseDto>().Result;

            // Debug.WriteLine("Expense received: " + selectedexpense.ExpenseName);
            return View(selectedexpense);
        }



        /// <summary>
        /// Displays a message when it catches an error 
        /// </summary>
        /// <returns>
        /// Returns a view of Error Page
        /// </returns>
        public ActionResult Error()
        {
            return View();
        }



        // GET: Expense/Create
        public ActionResult New()
        {
            var cardList = ListofCards();
            var categoryList = ListofCategories();
            var moodList = ListofMoods();
            var weatherList = ListofWeathers();

            ViewBag.Card = new SelectList(cardList, "CardId", "CardName");
            ViewBag.Category = new SelectList(categoryList, "CategoryId", "CategoryName");
            ViewBag.Mood = new SelectList(moodList, "MoodId", "MoodName");
            ViewBag.Weather = new SelectList(weatherList, "WeatherId", "WeatherName");

            return View();
        }



        /// <summary>
        /// Adds an expense to the system
        /// </summary>
        /// <param name="expense">Represenst an object of the Expense</param>
        /// <returns>
        /// Returns a List with the added expense data
        /// or
        /// 404: Returns an Error view displaying the error message
        /// </returns>

        // POST: Expense/Create
        [HttpPost]
        public ActionResult Create(Expense expense)
        {
            // Objective: Add a new expense into the system using API
            // curl: -d @expense.json -H "Content-Type:application/json" https://localhost:44384/api/ExpenseData/AddExpense

            Debug.WriteLine("The expense name craeted is: ");
            Debug.WriteLine(expense.ExpenseName);

            string url = "AddExpense";

            string jsonpayload = jss.Serialize(expense);

            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        private IEnumerable<CardDto> ListofCards()
        {
            string url = "https://localhost:44307/api/CardData/ListCards";
            HttpResponseMessage response = client.GetAsync(url).Result;
            return response.Content.ReadAsAsync<IEnumerable<CardDto>>().Result;
        }

        private IEnumerable<CategoryDto> ListofCategories()
        {
            string url = "https://localhost:44307/api/CategoryData/ListCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;
            return response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;
        }

        private IEnumerable<MoodDto> ListofMoods()
        {
            string url = "https://localhost:44307/api/MoodData/ListMoods";
            HttpResponseMessage response = client.GetAsync(url).Result;
            return response.Content.ReadAsAsync<IEnumerable<MoodDto>>().Result;
        }

        private IEnumerable<WeatherDto> ListofWeathers()
        {
            string url = "https://localhost:44307/api/WeatherData/ListWeathers";
            HttpResponseMessage response = client.GetAsync(url).Result;
            return response.Content.ReadAsAsync<IEnumerable<WeatherDto>>().Result;
        }




        /// <summary>
        /// Gets the selected expense to update in the system
        /// </summary>
        /// <param name="id">represents the selected expense id</param>
        /// <returns>
        /// Returns a view with the selected expense existing details.
        /// </returns>

        // GET: Expense/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "FindExpense/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ExpenseDto selectedexpense = response.Content.ReadAsAsync<ExpenseDto>().Result;


            //List of cards and categories
            var cardList = ListofCards();
            var categoryList = ListofCategories();
            var moodList = ListofMoods();
            var weatherList = ListofWeathers();

            var cardListItems = cardList.Select(c => new SelectListItem
            {
                Value = c.CardId.ToString(),
                Text = c.CardName
            });

            var categoryListItems = categoryList.Select(ct => new SelectListItem
            {
                Value = ct.CategoryId.ToString(),
                Text = ct.CategoryName
            });

            var moodListItems = moodList.Select(m => new SelectListItem
            {
                Value = m.MoodId.ToString(),
                Text = m.MoodName
            });

            var weatherListItems = weatherList.Select(w => new SelectListItem
            {
                Value = w.WeatherId.ToString(),
                Text = w.WeatherName
            });

            ViewBag.Cards = new SelectList(cardListItems, "Value", "Text", selectedexpense.CardId);
            ViewBag.Categories = new SelectList(categoryListItems, "Value", "Text", selectedexpense.CategoryId);
            ViewBag.Moods = new SelectList(moodListItems, "Value", "Text", selectedexpense.MoodId);
            ViewBag.Weathers = new SelectList(weatherListItems, "Value", "Text", selectedexpense.WeatherId);

            return View(selectedexpense);
        }

        /// <summary>
        /// Updates an existing expense in the system
        /// </summary>
        /// <param name="id">Represents the selected expense id</param>
        /// <param name="expense"> represents the expense object</param>
        /// <returns>
        /// Returns to a view to the details of the selected expense
        /// </returns>

        // POST: Expense/Update/5
        [HttpPost]
        public ActionResult Update(int id, Expense expense)
        {
            string url = "UpdateExpense/" + id;
            string jsonpayload = jss.Serialize(expense);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            Debug.WriteLine(jsonpayload);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details/" + id);
            }
            else
            {
                Debug.WriteLine("Error updating expense. Status code: " + response.StatusCode);
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine("Error message from server: " + errorMessage);
                return RedirectToAction("Error");

            }
        }



        /// <summary>
        /// Displays a message to confirm the delete process of an expense from the system
        /// </summary>
        /// <param name="id">Represents the expense id</param>
        /// <returns>
        /// DeleteConfirm View 
        /// </returns>

        // GET: Expense/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "FindExpense/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            ExpenseDto selectedexpense = response.Content.ReadAsAsync<ExpenseDto>().Result;
            return View(selectedexpense);
        }


        /// <summary>
        /// Removes an expense from the system
        /// </summary>
        /// <param name="id">Represents the expense id</param>
        /// <param name="collection">Represents the expense object</param>
        /// <returns>
        /// Deletes the selected expens and returns to the expense list
        /// </returns>
        // POST: Expense/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "DeleteExpense/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }



    }
}
