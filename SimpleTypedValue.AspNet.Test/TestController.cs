using Microsoft.AspNetCore.Mvc;

namespace SimpleTypedValue.AspNet.Test;

[Route("[controller]")]
public class TestController
{
    [HttpGet("{id}")]
    public IActionResult Get(SomeDto dto)
    {
        return new OkObjectResult(dto.Id);
    }
}