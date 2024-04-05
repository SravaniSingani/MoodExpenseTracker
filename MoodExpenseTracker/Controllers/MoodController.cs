using MoodExpenseTracker.Models;
using MoodExpenseTracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44307/api/");
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
        public ActionResult List()
        {

            // Objective: Access Mood Data API and retrieve list of moods
            // curl https://localhost:44307/api/MoodData/ListMoods

            // HttpClient client = new HttpClient() { };
            string url = "MoodData/ListMoods";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //  Debug.WriteLine("The response is: ");
            //  Debug.WriteLine(response.StatusCode);

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
            // Objective: Access Mood Data API and find a mood by its id
            // curl https://localhost:44307/api/MoodData/FindMood/{id}
            DetailsMood ViewModel = new DetailsMood();

            // HttpClient client = new HttpClient() { };
            string url = "MoodData/FindMood/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The response is: ");
            // Debug.WriteLine(response.StatusCode);

            MoodDto SelectedMood = response.Content.ReadAsAsync<MoodDto>().Result;

            // Debug.WriteLine("Mood received: " + SelectedMood.MoodName);

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
