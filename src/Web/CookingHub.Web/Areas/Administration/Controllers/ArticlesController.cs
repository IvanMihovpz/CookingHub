﻿namespace CookingHub.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;
    using CookingHub.Data.Models;
    using CookingHub.Models.InputModels.AdministratorInputModels.Articles;
    using CookingHub.Models.ViewModels.Articles;
    using CookingHub.Models.ViewModels.Categories;
    using CookingHub.Services.Data.Contracts;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ArticlesController : AdministrationController
    {
        private readonly IArticlesService articlesService;
        private readonly ICategoriesService categoriesService;
        private readonly UserManager<CookingHubUser> userManager;

        public ArticlesController(
            IArticlesService articlesService,
            ICategoriesService categoriesService,
            UserManager<CookingHubUser> userManager)
        {
            this.articlesService = articlesService;
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

            var model = new ArticleCreateInputModel
            {
                Categories = categories,
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticleCreateInputModel articleCreateInputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (!this.ModelState.IsValid)
            {
                var categories = await this.categoriesService
                   .GetAllCategoriesAsync<CategoryDetailsViewModel>();
                articleCreateInputModel.Categories = categories;
                return this.View(articleCreateInputModel);
            }

            await this.articlesService.CreateAsync(articleCreateInputModel, user.Id);
            return this.RedirectToAction("GetAll", "Articles", new { area = "Administration" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var categories = await this.categoriesService
                  .GetAllCategoriesAsync<CategoryDetailsViewModel>();

            var articleToEdit = await this.articlesService
                .GetViewModelByIdAsync<ArticleEditViewModel>(id);

            articleToEdit.Categories = categories;

            return this.View(articleToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ArticleEditViewModel articleEditViewModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (!this.ModelState.IsValid)
            {
                var categories = await this.categoriesService
                   .GetAllCategoriesAsync<CategoryDetailsViewModel>();
                articleEditViewModel.Categories = categories;
                return this.View(articleEditViewModel);
            }

            await this.articlesService.EditAsync(articleEditViewModel, user.Id);
            return this.RedirectToAction("GetAll", "Articles", new { area = "Administration" });
        }

        public async Task<IActionResult> Remove(int id)
        {
            var articlesToDelete = await this.articlesService.GetViewModelByIdAsync<ArticlesDetailsViewModel>(id);

            return this.View(articlesToDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Remove(ArticlesDetailsViewModel articlesDetailsViewModel)
        {
            await this.articlesService.DeleteByIdAsync(articlesDetailsViewModel.Id);
            return this.RedirectToAction("GetAll", "Articles", new { area = "Administration" });
        }

        public async Task<IActionResult> GetAll()
        {
            var articles = await this.articlesService.GetAllArticlesAsync<ArticlesDetailsViewModel>();
            return this.View(articles);
        }
    }
}
