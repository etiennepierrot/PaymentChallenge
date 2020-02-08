using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PaymentChallenge.WebApi.Helpers
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            this.next = next;
            logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            HttpRequest request = context.Request;
            request.EnableBuffering();

            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);
            request.Body.Seek(0, SeekOrigin.Begin);

            
            //todo we need to filter Cardnumber 
            string message = 
                "Request : " +
                $"Host : {request.Host} " +
                $"Schema : {request.Scheme} " +
                $"Method : {request.Method} " +
                $"Path : {request.Path} " +
                $"Body : {requestBody}";
            
            logger.LogInformation(message);

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await next(context);

                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var response = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                logger.LogInformation($"Response : " +
                                      $"StatusCode : {context.Response.StatusCode} " +
                                      $"Body : {response}");
                
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
    }
}