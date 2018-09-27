using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RecipeBox.Models;
using System;

namespace RecipeBox.Controllers
{
  public class TagsController : Controller
  {
    [HttpGet("/tags")]
    public ActionResult Index()
    {
      List<Tag> allTags = Tag.GetAll();
      return View(allTags);
    }

    [HttpGet("/tags/new")]
    public ActionResult CreateForm()
    {
      return View();
    }

    [HttpPost("/tags")]
    public ActionResult Create()
    {
      Tag newTag = new Tag(Request.Form["tag-name"]);
      newTag.Save();
      return RedirectToAction("Index");
    }

    [HttpGet("/tags/{id}")]
    public ActionResult Details(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Tag selectedTag = Tag.Find(id);
      List<Recipe> tagRecipes = selectedTag.GetRecipes();
      List<Recipe> allRecipes = Recipe.GetAll();
      model.Add("selectedTag", selectedTag);
      model.Add("tagRecipes", tagRecipes);
      model.Add("allRecipes", allRecipes);
      return View(model);
    }

    [HttpGet("/tags/{id}/recipes/new")]
    public ActionResult CreateRecipeForm()
    {
      return View("~/Views/Recipes/CreateForm.cshtml");
    }

    [HttpPost("/tags/{tagId}/recipes/new")]
    public ActionResult AddRecipe(int tagId)
    {
      Tag tag = Tag.Find(tagId);
      Recipe newRecipe = Recipe.Find(Int32.Parse(Request.Form["tag-id"]));
      tag.AddRecipe(newRecipe);
      return RedirectToAction("Details", new {id = tagId});
    }

    // [HttpGet("/tags/{tagId}/update")]
    // public ActionResult UpdateForm(int recipeId)
    // {
    //    Tag thisTag = Tag.Find(recipeId);
    //    return View("update", thisTag);
    // }
    //
    // [HttpPost("/tags/{recipeId}/update")]
    // public ActionResult Update(int recipeId)
    // {
    //   Tag thisTag = Tag.Find(recipeId);
    //   thisTag.Edit(Request.Form["new-recipe-name"], Request.Form["new-recipe-number"]);
    //   return RedirectToAction("Index");
    // }
    //
    // [HttpGet("/tags/{recipeid}/delete")]
    // public ActionResult DeleteOne(int recipeId)
    // {
    //   Tag thisTag = Tag.Find(recipeId);
    //   thisTag.Delete();
    //   return RedirectToAction("Index");
    // }
  }
}
