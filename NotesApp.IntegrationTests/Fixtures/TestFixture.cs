using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace NotesApp.IntegrationTests.Fixtures
{
    public class TestFixture<TStartup> : WebApplicationFactory<TStartup> where TStartup : TestStartup
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            var builder = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(x =>
                {
                    x.UseStartup<TStartup>();
                });
            return builder;
        }
    }
}
