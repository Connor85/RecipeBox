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
    }
}
