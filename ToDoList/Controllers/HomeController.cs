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

        [HttpGet("/category/{catid}/task/{taskid}")]
        public ActionResult TaskDelete(int catid, int taskid)
        {
            Dictionary<string,Object> model = new Dictionary<string,Object> {};
            Category category = Category.Find(catid);
            Task deleteMe = Task.Find(taskid);
            deleteMe.Delete();
            List<Task> taskList = category.GetTasks();
            model.Add("category", category);



            model.Add("tasks", taskList);

            return View("CategoryView", model);
        }

        [HttpGet("/category/{id}")]
        public ActionResult CategoryView(int id)
        {
            Dictionary<string, Object> model = new Dictionary<string, Object>();
            Category newCategory = Category.Find(id);
            List<Task> taskList = newCategory.GetTasks();

            model.Add("category", newCategory);
            model.Add("tasks", taskList);

            return View(model);
        }

        [HttpPost("/category/{id}/create-task")]
        public ActionResult CreateTask(int id)
        {
          Dictionary<string, Object> model = new Dictionary<string, Object>();

          Category newCategory = Category.Find(id);
          Task newTask = new Task(Request.Form["create-task"], newCategory.GetId());
          newTask.Save();

          model.Add("category", newCategory);
          model.Add("task", newTask);

          return View(model);
        }


    }
}
