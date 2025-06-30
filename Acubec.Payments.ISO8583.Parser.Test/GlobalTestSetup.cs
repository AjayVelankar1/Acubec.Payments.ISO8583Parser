using Microsoft.Extensions.DependencyInjection;

namespace Acubec.Payments.ISO8583.Parser.Test
{
    [SetUpFixture]
    public class GlobalTestSetup
    {
        static ServiceProvider Provider;
        protected  ServiceProvider _provider => Provider;

        [OneTimeSetUp]
        public void RunBeforeAnyTests()
        {
            var services = new ServiceCollection();
            services.AddISO8583Parser();
            Provider = services.BuildServiceProvider();
        }

        [OneTimeTearDown]
        public void RunAfterAllTests()
        {
            Provider?.Dispose();
        }
    }
}
