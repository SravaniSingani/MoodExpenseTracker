using MoodExpenseTracker.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using MoodExpenseTracker.Models.ViewModels;
using System.Web.Script.Serialization;

namespace MoodExpenseTracker.Controllers
{
    public class CardController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static CardController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                // Cookies are manually set in RequestHeader
                UseCookies = false
            };

            client = new HttpClient(handler);
            client.BaseAddress = new Uri("https://localhost:44307/api/");
        }

        private void GetApplicationCookie()
        {
            string token = "";
            // HTTP client is set up to be reused, otherwise, it will exhaust server resources.
            // This is a bit dangerous because a previously authenticated cookie could be cached for
            // a follow-up request from someone else. Reset cookies in HTTP client before grabbing a new one.
            client.DefaultRequestHeaders.Remove("Cookie");
            if (!User.Identity.IsAuthenticated) return;

            HttpCookie cookie = HttpContext.Request.Cookies.Get(".AspNet.ApplicationCookie");
            if (cookie != null) token = cookie.Value;

            // Collect token as it is submitted to the controller
            // Use it to pass along to the WebAPI.
            Debug.WriteLine("Token Submitted is : " + token);
            if (token != "") client.DefaultRequestHeaders.Add("Cookie", ".AspNet.ApplicationCookie=" + token);

            return;
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
            GetApplicationCookie(); // get token credentials

            string url = "CardData/ListCards";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<CardDto> cards = response.Content.ReadAsAsync<IEnumerable<CardDto>>().Result;

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
            GetApplicationCookie(); // get token credentials

            DetailsCard ViewModel = new DetailsCard();

            string url = "CardData/FindCard/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            CardDto SelectedCard = response.Content.ReadAsAsync<CardDto>().Result;

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
        // GET: Card/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Card/New
        [Authorize]
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
        [Authorize]
        public ActionResult Create(Card card)
        {
            GetApplicationCookie(); // get token credentials

            string url = "CardData/AddCard";

            string jsonpayload = jss.Serialize(card);

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

        /// <summary>
        /// Gets the selected card to update in the system
        /// </summary>
        /// <param name="id">represents the selected card id</param>
        /// <returns>
        /// Returns a view with the selected card existing details.
        /// </returns>


        // GET: Card/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie(); // get token credentials

            string url = "CardData/FindCard/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CardDto selectedcard = response.Content.ReadAsAsync<CardDto>().Result;
            return View(selectedcard);
        }

        /// <summary>
        /// Updates a Card in the system
        /// </summary>
        /// <param name="id"> Represents the id of a selected card to update </param>
        /// <param name="card"> Represenst an object of the Card </param>
        /// <returns>
        /// Returns to the details page with the updated values for the Card
        /// </returns>

        // POST: Card/Update/5
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Card card)
        {
            GetApplicationCookie(); // get token credentials

            string url = "CardData/UpdateCard/" + id;
            string jsonpayload = jss.Serialize(card);

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

        /// <summary>
        /// Displays a message to confirm the delete process of a card from the system
        /// </summary>
        /// <param name="id">Represents the card id</param>
        /// <returns>
        /// DeleteConfirm View 
        /// </returns>

        // GET: Card/DeleteConfirm/5
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie(); // get token credentials

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
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            GetApplicationCookie(); // get token credentials

            string url = "CardData/DeleteCard/" + id;
            HttpContent content = new StringContent("");
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
    }
}
