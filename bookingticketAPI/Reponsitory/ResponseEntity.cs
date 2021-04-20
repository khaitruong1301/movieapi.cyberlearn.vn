using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bookingticketAPI.Reponsitory
{
    public class ResponseEntity : IActionResult
    {
        public int StatusCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; set; }

        public object Content { get; set; }

        public DateTime DateTime { get; set; }
        public object MessageConstants { get; private set; }

        public ResponseEntity(int statusCode, object content = null, string message = null)
        {
            StatusCode = statusCode;
            Content = content;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            DateTime = DateTime.Now;
        }

        public async System.Threading.Tasks.Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = StatusCode;
            await new ObjectResult(this).ExecuteResultAsync(context);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 400:
                    return MessageConstant.MESSAGE_ERROR_400;

                case 404:
                    return MessageConstant.MESSAGE_ERROR_404;
                    
                case 500:
                    return MessageConstant.MESSAGE_ERROR_500;

                default:
                    return null;
            }
        }
    }
}
