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
    public class MoodDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// A list that returns all the Mood objects in the system
        /// </summary>
        /// <returns>
        /// All Mood objects in the system 
        /// </returns>
        ///<example>
        /// GET: api/MoodData/ListMoods
        /// CURL: curl https://localhost:44307/api/MoodData/ListMood
        /// </example>

        // List Moods

        // GET: api/MoodData/ListMoods
        [HttpGet]
        [ResponseType(typeof(Mood))]
        [Route("api/MoodData/ListMoods/{SearchKey?}")]
        public IHttpActionResult ListMoods(string SearchKey = null)
        {

            List<Mood> Moods = db.Moods.ToList();
            //if search key is entered
            if(!string.IsNullOrEmpty(SearchKey))
            {
                Moods = db.Moods.Where
                    (m => m.MoodName.Contains(SearchKey)).ToList();
            }

            List<MoodDto> moodDtos = new List<MoodDto>();

            Moods.ForEach(c => moodDtos.Add(new MoodDto()
            {
                MoodId = c.MoodId,
                MoodName = c.MoodName,

            }));

            return Ok(moodDtos);
        }

        /// <summary>
        /// Returns a selected Mood object in the system
        /// </summary>
        /// <param name="id">A selected Mood id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A Mood object in the system that matches the MoodID
        /// </returns>
        /// <example>
        /// GET: api/MoodData/FindMood/2
        /// CURL: curl https://localhost:44307/api/MoodData/FindMood/2
        /// </example>

        // Find a Mood

        // GET: api/MoodData/FindMood/2
        [HttpGet]
        [ResponseType(typeof(Mood))]
        [Route("api/MoodData/FindMood/{id}")]
        public IHttpActionResult FindMood(int id)
        {
            Mood mood = db.Moods.Find(id);
            MoodDto MoodDto = new MoodDto()
            {

                MoodId = mood.MoodId,
                MoodName = mood.MoodName,

            };
            if (mood == null)
            {
                return NotFound();
            }
            return Ok(MoodDto);
        }

        // All Mood Entries Will Remain Unchanged
        // CRUD not applied to Mood Entity

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MoodExists(int id)
        {
            return db.Moods.Count(e => e.MoodId == id) > 0;
        }
    }
}