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
    public class CategoryDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// A list that returns all the Categories in the system
        /// </summary>
        /// <returns>
        /// All Categories in the system 
        /// </returns>
        ///<example>
        /// GET: api/CategoryData/ListCategories
        /// CURL: curl https://localhost:44307/api/CategoryData/ListCategories
        /// </example>

        // List Categories

        // GET: api/CategoryData/ListCategories
        [HttpGet]
        [ResponseType(typeof(Category))]
        [Route("api/CategoryData/ListCategories/{SearchKey?}")]
        public IHttpActionResult ListCategories(string SearchKey = null)
        {

            List<Category> Categories = db.Categories.ToList();

            //if SearchKey is Entered
            if(!string.IsNullOrEmpty(SearchKey) )
            {
                Categories = db.Categories.Where
                    (c => c.CategoryName.Contains(SearchKey)).ToList();
            }

            List<CategoryDto> CategoryDtos = new List<CategoryDto>();

            Categories.ForEach(c => CategoryDtos.Add(new CategoryDto()
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName,

            }));

            return Ok(CategoryDtos);
        }


        /// <summary>
        /// Returns a selected category in the system
        /// </summary>
        /// <param name="id">A selected category id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// CONTENT: A category in the system that matches the CategoryId
        /// </returns>
        /// <example>
        /// GET: api/CategoryData/FindCategory/2
        /// CURL: curl https://localhost:44307/api/CategoryData/FindCategory/2
        /// </example>

        // Find a Category

        // GET: api/CategoryData/FindCategory/2
        [HttpGet]
        [ResponseType(typeof(Category))]
        [Route("api/CategoryData/FindCategory/{id}")]
        public IHttpActionResult FindCategory(int id)
        {
            Category Category = db.Categories.Find(id);
            CategoryDto CategoryDto = new CategoryDto()
            {

                CategoryId = Category.CategoryId,
                CategoryName = Category.CategoryName,

            };
            if (Category == null)
            {
                return NotFound();
            }
            return Ok(CategoryDto);
        }


        /// <summary>
        /// Updates a particular category in the system with Post data input
        /// </summary>
        /// <param name="id">Represents the category id (primary key)</param>
        /// <param name="category">JSON form data of a category</param>
        /// <returns>
        /// HEADER: 204 (Success, No Conetent Response)
        /// or
        /// HEADER: 400 (Bad Request)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CategoryData/UpdateCategory/1
        /// FORM Data: Category JSON Object
        /// CURL:  curl -d @category.json -H "Content-Type:application/json" https://localhost:44307/api/CategoryData/UpdateCategory/1
        /// </example>

        // Update a category 
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateCategory(int id, Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != category.CategoryId)
            {
                return BadRequest();
            }

            db.Entry(category).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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
        /// Add a category to the system with Post data input
        /// </summary>
        /// <param name="category">JSON form data of a category</param>
        /// <returns>
        /// HEADER: 201 (Created)
        /// CONTENT: Category Id, category Data
        /// or
        /// HEADER: 400 (Bad Request)
        /// </returns>
        /// <example>
        /// POST: api/CategoryData/AddCategory
        /// FORM Data: Category JSON Object
        /// CURL: curl -d @category.json -H "Content-Type:application/json" https://localhost:44307/api/CategoryData/AddCategory
        /// </example>

        // Add a Category
        [ResponseType(typeof(Category))]
        [HttpPost]
        public IHttpActionResult AddCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);
        }


        /// <summary>
        /// Deletes a category from the system by it's ID
        /// </summary>
        /// <param name="id">Represents the category id</param>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (Not Found)
        /// </returns>
        /// <example>
        /// POST: api/CategoryData/DeleteCategory/4
        /// CURL: curl -d "" https://localhost:44307/api/CategoryData/DeleteCategory/4
        /// </example>

        // Delete a Category
        [ResponseType(typeof(Category))]
        [HttpPost]
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return Ok(category);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int id)
        {
            return db.Categories.Count(e => e.CategoryId == id) > 0;
        }
    }
}