using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System.Collections.Generic;
using System;

namespace ToDoList.Tests
{

    [TestClass]
    public class TaskTests : IDisposable
    {
      public void Dispose()
      {
        Task.DeleteAll();
      }
      public TaskTests()
      {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=	8889;database=to_do_test;";
      }

      [TestMethod]
      public void GetAll_DatabaseEmptyAtFirst_0()
      {
        //Arrange
        int expected = 0;
        //Act
        int actual = Task.GetAll().Count;

        //Assert
        Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Task()
      {
        //Arrange
        Task expected = new Task("Mow the lawn");
        //Act

        Task actual = new Task("Mow the lawn");

        //Assert
        Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Save_SavesToDatabase_TaskList()
      {
        //Arrange
        Task testTask = new Task("Mow the lawn");
        List<Task> expected = new List<Task>{testTask};
        //Act
        testTask.Save();
        List<Task> actual = Task.GetAll();

        //Assert
        CollectionAssert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Save_AssignsIdToObject_Id()
      {
        //Arrange
        Task testTask = new Task("Mow the lawn");
        testTask.Save();
        int expected = testTask.GetId();

        //Act
        Task savedTask = Task.GetAll()[0];
        int actual = savedTask.GetId();

        //Assert
        Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Find_FindsTaskInDatabase_Task()
      {
        //Arrange
        Task testTask = new Task("Mow the lawn");
        testTask.Save();

        //Act
        Task foundTask = Task.Find(testTask.GetId());

        //Assert
        Assert.AreEqual(testTask, foundTask);
      }

    }
}
