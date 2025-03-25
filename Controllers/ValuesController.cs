using AspNetOnLambda.Services;
using Microsoft.AspNetCore.Mvc;

namespace AspNetOnLambda.Controllers;

[Route("api/[controller]")]
public class ValuesController : ControllerBase
{
    private readonly IRandomNumbersService _randomNumbersService;

    public ValuesController(IRandomNumbersService randomNumbersService)
    {
        _randomNumbersService = randomNumbersService;
    }
    // GET api/values
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }
    
    [HttpGet]
    [Route("rand")]
    public async Task<int> GetRandom()
    {
        var result = await _randomNumbersService.GetRandomNumber();
        return result;
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody]string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}