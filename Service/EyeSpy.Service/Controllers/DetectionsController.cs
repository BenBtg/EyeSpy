using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using EyeSpy.Service.Common;
using EyeSpy.Service.Common.Abstractions;
using EyeSpy.Service.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EyeSpy.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/detections")]
    [Authorize]
    public class DetectionsController : Controller
    {
        private readonly ITrustedPersonsService trustedPersonsService;
        private readonly ITrustedPersonsStorage trustedPersonsStorage;

        public DetectionsController(ITrustedPersonsService trustedPersonsService, ITrustedPersonsStorage trustedPersonsStorage)
        {
            this.trustedPersonsService = trustedPersonsService;
            this.trustedPersonsStorage = trustedPersonsStorage;
        }

        // NOTE: In future, do we track all historic detections?
        [HttpPost]
        public async Task <IActionResult> Post()
        {
            byte[] detectionImageData = this.Request.Body.ToBytes();

            if (detectionImageData?.Length <= 0)
                return new BadRequestObjectResult($"Binary body payload must not be empty");

            var trustedPerson = await this.trustedPersonsService.DetectIfPersonIsTrustedAsync(detectionImageData);

            if (!trustedPerson)
            {
                // TODO: Raise alert!    
                var detection = await trustedPersonsStorage.CreateDetectionAsync(new BaseDetection { Id = Guid.NewGuid().ToString().ToLower() }, detectionImageData);
            }

            return new OkObjectResult(trustedPerson); ;
        }

        // Need a Get method here to retrieve detection from table by id i.e. deep link provided in notification

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var detections = await this.trustedPersonsStorage.GetDetectionsAsync();
            return new JsonResult(detections);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return new BadRequestObjectResult($"Parameter {nameof(id)} is missing");

            var detection = await this.trustedPersonsStorage.GetDetectionByIdAsync(id);

            if (detection == null)
                return new NotFoundResult();

            return new JsonResult(detection);
        }
    }
}