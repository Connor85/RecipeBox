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

    public static Recipe Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM recipes WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int recipeId = 0;
      string recipeName = "";
      string recipeInstructions = "";
      while(rdr.Read())
      {
        recipeId = rdr.GetInt32(0);
        recipeName = rdr.GetString(1);
        recipeInstructions = rdr.GetString(2);
      }
      Recipe foundRecipe = new Recipe(recipeName,recipeInstructions, recipeId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundRecipe;
    }

    public void AddCategory(Category newCategory)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO categories_recipes (category_id, recipe_id) VALUES (@categoryId, @recipeId);";

            MySqlParameter category_id = new MySqlParameter();
            category_id.ParameterName = "@categoryId";
            category_id.Value = newCategory.GetId();
            cmd.Parameters.Add(category_id);

            MySqlParameter recipe_id = new MySqlParameter();
            recipe_id.ParameterName = "@recipeId";
            recipe_id.Value = _id;
            cmd.Parameters.Add(recipe_id);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
              conn.Dispose();
            }
        }

        public List<Category> GetCategories()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT categories.* FROM recipes
          JOIN categories_recipes ON (recipes.id = categories_recipes.recipe_id)
          JOIN categories ON (categories_recipes.category_id = categories.id)
          WHERE recipes.id = @RecipeId;";

          MySqlParameter recipeIdParameter = new MySqlParameter();
          recipeIdParameter.ParameterName = "@RecipeId";
          recipeIdParameter.Value = _id;
          cmd.Parameters.Add(recipeIdParameter);

          MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
          List<Category> categories = new List<Category>{};

          while(rdr.Read())
          {
            int categoryId = rdr.GetInt32(0);
            string categoryName = rdr.GetString(1);

            Category newCategory = new Category(categoryName, categoryId);
            categories.Add(newCategory);
          }
          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
          return categories;
        }
  }
}
