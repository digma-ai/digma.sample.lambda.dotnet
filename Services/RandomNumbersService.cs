namespace AspNetOnLambda.Services;

public interface IRandomNumbersService
{
    Task<int> GetRandomNumber();
}

public class RandomNumbersService : IRandomNumbersService
{
    private readonly HttpClient _httpClient = new();
    
    public async Task<int> GetRandomNumber()
    {
        var result = await _httpClient.GetFromJsonAsync<int[]>("http://www.randomnumberapi.com/api/v1.0/random?min=100&max=1000&count=1");
        return result[0];
    }
}