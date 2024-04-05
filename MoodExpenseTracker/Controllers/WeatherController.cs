using MoodExpenseTracker.Models;
using MoodExpenseTracker.Models.ViewModels;
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
    public class WeatherController : Controller
    {

            private static readonly HttpClient client;
            private JavaScriptSerializer jss = new JavaScriptSerializer();
            static WeatherController()
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:44307/api/");
            }


            /// <summary>
            /// Displays a list of weathers in the system
            /// </summary>
            /// <returns>
            /// Returns a view for Weather List
            /// </returns>
            /// <example>
            /// GET: Weather/List
            /// curl: curl https://localhost:44307/api/WeatherData/ListWeathers
            /// </example>
            // GET: Weather/List
            public ActionResult List()
            {

                // Objective: Access Weather Data API and retrieve list of weathers
                // curl https://localhost:44307/api/WeatherData/ListWeathers

                // HttpClient client = new HttpClient() { };
                string url = "WeatherData/ListWeathers";
                HttpResponseMessage response = client.GetAsync(url).Result;

                //  Debug.WriteLine("The response is: ");
                //  Debug.WriteLine(response.StatusCode);

                IEnumerable<WeatherDto> weathers = response.Content.ReadAsAsync<IEnumerable<WeatherDto>>().Result;

                // Debug.WriteLine("Number of weathers: " + weathers.Count());

                return View(weathers);
            }



        /// <summary>
        /// Retreieves a selcted weather from the list of weathers
        /// </summary>
        /// <param name="id">Represents the selcted weather id</param>
        /// <returns>
        /// Returns a view with details of the selected weather
        /// </returns>
        /// <example>
        ///  GET: Weather/Details/2
        ///  curl https://localhost:44307/api/WeatherData/FindWeather/{id}
        /// </example>

        // GET: Weather/Details/2
        public ActionResult Details(int id)
        {
            // Objective: Access Weather Data API and find a weather by its id
            // curl https://localhost:44307/api/WeatherData/FindWeather/{id}
            DetailsWeather ViewModel = new DetailsWeather();

            // HttpClient client = new HttpClient() { };
            string url = "WeatherData/FindWeather/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The response is: ");
            // Debug.WriteLine(response.StatusCode);

            WeatherDto SelectedWeather = response.Content.ReadAsAsync<WeatherDto>().Result;

            // Debug.WriteLine("Weather received: " + SelectedWeather.WeatherName);

            //Showcase List of Expenses associated with the Weather
            url = "expensedata/listexpensesforweather/" + id;

            ViewModel.SelectedWeather = SelectedWeather;
            response = client.GetAsync(url).Result;
            IEnumerable<ExpenseDto> RelatedExpensestoWeather = response.Content.ReadAsAsync<IEnumerable<ExpenseDto>>().Result;

            ViewModel.RelatedExpensestoWeather = RelatedExpensestoWeather;
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

        // GET: Weather/New
        public ActionResult New()
        {
            return View();
        }



        /// <summary>
        /// Adds a weather to the system
        /// </summary>
        /// <param name="weather">Represenst an object of the Weather</param>
        /// <returns>
        /// Returns a List with the added Weather data
        /// or
        /// 404: Returns an Error view displaying the error message
        /// </returns>

        // POST: Weather/Create
        [HttpPost]
        public ActionResult Create(Weather weather)
        {
            // Objective: Add a new Weather into the system using API
            // curl: -d @weather.json -H "Content-Type:application/json" https://localhost:44307/api/WeatherData/AddWeather

            Debug.WriteLine("The weather name craeted is: ");
            Debug.WriteLine(weather.WeatherName);

            string url = "WeatherData/AddWeather";
            
            string jsonpayload = jss.Serialize(weather);

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
                Debug.WriteLine("Error updating Weather. Status code: " + response.StatusCode);
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine("Error message from server: " + errorMessage);
                return RedirectToAction("Error");

            }
        }


        /// <summary>
        /// Gets the selected weather to update in the system
        /// </summary>
        /// <param name="id">represents the selected weather id</param>
        /// <returns>
        /// Returns a view with the selected weather existing details.
        /// </returns>
        // GET: Weather/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "WeatherData/FindWeather/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WeatherDto selectedweather = response.Content.ReadAsAsync<WeatherDto>().Result;
            return View(selectedweather);
        }

        /// <summary>
        /// Updates an existing Weather in the system
        /// </summary>
        /// <param name="id">Represents the selected Weather id</param>
        /// <param name="weather"> represents the Weather object</param>
        /// <returns>
        /// Returns to a view to the details of the selected weather
        /// </returns>
        // POST: Weather/Edit/2
        [HttpPost]
        public ActionResult Update(int id, Weather weather)
        {
            string url = "WeatherData/UpdateWeather/" + id;
            string jsonpayload = jss.Serialize(weather);

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
                Debug.WriteLine("Error updating Weather. Status code: " + response.StatusCode);
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine("Error message from server: " + errorMessage);
                return RedirectToAction("Error");

            }
        }





        /// <summary>
        /// Displays a message to confirm the delete process of a weather from the system
        /// </summary>
        /// <param name="id">Represents the weather id</param>
        /// <returns>
        /// DeleteConfirm View 
        /// </returns>

        // GET: Weather/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "WeatherData/FindWeather/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            WeatherDto selectedweather = response.Content.ReadAsAsync<WeatherDto>().Result;
            return View(selectedweather);
        }


        /// <summary>
        /// Removes a weather from the system
        /// </summary>
        /// <param name="id">Represents the weather id</param>
        /// <param name="collection">Represents the Weather object</param>
        /// <returns>
        /// Deletes the selected weather and returns to the Weather list
        /// </returns>

        // POST: Weather/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "WeatherData/DeleteWeather/" + id;
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
