using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        private readonly ITrustedPersonsFaceRecognition trustedPersonsService;
        private readonly ITrustedPersonsStorage trustedPersonsStorage;
        private readonly ITrustedPersonsNotifications trustedPersonsNotifications;

        public DetectionsController(ITrustedPersonsFaceRecognition trustedPersonsService, ITrustedPersonsStorage trustedPersonsStorage, ITrustedPersonsNotifications trustedPersonsNotifications)
        {
            this.trustedPersonsService = trustedPersonsService;
            this.trustedPersonsStorage = trustedPersonsStorage;
            this.trustedPersonsNotifications = trustedPersonsNotifications;
        }

        // NOTE: In future, do we track all historic detections?
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task <IActionResult> Post()
        {
            byte[] detectionImageData = this.Request.Body.ToBytes();

            if (detectionImageData?.Length <= 0)
                return new BadRequestObjectResult($"Binary body payload must not be empty");

            bool trustedPerson = true;
            Detection detection = null;

            // Execute trusted person check and detection creation in parallel
            List<Task> parallelTasks = new List<Task>
            {
                Task.Run(async () => { trustedPerson = await this.trustedPersonsService.DetectIfPersonIsTrustedAsync(detectionImageData); }),
                Task.Run(async () => { detection = await trustedPersonsStorage.CreateDetectionAsync(new BaseDetection { Id = Guid.NewGuid().ToString().ToLower() }, detectionImageData); })
            };

            await Task.WhenAll(parallelTasks);

            if (!trustedPerson && detection != null)
            { 
                await this.trustedPersonsNotifications.SendDetectionNotificationAsync<DetectionNotification>(new DetectionNotification
                {
                    Title = "Eye spy with my little eye..",
                    Message = "Unrecognized person detected",
                    DetectionId = detection.Id,
                    ImageReference = detection.ImageReference
                });
            }

            return new OkObjectResult(trustedPerson); ;
        }

        // Need a Get method here to retrieve detection from table by id i.e. deep link provided in notification

        [HttpGet]
        [ProducesResponseType(typeof(List<Detection>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get()
        {
            var detections = await this.trustedPersonsStorage.GetDetectionsAsync();
            return new JsonResult(detections);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Detection), (int)HttpStatusCode.OK)]
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