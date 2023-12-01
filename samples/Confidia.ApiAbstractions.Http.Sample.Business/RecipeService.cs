using System.Net;
using Confidia.ApiAbstractions.Http.Sample.Models;
using Confidia.ApiAbstractions.Http.Sample.Models.Responses;
using Confidia.ApiAbstractions.Http.Server;

namespace Confidia.ApiAbstractions.Http.Sample.Business;

public class RecipeService : IRecipeService
{
    private readonly Dictionary<int, Recipe> _recipes = new()
    {
        { 
            1, 
            new Recipe 
            { 
                Id = 1,
                Title = "Spaghetti Meatballs",
                Ingredients = new[] { "Pasta", "Meat" },
                Method = "Put some balls of meat on your pasta"
            } 
        },
        {
            2,
            new Recipe
            {
                Id = 2,
                Title = "Beef Pie",
                Ingredients = new[] { "Pastry", "Beef", "Gravy" },
                Method = "Put some cow in your gravy and then put your newly gravied cow in your pastry"
            }
        }
    };

    public Task<RecipeListResponse> GetAllAsync() 
        => Task.FromResult(HttpApiMessageBuilder.CreateSuccess<RecipeListResponse>(m =>
        {
            m.Recipes = _recipes.Values.ToList();
        }));

    public Task<RecipeResponse> GetAsync(int id)
        => Task.FromResult(
            _recipes.ContainsKey(id) ?
                HttpApiMessageBuilder.CreateSuccess<RecipeResponse>(m =>
                {
                    m.Recipe = _recipes[id];
                }) :
                HttpApiMessageBuilder.CreateFailure<RecipeResponse>(
                    HttpStatusCode.NotFound,
                    "Recipe not found",
                    RecipeError.NotYetCreated));
}
