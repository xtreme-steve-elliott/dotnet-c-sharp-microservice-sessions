using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NotesApp.IntegrationTests.Fixtures;
using NotesApp.Models;
using Xunit;

namespace NotesApp.IntegrationTests.Controllers
{
    public class NotesControllerTests : IDisposable, IClassFixture<TestFixture<TestStartup>>
    {
        private const string NotesEndpoint = "/api/notes";
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

        [Fact]
        public async Task Get_Async_ShouldReturnOk_WithNoteList()
        {
            var initial = new List<Note>
            {
                new () { Body = "Note 1" },
                new () { Body = "Note 2" }
            };
            var expected = new List<Note>
            {
                new () { Body = initial[0].Body },
                new () { Body = initial[1].Body }
            };

            {
                await _client.PostAsJsonAsync(NotesEndpoint, initial[0]);
                await _client.PostAsJsonAsync(NotesEndpoint, initial[1]);
            }

            var response = await _client.GetAsync(NotesEndpoint);
            response.EnsureSuccessStatusCode();

            var actual = await response.Content.ReadFromJsonAsync<Note[]>();
            actual.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected, opts => opts.Excluding(n => n.Id));
        }

        [Fact]
        public async Task Get_Async_ById_WhenInvalidId_ShouldReturnNotFound()
        {
            var response = await _client.GetAsync($"{NotesEndpoint}/0");
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_Async_ById_WhenValidId_ShouldReturnNote()
        {
            var initial = new List<Note>
            {
                new () { Body = "Note 1" },
                new () { Body = "Note 2" }
            };
            var expected = new Note { Body = initial[0].Body };

            var postResponse = await _client.PostAsJsonAsync(NotesEndpoint, initial);
            postResponse.EnsureSuccessStatusCode();

            var uri = postResponse.Headers.Location;
            var parsedId = long.TryParse(uri?.Segments.Last(), out var id);
            if (parsedId)
            {
                expected.Id = id;
            }

            var getResponse = await _client.GetAsync(uri);
            getResponse.EnsureSuccessStatusCode();

            var actual = await getResponse.Content.ReadFromJsonAsync<Note>();
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task Post_Async_WhenInvalidNote_ShouldReturnBadRequest()
        {
            var response = await _client.PostAsJsonAsync<Note>(NotesEndpoint, null);
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_Async_WhenValidNote_ShouldReturnCreatedAtRoute()
        {
            var initial = new Note { Body = "Note 1" };
            var expected = new Note { Body = initial.Body };

            var response = await _client.PostAsJsonAsync(NotesEndpoint, initial);
            response.EnsureSuccessStatusCode();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var actual = await response.Content.ReadFromJsonAsync<Note>();
            actual.Should().NotBeNull().And.BeEquivalentTo(expected, opts => opts.Excluding(n => n.Id));

            var uri = response.Headers.Location;
            uri?.AbsolutePath.Should().NotBeNull().And.EndWith($"{NotesEndpoint}/{actual?.Id}");
        }

        [Fact]
        public async Task Delete_Async_ById_WhenInvalidId_ShouldReturnNotFound()
        {
            var response = await _client.DeleteAsync($"{NotesEndpoint}/0");
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_Async_ById_WhenValidId_ShouldReturnOk_AndDeleteNote()
        {
            var initial = new Note { Body = "Note 1" };

            var postResponse = await _client.PostAsJsonAsync(NotesEndpoint, initial);
            postResponse.EnsureSuccessStatusCode();

            var uri = postResponse.Headers.Location;

            var deleteResponse = await _client.DeleteAsync(uri);
            deleteResponse.EnsureSuccessStatusCode();
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _client.GetAsync(uri);
            getResponse.IsSuccessStatusCode.Should().BeFalse();
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
