﻿namespace CookingHub.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using CookingHub.Data.Models;
    using CookingHub.Models.InputModels.AdministratorInputModels.Recipes;
    using CookingHub.Models.ViewModels.Categories;
    using CookingHub.Models.ViewModels.Recipes;
    using CookingHub.Services.Data.Contracts;


    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class RecipesController : AdministrationController
    {
        private readonly IRecipesService recipesService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<CookingHubUser> userManager;

        public RecipesController(
            IRecipesService recipesService,
            ICategoriesService categoriesService,
            UserManager<CookingHubUser> userManager)
        {
            this.recipesService = recipesService;
            this.categoriesService = categoriesService;
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Create()
        {
            var categories = await this.categoriesService
                .GetAllCategoriesAsync<CategoryDetailsViewModel>();

            var model = new RecipesCreateInputModel
            {
                Categories = categories,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecipesCreateInputModel recipesCreateInputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (!this.ModelState.IsValid)
            {
                var categories = await this.categoriesService
                  .GetAllCategoriesAsync<CategoryDetailsViewModel>();
                recipesCreateInputModel.Categories = categories;
                return this.View(recipesCreateInputModel);
            }

            await this.recipesService.CreateAsync(recipesCreateInputModel, user.Id);
            return this.RedirectToAction("GetAll", "Recipes", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var recipesToEdit = await this.recipesService
                .GetViewModelByIdAsync<RecipesEditViewModel>(id);
            var categories = await this.categoriesService
                  .GetAllCategoriesAsync<CategoryDetailsViewModel>();
            recipesToEdit.Categories = categories;

            return this.View(recipesToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RecipesEditViewModel recipesEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(recipesEditViewModel);
            }

            await this.recipesService.EditAsync(recipesEditViewModel);
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