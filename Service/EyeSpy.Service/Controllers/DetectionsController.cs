using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExeSpy.Service.Common;
using ExeSpy.Service.Common.Abstractions;
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

        public DetectionsController(ITrustedPersonsService trustedPersonsService)
        {
            this.trustedPersonsService = trustedPersonsService;
        }

        // NOTE: In future, do we track all historic detections?
        [HttpPost]
        public async Task <IActionResult> Post()
        {
            var trustedPerson = await this.trustedPersonsService.DetectIfPersonIsTrustedAsync(this.Request.Body.ToBytes());

            //if (!trustedPerson)
            //    // TODO: Store in blob storage and raise alert!

            return new OkObjectResult(trustedPerson); ;
        }
    }
}