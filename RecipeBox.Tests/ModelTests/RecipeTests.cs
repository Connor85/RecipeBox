using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecipeBox.Models;
using System;
using System.Collections.Generic;


namespace RecipeBox.Tests
{
  [TestClass]
  public class RecipeTests : IDisposable
  {
    public void Dispose()
    {
      Recipe.DeleteAll();
    }
    public RecipeTests()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=recipe_box_test;";
    }

    [TestMethod]
    public void GetAll_DBStartsEmpty_0()
    {
      //Arrange
      //Act
      int result = Recipe.GetAll().Count;

      //Assert
      Assert.AreEqual(0,result);
    }
  }
}
