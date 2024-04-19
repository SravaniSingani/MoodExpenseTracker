using MoodExpenseTracker.Models;
using MoodExpenseTracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace MoodExpenseTracker.Controllers
{
    public class MoodController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static MoodController()
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
        /// Displays a list of moods in the system
        /// </summary>
        /// <returns>
        /// Returns a view for Mood List
        /// </returns>
        /// <example>
        /// GET: Mood/List
        /// curl: curl https://localhost:44307/api/MoodData/ListMoods
        /// </example>

        // GET: Mood/List
        public ActionResult List(string SearchKey = null)
        {
            GetApplicationCookie(); // get token credentials

            // Objective: Access Mood Data API and retrieve list of moods

            string url = "MoodData/ListMoods/" + SearchKey;
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<MoodDto> moods = response.Content.ReadAsAsync<IEnumerable<MoodDto>>().Result;

            // Debug.WriteLine("Number of moods: " + moods.Count());
            return View(moods);
        }

        /// <summary>
        /// Retreieves a selcted mood from the list of moods
        /// </summary>
        /// <param name="id">Represents the selcted mood id</param>
        /// <returns>
        /// Returns a view with details of the selected mood
        /// </returns>
        /// <example>
        ///  GET: Mood/Details/2
        ///  curl https://localhost:44307/api/MoodData/FindMood/{id}
        /// </example>
        // GET: Mood/Details/2
        public ActionResult Details(int id)
        {
            GetApplicationCookie(); // get token credentials

            // Objective: Access Mood Data API and find a mood by its id

            DetailsMood ViewModel = new DetailsMood();

            string url = "MoodData/FindMood/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            MoodDto SelectedMood = response.Content.ReadAsAsync<MoodDto>().Result;

            //Showcase List of Expenses associated with the Mood
            url = "expensedata/listexpensesformood/" + id;

            ViewModel.SelectedMood = SelectedMood;
            response = client.GetAsync(url).Result;
            IEnumerable<ExpenseDto> RelatedExpensestoMood = response.Content.ReadAsAsync<IEnumerable<ExpenseDto>>().Result;

            ViewModel.RelatedExpensestoMood = RelatedExpensestoMood;
            return View(ViewModel);
        }

        // All the mood entries will remian static and unchanged
        // No CRUD applied to the Mood Entity

    }
}
