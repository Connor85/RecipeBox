using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RecipeBox;

namespace RecipeBox.Models
{
  public class Recipe
  {
    private string _name;
    private string _instructions;
    private int _id;

    public Recipe(string name, string instructions, int id = 0)
    {
      _name = name;
      _instructions = instructions;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }

    public string GetInstructions()
    {
      return _instructions;
    }

    public int GetId()
    {
      return _id;
    }

    public override bool Equals(System.Object otherRecipe)
    {
      if (!(otherRecipe is Recipe))
      {
        return false;
      }
      else
      {
        Recipe newRecipe = (Recipe) otherRecipe;
        bool areIdsEqual = (this.GetId() == newRecipe.GetId());
        bool areNamesEqual = (this.GetName() == newRecipe.GetName());
        return (areIdsEqual && areNamesEqual);
      }
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO recipes (name, instructions) VALUES (@name, @instructions);";

      MySqlParameter newName = new MySqlParameter();
      newName.ParameterName = "@name";
      newName.Value = this.GetName();
      cmd.Parameters.Add(newName);

      MySqlParameter newInstructions = new MySqlParameter();
      newInstructions.ParameterName = "@instructions";
      newInstructions.Value = this.GetInstructions();
      cmd.Parameters.Add(newInstructions);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Recipe> GetAll()
    {
      List<Recipe> allRecipes = new List<Recipe> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM recipes;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int recipeId = rdr.GetInt32(0);
        string recipeName = rdr.GetString(1);
        string recipeInstructions = rdr.GetString(2);
        Recipe newRecipe = new Recipe(recipeName, recipeInstructions, recipeId);
        allRecipes.Add(newRecipe);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allRecipes;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE from recipes;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if(conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
