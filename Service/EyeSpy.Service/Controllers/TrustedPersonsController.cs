using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExeSpy.Service.Common.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EyeSpy.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/trustedpersons")]
    [Authorize]
    public class TrustedPersonsController : Controller
    {
        // TODO: Allow upload of one or more pictures - MVP: Just accept one
        [HttpPost]
        public async Task<IActionResult> Post([FromQuery]string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new BadRequestObjectResult($"Query parameter {nameof(name)} is missing");

            return await Task.FromResult<OkResult>(new OkResult());
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stubResults = new List<TrustedPerson>
            {
                new TrustedPerson { Id = "0f98b9fd-afe5-4e12-a959-a2063592e285", Name = "Ben Buttigieg", ProfileUrl = "https://media.licdn.com/dms/image/C4D03AQFmGPu7n6vtUA/profile-displayphoto-shrink_800_800/0?e=1538006400&v=beta&t=6-Vztk6Wq0OUBJ03KgrdNg1fGp-TYJElcwJ3-BFyz9o" },
                new TrustedPerson { Id = "0a4ead19-aa7e-4916-b6cc-07fd7e41379b", Name = "Mike Parker", ProfileUrl = "https://media.licdn.com/dms/image/C4D03AQE_FDf80nsZmw/profile-displayphoto-shrink_100_100/0?e=1538006400&v=beta&t=eD9wKwM4LJ6REYxYiaesnCOJ4l1pug71RZAzNS9LtEQ" },
                new TrustedPerson { Id = "bf52c8b3-fe57-4c23-8342-d67e7986f86d", Name = "Alexex Strakh", ProfileUrl = "https://media.licdn.com/dms/image/C5603AQEec7hopx1IYw/profile-displayphoto-shrink_800_800/0?e=1538006400&v=beta&t=uEr30wIQRCvylHMmMGLqgAU5GgdZG21Y8IRHTEOd6sY" }
            };

            return await Task.FromResult<JsonResult>(new JsonResult(stubResults));
        }
    }
}