using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using Categories.Models;
using System.Collections.Generic;
using System;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public ActionResult Index()
        {
          List<Category> categoryList = Category.GetAll();
          return View(categoryList);
        }

        [HttpPost("/create-category")]
        public ActionResult CreateCategory()
        {
          string userInput = Request.Form["create-category"];

          Category newCategory = new Category(userInput);
          newCategory.Save();

          return View(newCategory);
        }

        [HttpPost("/delete-categories")]
        public ActionResult DeleteCategories()
        {
          Category.DeleteAll();

          return View();
        }

        [HttpGet("/category/{id}")]
        public ActionResult CategoryView(int id)
        {
            Category newCategory = Category.Find(id);
            return View(newCategory);
        }

    }
}
