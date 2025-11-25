using System.Collections.Generic;
using System.Threading.Tasks;
using J3M_BE.Controllers;
using J3M.Models;
using J3M.Services.Http;
using J3M.Shared.DTOs.Allergies;
using J3M.Shared.DTOs.Diets;
using J3M.Shared.MealPlanModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;


namespace MealPlanMvcControllerTests
{
    public class MealPlanMvcControllerTests
    {
        private readonly Mock<IAuthorizedApiClient> _mockApiClient;
        private readonly MealPlanMvcController _controller;

        public MealPlanMvcControllerTests()
        {
            _mockApiClient = new Mock<IAuthorizedApiClient>();

            // Setup default GET responses
            _mockApiClient.Setup(c => c.GetAsync<List<AllergyDto>>("api/meta/allergies"))
                .ReturnsAsync(new List<AllergyDto> { new AllergyDto { AllergyId = 1, AllergyName = "Gluten" } });

            _mockApiClient.Setup(c => c.GetAsync<List<DietDto>>("api/meta/diets"))
                .ReturnsAsync(new List<DietDto> { new DietDto { DietId = 1, DietName = "Vegan" } });

            _controller = new MealPlanMvcController(_mockApiClient.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithMealPlanViewModel()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<MealPlanViewModel>(viewResult.Model);

            Assert.Single(model.Allergies);
            Assert.Single(model.Diets);
            Assert.Null(model.WeeklyPlan);
        }

        [Fact]
        public async Task GenerateWeeklyMenu_NormalRequest_ReturnsViewResult_WithWeeklyPlan()
        {
            // Arrange
            var fakePlan = new WeeklyMealPlanDto
            {
                DaySummaries = new List<DayMealPlanDto>
                {
                    new DayMealPlanDto { Day = "Monday", Meals = new List<MealSlotDto>() }
                },
                ShoppingList = new List<string> { "Tomatoes", "Pasta" }
            };

            _mockApiClient.Setup(c => c.PostAsync<WeeklyMealPlanDto>(
                "api/MealPlan/weekly/ai",
                It.IsAny<MealPlanRequest>()))
                .ReturnsAsync(fakePlan);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext() // no AJAX header
            };

            // Act
            var result = await _controller.GenerateWeeklyMenu(new List<int> { 1 }, new List<int> { 1 });

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<MealPlanViewModel>(viewResult.Model);

            Assert.NotNull(model.WeeklyPlan);
            Assert.Single(model.WeeklyPlan.DaySummaries);
            Assert.Contains("Tomatoes", model.WeeklyPlan.ShoppingList);
        }

        [Fact]
        public async Task GenerateWeeklyMenu_AjaxRequest_ReturnsPartialView()
        {
            // Arrange
            var fakePlan = new WeeklyMealPlanDto
            {
                DaySummaries = new List<DayMealPlanDto>(),
                ShoppingList = new List<string>()
            };

            _mockApiClient.Setup(c => c.PostAsync<WeeklyMealPlanDto>(
                "api/MealPlan/weekly/ai",
                It.IsAny<MealPlanRequest>()))
                .ReturnsAsync(fakePlan);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
            _controller.ControllerContext.HttpContext.Request.Headers["X-Requested-With"] = "XMLHttpRequest";

            // Act
            var result = await _controller.GenerateWeeklyMenu(new List<int>(), new List<int>());

            // Assert
            var partialResult = Assert.IsType<PartialViewResult>(result);
            Assert.Equal("_MealPlanPartial", partialResult.ViewName);
        }

        [Fact]
        public async Task GenerateWeeklyMenu_WhenApiReturnsNull_ReturnsErrorView()
        {
            // Arrange
            _mockApiClient.Setup(c => c.PostAsync<WeeklyMealPlanDto>(
                "api/MealPlan/weekly/ai",
                It.IsAny<MealPlanRequest>()))
                .ReturnsAsync((WeeklyMealPlanDto)null);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            // Act
            var result = await _controller.GenerateWeeklyMenu(new List<int>(), new List<int>());

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);
        }
    }
}
