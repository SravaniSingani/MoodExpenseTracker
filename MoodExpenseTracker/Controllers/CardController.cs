using MoodExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MoodExpenseTracker.Models.ViewModels;
using System.Web.Script.Serialization;
using System.Diagnostics;

namespace MoodExpenseTracker.Controllers
{
    public class CardController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static CardController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44307/api/");

        }

        /// <summary>
        /// Displays a list of cards in the system
        /// </summary>
        /// <returns>
        /// Returns a view for Card List
        /// </returns>
        /// <example>
        /// GET: Card/List
        /// curl: curl https://localhost:44384/api/CardData/ListCards
        /// </example>

        // GET: Card/List
        public ActionResult List()
        {
            // Objective: Access Card Data API and retrieve list of cards
            // curl https://localhost:44384/api/CardData/ListCards

            // HttpClient client = new HttpClient() { };
            string url = "CardData/ListCards";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //  Debug.WriteLine("The response is: ");
            //  Debug.WriteLine(response.StatusCode);

            IEnumerable<CardDto> cards = response.Content.ReadAsAsync<IEnumerable<CardDto>>().Result;

            // Debug.WriteLine("Number of cards: " + cards.Count());

            return View(cards);
        }

        /// <summary>
        /// Retreieves a selcted card from the list of cards
        /// </summary>
        /// <param name="id">Represents the selcted card id</param>
        /// <returns>
        /// Reeturns a view with details of the selected card
        /// </returns>
        /// <example>
        ///  GET: Card/Details/5
        ///  curl https://localhost:44384/api/CardData/FindCard/{id}
        /// </example>

        // GET: Card/Details/5
        public ActionResult Details(int id)
        {
            // Objective: Access Card Data API and find a card by its id
            // curl https://localhost:44384/api/CardData/FindCard/{id}

            DetailsCard ViewModel = new DetailsCard();

            // HttpClient client = new HttpClient() { };
            string url = "CardData/FindCard/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The response is: ");
            // Debug.WriteLine(response.StatusCode);

            CardDto SelectedCard = response.Content.ReadAsAsync<CardDto>().Result;

            // Debug.WriteLine("Expense received: " + selectedexpense.ExpenseName);

            //Showcase List of Expenses associated with the card
            url = "expensedata/listexpensesforcard/" + id;

            ViewModel.SelectedCard = SelectedCard;
            response = client.GetAsync(url).Result;
            IEnumerable<ExpenseDto> RelatedExpensestoCard = response.Content.ReadAsAsync<IEnumerable<ExpenseDto>>().Result;

            ViewModel.RelatedExpensestoCard = RelatedExpensestoCard;


            return View(ViewModel);
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

        // GET: Card/Create
        public ActionResult New()
        {
            return View();
        }



        /// <summary>
        /// Adds a card to the system
        /// </summary>
        /// <param name="card">Represenst an object of the Card</param>
        /// <returns>
        /// Returns a List with the added card data
        /// or
        /// 404: Returns an Error view displaying the error message
        /// </returns>

        // POST: Card/Create
        [HttpPost]
        public ActionResult Create(Card card)
        {
            // Objective: Add a new expense into the system using API
            // curl: -d @expense.json -H "Content-Type:application/json" https://localhost:44384/api/ExpenseData/AddExpense

            Debug.WriteLine("The expense name craeted is: ");
            Debug.WriteLine(card.CardName);

            string url = "CardData/AddCard";

            string jsonpayload = jss.Serialize(card);

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
                Debug.WriteLine("Error updating expense. Status code: " + response.StatusCode);
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine("Error message from server: " + errorMessage);
                return RedirectToAction("Error");

            }
        }



        /// <summary>
        /// Gets the selected card to update in the system
        /// </summary>
        /// <param name="id">represents the selected card id</param>
        /// <returns>
        /// Returns a view with the selected card existing details.
        /// </returns>

        // GET: Card/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "CardData/FindCard/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CardDto selectedcard = response.Content.ReadAsAsync<CardDto>().Result;
            return View(selectedcard);
        }



        // POST: Card/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Card card)
        {
            string url = "CardData/UpdateCard/" + id;
            string jsonpayload = jss.Serialize(card);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            Debug.WriteLine(jsonpayload);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                Debug.WriteLine("Error updating card. Status code: " + response.StatusCode);
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine("Error message from server: " + errorMessage);
                return RedirectToAction("Error");

            }
        }



        /// <summary>
        /// Displays a message to confirm the delete process of a card from the system
        /// </summary>
        /// <param name="id">Represents the card id</param>
        /// <returns>
        /// DeleteConfirm View 
        /// </returns>

        // GET: Card/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "CardData/FindCard/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CardDto selectedcard = response.Content.ReadAsAsync<CardDto>().Result;
            return View(selectedcard);
        }



        /// <summary>
        /// Removes a card from the system
        /// </summary>
        /// <param name="id">Represents the card id</param>
        /// <param name="collection">Represents the card object</param>
        /// <returns>
        /// Deletes the selected card and returns to the cards list
        /// </returns>

        // POST: Card/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "CardData/DeleteCard/" + id;
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
