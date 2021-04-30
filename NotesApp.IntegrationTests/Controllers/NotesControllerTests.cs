using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.IntegrationTests.Fixtures;
using NotesApp.Models;
using Xunit;

namespace NotesApp.IntegrationTests.Controllers
{
    public class NotesControllerTests : IDisposable, IClassFixture<TestFixture<TestStartup>>
    {
        private readonly TestFixture<TestStartup> _fixture;
        private readonly HttpClient _client;

        public NotesControllerTests(TestFixture<TestStartup> fixture)
        {
            _fixture = fixture;
            _client = _fixture.CreateClient();
        }
        
        public void Dispose()
        {
            var context = _fixture.Services.GetRequiredService<NotesAppDbContext>();
            context.Notes.RemoveRange(context.Notes);
            context.SaveChanges();
        }
    }
}
