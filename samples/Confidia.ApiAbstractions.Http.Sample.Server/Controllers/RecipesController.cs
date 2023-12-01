using Microsoft.AspNetCore.Mvc;
using Confidia.ApiAbstractions.Http.Sample.Business;
using Confidia.ApiAbstractions.Http.Sample.Models.Responses;
using Confidia.ApiAbstractions.Http.Server;

namespace Confidia.ApiAbstractions.Http.SampleServer.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecipesController(IRecipeService recipeService) : ControllerBase
{
    private readonly IRecipeService _recipeService = recipeService;

    [HttpGet("")]
    public async Task<IActionResult> GetAllAsync()
    {
        RecipeListResponse message = await _recipeService.GetAllAsync();

        return this.HttpApiResult(message);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        RecipeResponse message = await _recipeService.GetAsync(id);

        return this.HttpApiResult(message);
    }
}
