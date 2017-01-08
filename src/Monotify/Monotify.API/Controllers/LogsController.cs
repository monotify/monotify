using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monotify.API.Helper;
using Monotify.Models;
using Monotify.Models.Base;

namespace Monotify.API.Controllers
{
    public class LogsController : BaseController
    {
        [Produces(typeof(MonoReturn<MonoPagedResult<MonoLog>>))]
        [HttpGet]
        public async Task<MonoReturn> Get()
        {
            var logs = new MonoPagedResult<MonoLog>();
            return Success(data: logs, message: "There is no log to show.");
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<MonoReturn> Get(string id)
        {
            return Error("Log not found.");
        }

        [HttpPost]
        public async Task<MonoReturn> Post([FromBody] MonoLog log)
        {
            return Error("You should pass your log in your response body.");
        }

        [HttpPut]
        public async Task<MonoReturn> Put()
        {
            return Error($"You should pass your log id in your query. For example /logs/{Guid.NewGuid()}");
        }

        [HttpPut("{id}")]
        public async Task<MonoReturn> Put(string id, [FromBody] MonoLog value)
        {
            return Error("You should pass your log id in your query string and log in your response body.");
        }

        [HttpDelete("{id}")]
        public async Task<MonoReturn> Delete(int id)
        {
            return Error("Log not found.");
        }
    }
}