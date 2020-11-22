﻿namespace CookingHub.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookingHub.Models.InputModels.AdministratorInputModels.Recipes;
    using CookingHub.Models.ViewModels.Recipes;
    using CookingHub.Services.Data.Contracts;
    using Microsoft.AspNetCore.Mvc;

    public class RecipeController : AdministrationController
    {
        private readonly IRecipesService recipesService;

        public RecipeController(IRecipesService recipesService)
        {
            this.recipesService = recipesService;

        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecipesCreateInputModel recipeCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(recipeCreateInputModel);
            }

            await this.recipesService.CreateAsync(recipeCreateInputModel);
            return this.RedirectToAction("GetAll", "Recipes", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categoryToEdit = await this.recipesService
                .GetViewModelByIdAsync<RecipesEditViewModel>(id);

            return this.View(categoryToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RecipesEditViewModel recipeEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(recipeEditViewModel);
            }

            await this.recipesService.EditAsync(recipeEditViewModel);
            return this.RedirectToAction("GetAll", "Recipes", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var recipesToDelete = await this.recipesService.GetViewModelByIdAsync<RecipesDetailsViewModel>(id);

            return this.View(recipesToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(RecipesDetailsViewModel recipesDetailsViewModel)
        {
            await this.recipesService.DeleteByIdAsync(recipesDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "Recipes", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll()
        {
            var recipes = await this.recipesService.GetAllRecipesAsync<RecipesDetailsViewModel>();
            return this.View(recipes);
        }
    }
}