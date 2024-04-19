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
    public class WeatherDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        /// <summary>
        /// A list that returns all the Weather objects in the system
        /// </summary>
        /// <returns>
        /// All Weather objects in the system 
        /// </returns>
        ///<example>
        /// GET: api/WeatherData/ListWeathers
        /// CURL: curl https://localhost:44307/api/WeatherData/ListWeathers
        /// </example>

        // List Categories

        // GET: api/WeatherData/ListWeathers
        [HttpGet]
        [ResponseType(typeof(Weather))]
        [Route("api/WeatherData/ListWeathers/{SearchKey?}")]
        public IHttpActionResult ListWeathers(string SearchKey = null)
        {

            List<Weather> weathers = db.Weathers.ToList();
            //if SearchKey is entered
            if(!string.IsNullOrEmpty(SearchKey) )
            {
                weathers = db.Weathers.Where
                    (w => w.WeatherName.Contains(SearchKey)).ToList();
            }

            List<WeatherDto> weatherDtos = new List<WeatherDto>();

            weathers.ForEach(c => weatherDtos.Add(new WeatherDto()
            {
                WeatherId = c.WeatherId,
                WeatherName = c.WeatherName,

            }));

            return Ok(weatherDtos);
        }

        /// <summary>
        /// Returns a selected Weather object in the system
        /// </summary>
        /// <param name="id">A selected Weather id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A category in the system that matches the WeatherId
        /// </returns>
        /// <example>
        /// GET: api/WeatherData/FindWeather/2
        /// CURL: curl https://localhost:44307/api/WeatherData/FindWeather/2
        /// </example>

        // Find a Weather

        // GET: api/WeatherData/FindWeather/2
        [HttpGet]
        [ResponseType(typeof(Weather))]
        [Route("api/WeatherData/FindWeather/{id}")]
        public IHttpActionResult FindWeather(int id)
        {
            Weather weather = db.Weathers.Find(id);
            WeatherDto weatherDto = new WeatherDto()
            {

                WeatherId = weather.WeatherId,
                WeatherName = weather.WeatherName,

            };
            if (weather == null)
            {
                return NotFound();
            }
            return Ok(weatherDto);
        }



        /// <summary>
        /// Updates a particular Weather object in the system with Post data input
        /// </summary>
        /// <param name="id">Represents the weather id (primary key)</param>
        /// <param name="weather">JSON form data of a weather</param>
        /// <returns>
        /// HEADER: 204 (Success, No Content Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/WeatherData/UpdateWeather/1
        /// FORM Data: Weather JSON Object
        /// CURL:  curl -d @weather.json -H "Content-Type:application/json" https://localhost:44307/api/WeatherData/UpdateWeather/1
        /// </example>

        // Update a category 
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateWeather(int id, Weather weather)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != weather.WeatherId)
            {
                return BadRequest();
            }

            db.Entry(weather).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeatherExists(id))
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
        /// Add a Weather object to the system with Post data input
        /// </summary>
        /// <param name="weather">JSON form data of a weather</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Weather Id, Weather Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/WeatherData/AddWeather
        /// FORM Data: Weather JSON Object
        /// CURL: curl -d @weather.json -H "Content-Type:application/json" https://localhost:44307/api/WeatherData/AddWeather
        /// </example>
        [ResponseType(typeof(Weather))]
        [HttpPost]
        public IHttpActionResult AddWeather(Weather weather)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Weathers.Add(weather);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = weather.WeatherId }, weather);
        }


        /// <summary>
        /// Deletes a Weather object from the system by it's ID
        /// </summary>
        /// <param name="id">Represents the Weather id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/WeatherData/DeleteWeather/4
        /// CURL: curl -d "" https://localhost:44384/api/WeatherData/DeleteWeather/4
        /// </example>
        // DELETE: api/WeatherData/5
        [ResponseType(typeof(Weather))]
        [HttpPost]  
        public IHttpActionResult DeleteWeather(int id)
        {
            Weather weather = db.Weathers.Find(id);
            if (weather == null)
            {
                return NotFound();
            }

            db.Weathers.Remove(weather);
            db.SaveChanges();

            return Ok(weather);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool WeatherExists(int id)
        {
            return db.Weathers.Count(e => e.WeatherId == id) > 0;
        }
    }
}