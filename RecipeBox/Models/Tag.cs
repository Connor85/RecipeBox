using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using RecipeBox;

namespace RecipeBox.Models
{
    public class Tag
    {
      private string _name;
      private int _id;

      public Tag(string name, int id = 0)
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
        cmd.CommandText = @"DELETE FROM tags;";

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
        cmd.CommandText = @"INSERT INTO tags (name) VALUES (@name);";

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

      public static List<Tag> GetAll()
      {
        List<Tag> allTags = new List<Tag> {};
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM tags;";

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        while(rdr.Read())
        {
          int tagId = rdr.GetInt32(0);
          string tagName = rdr.GetString(1);
          Tag newTag = new Tag(tagName, tagId);
          allTags.Add(newTag);
        }

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return allTags;
      }


      public static Tag Find(int id)
      {
        MySqlConnection conn = DB.Connection();
        conn.Open();

        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT * FROM tags WHERE id = (@searchId);";

        MySqlParameter searchId = new MySqlParameter();
        searchId.ParameterName = "@searchId";
        searchId.Value = id;
        cmd.Parameters.Add(searchId);

        var rdr = cmd.ExecuteReader() as MySqlDataReader;
        int TagId = 0;
        string TagName = "";

        while(rdr.Read())
        {
          TagId = rdr.GetInt32(0);
          TagName = rdr.GetString(1);
        }
        Tag foundTag = new Tag(TagName, TagId);

        conn.Close();
        if (conn != null)
        {
          conn.Dispose();
        }
        return foundTag;
      }

      public void AddRecipe(Recipe newRecipe)
          {
              MySqlConnection conn = DB.Connection();
              conn.Open();
              var cmd = conn.CreateCommand() as MySqlCommand;
              cmd.CommandText = @"INSERT INTO tags_recipes (tag_id, recipe_id) VALUES (@tagId, @recipeId);";

              MySqlParameter tag_id = new MySqlParameter();
              tag_id.ParameterName = "@tagId";
              tag_id.Value = _id;
              cmd.Parameters.Add(tag_id);

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
            cmd.CommandText = @"SELECT recipes.* FROM tags
            JOIN tags_recipes ON (tags.id = tags_recipes.tag_id)
            JOIN recipes ON (tags_recipes.recipe_id = recipes.id)
            WHERE tags.id = @tagId;";

            MySqlParameter tagIdParameter = new MySqlParameter();
            tagIdParameter.ParameterName = "@tagId";
            tagIdParameter.Value = _id;
            cmd.Parameters.Add(tagIdParameter);

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
