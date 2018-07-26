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
        private readonly ITrustedPersonsImageService trustedPersonsImageService;

        public ImagesController(ITrustedPersonsStorage trustedPersonsStorage, ITrustedPersonsImageService trustedPersonsImageService)
        {
            this.trustedPersonsStorage = trustedPersonsStorage;
            this.trustedPersonsImageService = trustedPersonsImageService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Stream), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get([FromQuery]string path, [FromQuery]string name)
        {
            var imageBytes = await this.trustedPersonsStorage.GetMediaContentAsync(path, name);
            var jpgBytes = this.trustedPersonsImageService.ConvertImageToJpg(imageBytes); // NOTE: We only need this because there are other file formats in storage right now!
            return File(jpgBytes, "image/jpg", $"{name}.jpg");
        }
    }
}