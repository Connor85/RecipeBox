using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System;

namespace RecipeBox.Controllers
{
  public class RecipesController : Controller
  {
    [HttpGet("/recipes")]
    public ActionResult Index()
    {
      List<Recipe> allRecipes = Recipe.GetAll();
      return View(allRecipes);
    }

    [HttpGet("/recipes/new")]
    public ActionResult CreateForm()
    {
      return View();
    }

    [HttpPost("/recipes")]
    public ActionResult Create()
    {
      Recipe newRecipe = new Recipe(Request.Form["recipe-name"], Request.Form["recipe-instruction"]);
      newRecipe.Save();
      return RedirectToAction("Index");
    }

    [HttpGet("/recipes/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Recipe selectedRecipe = Recipe.Find(id);
      List<Category> recipeCategories = selectedRecipe.GetCategories();
      List<Tag> recipeTags = selectedRecipe.GetTags();
      List<Ingredient> recipeIngredients = selectedRecipe.GetIngredients();
      List<Category> allCategories = Category.GetAll();
      List<Tag> allTags = Tag.GetAll();
      List<Ingredient> allIngredients = Ingredient.GetAll();
      model.Add("selectedRecipe", selectedRecipe);
      model.Add("recipeCategories", recipeCategories);
      model.Add("recipeTags", selectedRecipe);
      model.Add("recipeIngredients", selectedRecipe);
      model.Add("allCategories", allCategories);
      model.Add("allTags", allCategories);
      model.Add("allIngredients", allCategories);
      return View(model);
    }

    [HttpGet("/recipes/{id}/categories/new")]
    public ActionResult CreateCategoryForm()
    {
      return View("~/Views/Categories/CreateForm.cshtml");
    }

    [HttpPost("/recipes/{recipeId}/categories/new")]
    public ActionResult AddCategory(int recipeId)
    {
      Recipe recipe = Recipe.Find(recipeId);
      Category category = Category.Find(Int32.Parse(Request.Form["category-id"]));
      recipe.AddCategory(category);
      return RedirectToAction("Index");
    }

    [HttpGet("/recipes/{id}/ingredients/new")]
    public ActionResult CreateIngredientForm()
    {
      return View("~/Views/Ingredients/CreateForm.cshtml");
    }

    [HttpPost("/recipes/{recipeId}/ingredients/new")]
    public ActionResult AddIngredient(int recipeId)
    {
      Recipe recipe = Recipe.Find(recipeId);
      Category category = Category.Find(Int32.Parse(Request.Form["category-id"]));
      recipe.AddCategory(category);
      return RedirectToAction("Index");
    }

    [HttpGet("/recipes/{id}/tags/new")]
    public ActionResult CreateTagForm()
    {
      return View("~/Views/Tags/CreateForm.cshtml");
    }

    [HttpPost("/recipes/{recipeId}/tags/new")]
    public ActionResult AddTag(int recipeId)
    {
      Recipe recipe = Recipe.Find(recipeId);
      Category category = Category.Find(Int32.Parse(Request.Form["category-id"]));
      recipe.AddCategory(category);
      return RedirectToAction("Index");
    }

    // [HttpGet("/recipes/{recipeId}/update")]
    // public ActionResult UpdateForm(int recipeId)
    // {
    //    Recipe thisRecipe = Recipe.Find(recipeId);
    //    return View("update", thisRecipe);
    // }
    //
    // [HttpPost("/recipes/{recipeId}/update")]
    // public ActionResult Update(int recipeId)
    // {
    //   Recipe thisRecipe = Recipe.Find(recipeId);
    //   thisRecipe.Edit(Request.Form["new-recipe-name"], Request.Form["new-recipe-number"]);
    //   return RedirectToAction("Index");
    // }
    //
    // [HttpGet("/recipes/{recipeid}/delete")]
    // public ActionResult DeleteOne(int recipeId)
    // {
    //   Recipe thisRecipe = Recipe.Find(recipeId);
    //   thisRecipe.Delete();
    //   return RedirectToAction("Index");
    // }
  }
}
