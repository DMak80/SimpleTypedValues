using Microsoft.AspNetCore.Mvc;

namespace SimpleTypedValue.AspNet.Test;

public class SomeDto
{
    [FromRoute]
    public LongTypedValue Id { get; set; }
}