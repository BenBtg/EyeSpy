using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EyeSpy.Service.Common;
using EyeSpy.Service.Common.Abstractions;
using EyeSpy.Service.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EyeSpy.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/trustedpersons")]
    [Authorize]
    public class TrustedPersonsController : Controller
    {
        private readonly ITrustedPersonsFaceRecognition trustedPersonsService;
        private readonly ITrustedPersonsStorage trustedPersonsStorage;
        private readonly ITrustedPersonsImageService trustedPersonsImageService;

        public TrustedPersonsController(ITrustedPersonsFaceRecognition trustedPersonsService, ITrustedPersonsStorage trustedPersonsStorage, ITrustedPersonsImageService trustedPersonsImageService)
        {
            this.trustedPersonsService = trustedPersonsService;
            this.trustedPersonsStorage = trustedPersonsStorage;
            this.trustedPersonsImageService = trustedPersonsImageService;
        }

        // TODO: Allow upload of one or more pictures - MVP: Just accept one
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType(typeof(TrustedPerson), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromQuery]string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new BadRequestObjectResult($"Query parameter {nameof(name)} is missing");

            byte[] trustedPersonImageData = this.Request.Body.ToBytes();

            if (trustedPersonImageData?.Length <= 0)
                return new BadRequestObjectResult($"Binary body payload must not be empty");

            trustedPersonImageData = this.trustedPersonsImageService.ConvertImageToJpg(trustedPersonImageData);

            // Create the trusted person in Face API (to get the ID)
            var baseTrustedPerson = await trustedPersonsService.CreateTrustedPersonAsync(name, trustedPersonImageData);

            // Add the trusted person image to blob storage (using the ID from the Face Api) trusted person
            var trustedPerson = await this.trustedPersonsStorage.CreateTrustedPersonAsync(baseTrustedPerson, trustedPersonImageData);

            // TODO: Roll back any failures here
            if (trustedPerson == null)
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);

            return new JsonResult(trustedPerson);
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TrustedPerson>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var trustedPersons = await this.trustedPersonsStorage.GetTrustedPersonsAsync();
            return new JsonResult(trustedPersons);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(TrustedPerson), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return new BadRequestObjectResult($"Parameter {nameof(id)} is missing");

            var trustedPerson = await this.trustedPersonsStorage.GetTrustedPersonByIdAsync(id);

            if (trustedPerson == null)
                return new NotFoundResult();

            return new JsonResult(trustedPerson);
        }
    }
}