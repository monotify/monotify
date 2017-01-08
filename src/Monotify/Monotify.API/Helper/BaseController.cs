using Microsoft.AspNetCore.Mvc;
using Monotify.Models.Base;

namespace Monotify.API.Helper
{
    [Route("v1/[controller]")]
    public class BaseController : Controller
    {
        public MonoReturn Error(string message = default(string), string internalMessage = default(string), MonoStatusCode code = MonoStatusCode.BadRequest)
        {
            return new MonoReturn
            {
                Code = code,
                Message = message,
                InternalMessage = internalMessage,
                Success = false
            };
        }

        public MonoReturn Success(object data = default(object), string message = default(string), string internalMessage = default(string))
        {
            return new MonoReturn
            {
                Success = true,
                Code = MonoStatusCode.Success,
                Message = message,
                InternalMessage = internalMessage,
                Data = data
            };
        }
    }
}