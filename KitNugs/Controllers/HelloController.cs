using KitNugs.Services;
using Microsoft.AspNetCore.Mvc;

namespace KitNugs.Controllers
{
    public class HelloController : HelloControllerBase
    {
        private readonly ILogger<HelloController> _logger;
        private readonly IHelloService _helloService;

        public HelloController(ILogger<HelloController> logger, IHelloService helloService)
        {
            _logger = logger;
            _helloService = helloService;
        }

        public override async Task<HelloResponse> Hello([FromQuery] string name)
        {
            _logger.LogInformation("Entering GET!");
            var businessResult = await _helloService.BusinessLogic(name);

            String ip;
            var headers = Request.Headers.ToList();
            if (headers.Exists((kvp) => kvp.Key == "X-Forwarded-For"))
            {
                // when running behind a load balancer you can expect this header
                var header = headers.First((kvp) => kvp.Key == "X-Forwarded-For").Value.ToString();
                // in case the IP contains a port, remove ':' and everything after
                ip = header.Remove(header.IndexOf(':'));
            }
            else
            {
                // this will always have a value (running locally in development won't have the header)
                ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();
            }

            return new HelloResponse { 
                Now = businessResult.Now, 
                Name = businessResult.Name,
                From_configuration = businessResult.FromConfiguration,
                Ip = ip
            };
        }
    }
}
