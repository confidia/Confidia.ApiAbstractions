using Confidia.ApiAbstractions.Http.Constants;
using Confidia.ApiAbstractions.Http.Extensions;
using Confidia.ApiAbstractions.Http.Sample.ApiClient;
using Confidia.ApiAbstractions.Http.Sample.Models.Responses;
using Confidia.ApiAbstractions.Models.Responses;

namespace Confidia.ApiAbstractions.Http.Sample.UnitTests;

[Collection("http")]
public class RecipeApiClientShould
{
    private readonly TestFixture _host;
    private readonly IRecipeApiClient _client;

    public RecipeApiClientShould(TestFixture host)
    {
        _host = host;

        _client = new RecipeApiClient(host.Services.GetRequiredService<ILogger<RecipeApiClient>>(), new HttpClient(TestFixture.MockHttp), _host.Services);
    }

    [Fact]
    public async Task GetAllRecipes()
    {
        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Get, "http://tastyrecipes.com/api/recipes")
            .Respond(MediaTypes.Json, "{\"isSuccess\":true, \"recipes\":[{\"id\":1,\"title\":\"Spag Bol\"}]}");

        var recipes = await _client.GetAllAsync();

        Assert.True(recipes.Success);
        var recipeResult = recipes.ToSpecificResponse<RecipeListResponse>();
        Assert.NotNull(recipeResult);
        Assert.True(recipeResult.IsSuccess);
        Assert.Equal(1, recipeResult.Recipes.First().Id);
        Assert.Equal("Spag Bol", recipeResult.Recipes.First().Title);
    }

    [Fact]
    public async Task NotThrowIfInvalidJson()
    {
        TestFixture.MockHttp.Clear();
        TestFixture.MockHttp.When(HttpMethod.Get, "http://tastyrecipes.com/api/recipes")
            .Respond(MediaTypes.Text, "<html>");

        var recipes = await _client.GetAllAsync();

        Assert.True(!recipes.Success);
        var recipeResult = (ErrorApiResponse)recipes;
        Assert.NotNull(recipeResult);
        Assert.NotNull(recipeResult.Exception);
    }
}
