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
    }
}
