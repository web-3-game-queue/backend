using GameQueue.Core.Commands.Maps;
using Microsoft.AspNetCore.Mvc;

namespace GameQueue.Backend.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MapController : ControllerBase
{
    [HttpPost]
    public void AddMap([FromBody] AddMapCommand map)
    {
        Console.WriteLine(map);
    }
}
