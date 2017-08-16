using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace ToDoList.Models
{
  public class Task
  {
    private int _id;
    private string _description;
    private int _categoryId;

    public Task(string Description, int categoryId, int Id = 0)
    {
      _id = Id;
      _categoryId = categoryId;
      _description = Description;
    }
    public int GetId()
    {
      return _id;
    }
    public string GetDescription()
    {
      return _description;
    }
    public int GetCategoryId()
    {
      return _categoryId;
    }

    public override bool Equals(Object otherTask)
   {
     if (!(otherTask is Task))
     {
       return false;
     }
     else
     {
       Task newTask = (Task) otherTask;
       bool idEquality = (this.GetId() == newTask.GetId());
       bool descriptionEquality = (this.GetDescription() == newTask.GetDescription());
       bool categoryEquality = this.GetCategoryId() == newTask.GetCategoryId();

       return (idEquality && descriptionEquality && categoryEquality);
     }
    }

    public override int GetHashCode()
        {
             return this.GetDescription().GetHashCode();
        }

    public static List<Task> GetAll()
    {
      List<Task> allTasks = new List<Task> {};

      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM tasks;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskName = rdr.GetString(1);
        int taskCategoryId = rdr.GetInt32(2);
        Task newTask = new Task(taskName, taskCategoryId, taskId);
        allTasks.Add(newTask);
      }
      conn.Close();
      return allTasks;
    }

    public void Save()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO tasks (description, category_id) VALUES (@description, @category_id);";

        MySqlParameter description = new MySqlParameter();
        description.ParameterName = "@description";
        description.Value = this._description;
        cmd.Parameters.Add(description);

        MySqlParameter categoryId = new MySqlParameter();
        categoryId.ParameterName = "@category_id";
        categoryId.Value = this._categoryId;
        cmd.Parameters.Add(categoryId);

        cmd.ExecuteNonQuery();
        _id = (int) cmd.LastInsertedId;
        conn.Close();
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM tasks WHERE id = @taskId;";

      MySqlParameter taskId = new MySqlParameter();
      taskId.ParameterName = "@taskId";
      taskId.Value = this._id;
      cmd.Parameters.Add(taskId);

      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public static void DeleteAll()
     {
       MySqlConnection conn = DB.Connection();
       conn.Open();
       var cmd = conn.CreateCommand() as MySqlCommand;
       cmd.CommandText = @"DELETE FROM tasks;";
       cmd.ExecuteNonQuery();
       conn.Close();
     }
     public static Task Find(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM tasks WHERE id = @searchId;";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = id;
        cmd.Parameters.Add(searchId);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;

        int taskId = 0;
        string taskDescription = "";
        int taskCategoryId = -1;

        while (rdr.Read())
        {
            taskId = rdr.GetInt32(0);
            taskDescription = rdr.GetString(1);
            taskCategoryId = rdr.GetInt32(2);
        }
        Task foundTask = new Task(taskDescription, taskCategoryId, taskId);
        conn.Close();
        return foundTask;
      }

      public void UpdateDescription(string newDescription)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"UPDATE tasks SET description = @newDescription WHERE id = @searchId;";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = _id;
        cmd.Parameters.Add(searchId);

        MySqlParameter description = new MySqlParameter();
        description.ParameterName = "@newDescription";
        description.Value = newDescription;
        cmd.Parameters.Add(description);

        cmd.ExecuteNonQuery();
        conn.Close();
      }
  }
}
