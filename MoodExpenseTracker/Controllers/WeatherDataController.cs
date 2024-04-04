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

        // GET: api/WeatherData
        public IQueryable<Weather> GetWeathers()
        {
            return db.Weathers;
        }

        // GET: api/WeatherData/5
        [ResponseType(typeof(Weather))]
        public IHttpActionResult GetWeather(int id)
        {
            Weather weather = db.Weathers.Find(id);
            if (weather == null)
            {
                return NotFound();
            }

            return Ok(weather);
        }

        // PUT: api/WeatherData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWeather(int id, Weather weather)
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

        // POST: api/WeatherData
        [ResponseType(typeof(Weather))]
        public IHttpActionResult PostWeather(Weather weather)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Weathers.Add(weather);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = weather.WeatherId }, weather);
        }

        // DELETE: api/WeatherData/5
        [ResponseType(typeof(Weather))]
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