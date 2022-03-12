using TimCodes.ApiAbstractions.Http.Constants;
using TimCodes.ApiAbstractions.Http.Extensions;
using TimCodes.ApiAbstractions.Http.Sample.ApiClient;
using TimCodes.ApiAbstractions.Http.Sample.Models.Responses;

namespace TimCodes.ApiAbstractions.Http.Sample.UnitTests;

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
}
