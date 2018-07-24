using System.Threading.Tasks;
using EyeSpy.Service.Common.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EyeSpy.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/images")]
    [Authorize]
    public class ImagesController : Controller
    {
        private readonly ITrustedPersonsStorage trustedPersonsStorage;

        public ImagesController(ITrustedPersonsStorage trustedPersonsStorage)
        {
            this.trustedPersonsStorage = trustedPersonsStorage;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery]string path, [FromQuery]string name)
        {
            var imageBytes = await this.trustedPersonsStorage.GetMediaContentAsync(path, name);
            return File(imageBytes, "image/png", $"{name}.png"); // TODO: Do this better. Need to confirm and store the type as part of file upload
        }
    }
}