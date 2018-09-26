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
    }
}
