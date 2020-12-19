using Microsoft.AspNetCore.Mvc;
namespace Digital.Identity.Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [ProducesResponseType(500)]
    [ProducesResponseType(401)]
    public class AdminControllerBase: ControllerBase
    {
        
    }
}
