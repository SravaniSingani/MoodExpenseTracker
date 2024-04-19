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
    public class WeatherController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static WeatherController()
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
        public ActionResult List(string SearchKey = null)
        {
            GetApplicationCookie(); // get token credentials

            // Objective: Access Weather Data API and retrieve list of weathers

            string url = "WeatherData/ListWeathers/" + SearchKey;
            HttpResponseMessage response = client.GetAsync(url).Result;

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
            GetApplicationCookie(); // get token credentials


            DetailsWeather ViewModel = new DetailsWeather();
            // Objective: Access Weather Data API and find a weather by its id

            string url = "WeatherData/FindWeather/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            WeatherDto SelectedWeather = response.Content.ReadAsAsync<WeatherDto>().Result;

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

        // GET: Weather/Error
        public ActionResult Error()
        {
            return View();
        }

        // GET: Weather/New
        [Authorize]
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
        [Authorize]
        public ActionResult Create(Weather weather)
        {
            GetApplicationCookie(); // get token credentials

            string url = "WeatherData/AddWeather";

            string jsonpayload = jss.Serialize(weather);

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
        /// Gets the selected weather to update in the system
        /// </summary>
        /// <param name="id">represents the selected weather id</param>
        /// <returns>
        /// Returns a view with the selected weather existing details.
        /// </returns>

        // GET: Weather/Edit/2
        [Authorize]
        public ActionResult Edit(int id)
        {
            GetApplicationCookie(); // get token credentials

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
        // POST: Weather/Update/2
        [HttpPost]
        [Authorize]
        public ActionResult Update(int id, Weather weather)
        {
            GetApplicationCookie(); // get token credentials

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

        // GET: Weather/DeleteConfirm/2
        [Authorize]
        public ActionResult DeleteConfirm(int id)
        {
            GetApplicationCookie(); // get token credentials

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

        // POST: Weather/Delete/2
        [HttpPost]
        [Authorize]
        public ActionResult Delete(int id, FormCollection collection)
        {
            GetApplicationCookie(); // get token credentials

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
