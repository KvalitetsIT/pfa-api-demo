using KitNugs.Configuration;
using KitNugs.Repository;
using KitNugs.Services.Model;

namespace KitNugs.Services
{
    public class HelloService : IHelloService
    {
        private readonly string _configurationValue;
        private readonly ILogger<HelloService> _logger;

        public HelloService(IServiceConfiguration configuration, ILogger<HelloService> logger)
        {
            _configurationValue = configuration.GetConfigurationValue(ConfigurationVariables.TEST_VAR);
            _logger = logger;
        }

        public async Task<HelloModel> BusinessLogic(string name)
        {

            _logger.LogDebug("Doing business logic.");

            return new HelloModel()
            {
                Name = name,
                Now = DateTime.Now,
                FromConfiguration = _configurationValue,
            };
        }
    }
}
