using System.IO;
using System.Net;
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
        [ProducesResponseType(typeof(Stream), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery]string path, [FromQuery]string name)
        {
            var imageBytes = await this.trustedPersonsStorage.GetMediaContentAsync(path, name);
            return File(imageBytes, "application/octet-stream");
        }
    }
}