using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NotesApp.Controllers;
using NotesApp.Models;
using NotesApp.Services;
using Xunit;

namespace NotesApp.Tests.Controllers
{
    public class NotesControllerTests
    {
        private readonly Mock<IModelService<Note>> _noteServiceMock;
        private readonly NotesController _subject;

        public NotesControllerTests()
        {
            _noteServiceMock = new Mock<IModelService<Note>>();
            _subject = new NotesController(_noteServiceMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldCall_NoteService_GetAllAsync()
        {
            await _subject.GetAllAsync();
            _noteServiceMock.Verify(nsm => nsm.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnOk_WithNoteList()
        {
            var initial = new List<Note>
            {
                new () { Id = 1L, Body = "Note 1" },
                new () { Id = 2L, Body = "Note 2" }
            };
            var expected = initial;

            _noteServiceMock
                .Setup(nsm => nsm.GetAllAsync())
                .ReturnsAsync(initial);

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
