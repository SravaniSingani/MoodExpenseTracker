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
    public class CategoryController : Controller
    {


        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();
        static CategoryController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44307/api/");
        }


        /// <summary>
        /// Displays a list of categories in the system
        /// </summary>
        /// <returns>
        /// Returns a view for Category List
        /// </returns>
        /// <example>
        /// GET: Category/List
        /// curl: curl https://localhost:44307/api/CategoryData/ListCategories
        /// </example>
        // GET: Category/List
        public ActionResult List()
        {

            // Objective: Access Category Data API and retrieve list of categories
            // curl https://localhost:44307/api/CategoryData/ListCategories

            // HttpClient client = new HttpClient() { };
            string url = "CategoryData/ListCategories";
            HttpResponseMessage response = client.GetAsync(url).Result;

            //  Debug.WriteLine("The response is: ");
            //  Debug.WriteLine(response.StatusCode);

            IEnumerable<CategoryDto> categories = response.Content.ReadAsAsync<IEnumerable<CategoryDto>>().Result;

            // Debug.WriteLine("Number of categories: " + categories.Count());

            return View(categories);
        }




        /// <summary>
        /// Retreieves a selcted category from the list of categories
        /// </summary>
        /// <param name="id">Represents the selcted category id</param>
        /// <returns>
        /// Reeturns a view with details of the selected category
        /// </returns>
        /// <example>
        ///  GET: Category/Details/5
        ///  curl https://localhost:44384/api/CategoryData/FindCategory/{id}
        /// </example>

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            // Objective: Access Category Data API and find a category by its id
            // curl https://localhost:44384/api/CategoryData/FindCategory/{id}
            DetailsCategory ViewModel = new DetailsCategory();

            // HttpClient client = new HttpClient() { };
            string url = "CategoryData/FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            // Debug.WriteLine("The response is: ");
            // Debug.WriteLine(response.StatusCode);

            CategoryDto SelectedCategory = response.Content.ReadAsAsync<CategoryDto>().Result;

            // Debug.WriteLine("Category received: " + selectedcategory.CategoryName);

            //Showcase List of Expenses associated with the category
            url = "expensedata/listexpensesforcategory/" + id;

            ViewModel.SelectedCategory = SelectedCategory;
            response = client.GetAsync(url).Result;
            IEnumerable<ExpenseDto> RelatedExpensestoCategory = response.Content.ReadAsAsync<IEnumerable<ExpenseDto>>().Result;

            ViewModel.RelatedExpensestoCategory = RelatedExpensestoCategory;
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

        // GET: Category/New
        public ActionResult New()
        {
            return View();
        }



        /// <summary>
        /// Adds a category to the system
        /// </summary>
        /// <param name="category">Represenst an object of the Category</param>
        /// <returns>
        /// Returns a List with the added category data
        /// or
        /// 404: Returns an Error view displaying the error message
        /// </returns>

        // POST: Category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            // Objective: Add a new category into the system using API
            // curl: -d @category.json -H "Content-Type:application/json" https://localhost:44384/api/CategoryData/AddCategory

            Debug.WriteLine("The category name craeted is: ");
            Debug.WriteLine(category.CategoryName);

            string url = "CategoryData/AddCategory";

            string jsonpayload = jss.Serialize(category);

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
                Debug.WriteLine("Error updating category. Status code: " + response.StatusCode);
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine("Error message from server: " + errorMessage);
                return RedirectToAction("Error");

            }
        }




        /// <summary>
        /// Gets the selected category to update in the system
        /// </summary>
        /// <param name="id">represents the selected category id</param>
        /// <returns>
        /// Returns a view with the selected category existing details.
        /// </returns>
        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "CategoryData/FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CategoryDto selectedcategory = response.Content.ReadAsAsync<CategoryDto>().Result;
            return View(selectedcategory);
        }


        /// <summary>
        /// Updates an existing category in the system
        /// </summary>
        /// <param name="id">Represents the selected category id</param>
        /// <param name="category"> represents the category object</param>
        /// <returns>
        /// Returns to a view to the details of the selected category
        /// </returns>
        // POST: Category/Edit/5
        [HttpPost]
        public ActionResult Update(int id, Category category)
        {
            string url = "CategoryData/UpdateCategory/" + id;
            string jsonpayload = jss.Serialize(category);

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
                Debug.WriteLine("Error updating category. Status code: " + response.StatusCode);
                string errorMessage = response.Content.ReadAsStringAsync().Result;
                Debug.WriteLine("Error message from server: " + errorMessage);
                return RedirectToAction("Error");

            }
        }


        /// <summary>
        /// Displays a message to confirm the delete process of a category from the system
        /// </summary>
        /// <param name="id">Represents the category id</param>
        /// <returns>
        /// DeleteConfirm View 
        /// </returns>

        // GET: Category/DeleteConfirm/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "CategoryData/FindCategory/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            CategoryDto selectedcategory = response.Content.ReadAsAsync<CategoryDto>().Result;
            return View(selectedcategory);
        }


        /// <summary>
        /// Removes a category from the system
        /// </summary>
        /// <param name="id">Represents the category id</param>
        /// <param name="collection">Represents the category object</param>
        /// <returns>
        /// Deletes the selected category and returns to the category list
        /// </returns>

        // POST: Category/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            string url = "CategoryData/DeleteCategory/" + id;
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
