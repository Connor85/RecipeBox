using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RecipeBox;

namespace RecipeBox.Models
{
  public class Ingredient
  {
    private string _name;
    private int _id;

    public Ingredient(string name, int id = 0)
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
      cmd.CommandText = @"DELETE FROM ingredients;";

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
      cmd.CommandText = @"INSERT INTO ingredients (name) VALUES (@name);";

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

    public static List<Ingredient> GetAll()
    {
      List<Ingredient> allIngredients = new List<Ingredient> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM ingredients;";

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int ingredientId = rdr.GetInt32(0);
        string ingredientName = rdr.GetString(1);
        Ingredient newIngredient = new Ingredient(ingredientName, ingredientId);
        allIngredients.Add(newIngredient);
      }

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allIngredients;
    }


    public static Ingredient Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM ingredients WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int IngredientId = 0;
      string IngredientName = "";

      while(rdr.Read())
      {
        IngredientId = rdr.GetInt32(0);
        IngredientName = rdr.GetString(1);
      }
      Ingredient foundIngredient = new Ingredient(IngredientName, IngredientId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return foundIngredient;
    }

    public void AddRecipe(Recipe newRecipe)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"INSERT INTO ingredients_recipes (ingredient_id, recipe_id) VALUES (@ingredientId, @recipeId);";

            MySqlParameter ingredient_id = new MySqlParameter();
            ingredient_id.ParameterName = "@ingredientId";
            ingredient_id.Value = _id;
            cmd.Parameters.Add(ingredient_id);

            MySqlParameter recipe_id = new MySqlParameter();
            recipe_id.ParameterName = "@recipeId";
            recipe_id.Value = newRecipe.GetId();
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
          cmd.CommandText = @"SELECT recipes.* FROM ingredients
          JOIN ingredients_recipes ON (ingredient.id = ingredients_recipes.ingredient_id)
          JOIN recipes ON (ingredients_recipes.recipe_id = recipes.id)
          WHERE ingredients.id = @ingredientId;";

          MySqlParameter recipeIdParameter = new MySqlParameter();
          ingredientIdParameter.ParameterName = "@ingredientId";
          ingredientIdParameter.Value = _id;
          cmd.Parameters.Add(ingredientIdParameter);

          MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
          List<Recipe> recipes = new List<Recipe>{};

          while(rdr.Read())
          {
            int recipeId = rdr.GetInt32(0);
            string recipeName = rdr.GetString(1);

            Recipe newRecipe = new Recipe(recipeName, recipeId);
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
