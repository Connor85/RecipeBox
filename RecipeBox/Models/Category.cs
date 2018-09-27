using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RecipeBox;

namespace RecipeBox.Models
{
  public class Category
  {
    private string _name;
    private int _id;

    public Category(string name, int id = 0)
    {
      _name = name;
      _id = id;
    }

    public string GetName()
    {
      return _name;
    }

    public int GetId()
    {
      return _id;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM categories;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO categories (name) VALUES (@name);";

      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Category> GetAll()
    {
      List<Category> allCategories = new List<Category> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        string categoryName = rdr.GetString(1);
        Category newCategory = new Category(categoryName, categoryId);
        allCategories.Add(newCategory);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCategories;
    }


    public static Category Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int CategoryId = 0;
      string CategoryName = "";

      while(rdr.Read())
      {
        CategoryId = rdr.GetInt32(0);
        CategoryName = rdr.GetString(1);
      }
      Category foundCategory = new Category(CategoryName, CategoryId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundCategory;
    }

    public void AddRecipe(Recipe newRecipe)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO categories_recipes (category_id, recipe_id) VALUES (@categoryId, @recipeId);";

            MySqlParameter category_id = new MySqlParameter();
            category_id.ParameterName = "@categoryId";
            category_id.Value = newRecipe.GetId();
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

        public List<Recipe> GetRecipes()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();
          MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"SELECT recipes.* FROM categories
          JOIN categories_recipes ON (category.id = categories_recipes.category_id)
          JOIN recipes ON (categories_recipes.recipe_id = recipes.id)
          WHERE categories.id = @categoryId;";

          MySqlParameter categoryIdParameter = new MySqlParameter();
          categoryIdParameter.ParameterName = "@categoryId";
          categoryIdParameter.Value = _id;
          cmd.Parameters.Add(categoryIdParameter);

          MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
          List<Recipe> recipes = new List<Recipe>{};

          while(rdr.Read())
          {
            int recipeId = rdr.GetInt32(0);
            string recipeName = rdr.GetString(1);
            string recipeInstructions = rdr.GetString(2);

            Recipe newRecipe = new Recipe(recipeName, recipeInstructions, recipeId);
            recipes.Add(newRecipe);
          }
          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
          return recipes;
        }
  }
}
