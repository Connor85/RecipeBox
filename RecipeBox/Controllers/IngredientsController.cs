using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System;

namespace RecipeBox.Controllers
{
  public class IngredientsController : Controller
  {
    [HttpGet("/ingredients")]
    public ActionResult Index()
    {
      List<Ingredient> allIngredients = Ingredient.GetAll();
      return View(allIngredients);
    }

    [HttpGet("/ingredients/new")]
    public ActionResult CreateForm()
    {
      return View();
    }

    [HttpPost("/ingredients")]
    public ActionResult Create()
    {
      Ingredient newIngredient = new Ingredient(Request.Form["ingredient-name"]);
      newIngredient.Save();
      return RedirectToAction("Index");
    }

    [HttpGet("/ingredients/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Ingredient selectedIngredient = Ingredient.Find(id);
      List<Recipe> ingredientRecipes = selectedIngredient.GetRecipes();
      List<Recipe> allRecipes = Recipe.GetAll();
      model.Add("selectedIngredient", selectedIngredient);
      model.Add("ingredientRecipes", ingredientRecipes);
      model.Add("allRecipes", allRecipes);
      return View(model);
    }

    [HttpGet("/ingredients/{id}/recipes/new")]
    public ActionResult CreateRecipeForm()
    {
      return View("~/Views/Recipes/CreateForm.cshtml");
    }

    [HttpPost("/ingredients/{ingredientId}/recipes/new")]
    public ActionResult AddRecipe(int ingredientId)
    {
      Ingredient ingredient = Ingredient.Find(ingredientId);
      Recipe newRecipe = Recipe.Find(Int32.Parse(Request.Form["ingredient-id"]));
      ingredient.AddRecipe(newRecipe);
      return RedirectToAction("Details", new {id = ingredientId});
    }

    // [HttpGet("/ingredients/{ingredientId}/update")]
    // public ActionResult UpdateForm(int recipeId)
    // {
    //    Ingredient thisIngredient = Ingredient.Find(recipeId);
    //    return View("update", thisIngredient);
    // }
    //
    // [HttpPost("/ingredients/{recipeId}/update")]
    // public ActionResult Update(int recipeId)
    // {
    //   Ingredient thisIngredient = Ingredient.Find(recipeId);
    //   thisIngredient.Edit(Request.Form["new-recipe-name"], Request.Form["new-recipe-number"]);
    //   return RedirectToAction("Index");
    // }
    //
    // [HttpGet("/ingredients/{recipeid}/delete")]
    // public ActionResult DeleteOne(int recipeId)
    // {
    //   Ingredient thisIngredient = Ingredient.Find(recipeId);
    //   thisIngredient.Delete();
    //   return RedirectToAction("Index");
    // }
  }
}
