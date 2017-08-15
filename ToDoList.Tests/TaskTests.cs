using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System;
using ToDoList.Models;
using Categories.Models;

namespace ToDoList.Tests
{

   [TestClass]
   public class TaskTests : IDisposable
   {
       public TaskTests()
       {
           DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo_test;";
       }
       public void Dispose()
       {
         Task.DeleteAll();
         Category.DeleteAll();
       }

       [TestMethod]
       public void Equals_OverrideTrueForSameDescription_Task()
       {
         //Arrange, Act
         Task firstTask = new Task("Mow the lawn", 1);
         Task secondTask = new Task("Mow the lawn", 1);

         //Assert
         Assert.AreEqual(firstTask, secondTask);
       }

       [TestMethod]
       public void Save_SavesTaskToDatabase_TaskList()
       {
         //Arrange
         Task testTask = new Task("Mow the lawn", 1);
         testTask.Save();

         //Act
        List<Task> testList = new List<Task>{testTask};
         List<Task> result = Task.GetAll();


         //Assert
         CollectionAssert.AreEqual(testList, result);
       }
      [TestMethod]
       public void Save_DatabaseAssignsIdToObject_Id()
       {
         //Arrange
         Task testTask = new Task("Mow the lawn", 1);
         testTask.Save();

         //Act
         Task savedTask = Task.GetAll()[0];

         int result = savedTask.GetId();
         int testId = testTask.GetId();

         //Assert
         Assert.AreEqual(testId, result);
       }

       [TestMethod]
       public void Find_FindsTaskInDatabase_Task()
       {
         //Arrange
         Task testTask = new Task("Mow the lawn", 1);
         testTask.Save();

         //Act
         Task foundTask = Task.Find(testTask.GetId());

         //Assert
         Assert.AreEqual(testTask, foundTask);
       }
       [TestMethod]
       public void Delete_DeleteTaskInstanceInDatabase_Task()
       {
         Task testTask1 = new Task("Cut the dogs hair", 1);
         Task testTask2 = new Task("Cut the cats hair", 1);
         testTask1.Save();
         testTask2.Save();

         List<Task> expected = new List<Task> {testTask2};

         testTask1.Delete();
         List<Task> actual = Task.GetAll();


         CollectionAssert.AreEqual(expected, actual);
       }
   }
}
