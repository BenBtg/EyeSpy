using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExeSpy.Service.Common.Abstractions;
using EyeSpy.Service.FaceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EyeSpy.Service.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ITrustedPersonsService trustedPersonsService;

        public ValuesController(ITrustedPersonsService trustedPersonsService)
        {
            this.trustedPersonsService = trustedPersonsService;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            // Quick Test
            //var personGroupsService = new PersonGroupsService("https://northeurope.api.cognitive.microsoft.com/face/v1.0/", "28cc27dbba5a4fa397ec824879c32947");
            //var groups = await this.trustedPersonsService.ini();

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
