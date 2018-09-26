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
    }
}
