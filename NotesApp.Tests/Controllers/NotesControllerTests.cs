using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NotesApp.Controllers;
using NotesApp.Models;
using Xunit;

namespace NotesApp.Tests.Controllers
{
    public class NotesControllerTests
    {
        private readonly NotesController _subject;

        public NotesControllerTests()
        {
            _subject = new NotesController();
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOk_WithNoteList()
        {
            var expected = new List<Note>
            {
                new () { Id = 1L, Body = "Note 1" },
                new () { Id = 2L, Body = "Note 2" }
            };

            var response = await _subject.GetAllAsync();
            var result = response?.Result;
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var value = result.As<OkObjectResult>().Value;
            value.Should().NotBeNull().And.BeAssignableTo<IEnumerable<Note>>();

            var actual = value.As<IEnumerable<Note>>();
            actual.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }
    }
}
