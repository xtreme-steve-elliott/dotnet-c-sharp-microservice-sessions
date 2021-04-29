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
            _noteServiceMock.VerifyNoOtherCalls();
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

        [Fact]
        public async Task GetByIdAsync_ShouldCall_NoteService_GetByIdAsync()
        {
            const long id = 1L;
            await _subject.GetByIdAsync(id);
            _noteServiceMock.Verify(nsm => nsm.GetByIdAsync(id), Times.Once);
            _noteServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_WhenNoNoteFound_ShouldReturnNotFound()
        {
            _noteServiceMock
                .Setup(nsm => nsm.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(null as Note);

            var response = await _subject.GetByIdAsync(0L);
            var result = response?.Result;
            result.Should().NotBeNull().And.BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetByIdAsync_WhenNoteFound_ShouldReturnOk_WithNote()
        {
            const long id = 1L;
            var initial = new Note { Id = id, Body = "Note 1" };
            var expected = initial;

            _noteServiceMock
                .Setup(nsm => nsm.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(initial);

            var response = await _subject.GetByIdAsync(id);
            var result = response?.Result;
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();

            var value = result.As<OkObjectResult>().Value;
            value.Should().NotBeNull().And.BeAssignableTo<Note>();

            var actual = value.As<Note>();
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddAsync_WhenInvalidNote_ShouldNotCall_NoteService()
        {
            await _subject.AddAsync(null);
            _noteServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddAsync_WhenInvalidNote_ShouldReturnBadRequest()
        {
            var response = await _subject.AddAsync(null);
            response?.Result.Should().NotBeNull().And.BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task AddAsync_WhenValidNote_ShouldCall_NoteService_AddAsync()
        {
            var toAdd = new Note { Body = "Note 1" };
            var initial = new Note { Id = 1L, Body = toAdd.Body };

            _noteServiceMock
                .Setup(nsm => nsm.AddAsync(It.IsAny<Note>()))
                .ReturnsAsync(initial);

            await _subject.AddAsync(toAdd);
            _noteServiceMock.Verify(nsm => nsm.AddAsync(toAdd));
            _noteServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddAsync_WhenValidNote_ShouldReturnCreatedAtRoute()
        {
            var toAdd = new Note { Body = "Note 1" };
            var initial = new Note { Id = 1L, Body = toAdd.Body };
            var expected = initial;
            
            _noteServiceMock
                .Setup(nsm => nsm.AddAsync(It.IsAny<Note>()))
                .ReturnsAsync(initial);

            var response = await _subject.AddAsync(toAdd);
            var result = response?.Result;
            result.Should().NotBeNull().And.BeOfType<CreatedAtRouteResult>();

            var castResult = result.As<CreatedAtRouteResult>();
            castResult.RouteName.Should().Be(NotesController.GetByIdRouteName);
            castResult.RouteValues.Should().Contain("id", expected.Id);
            castResult.Value.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldCall_NoteService_DeleteByIdAsync()
        {
            const long id = 1L;
            await _subject.DeleteByIdAsync(id);
            _noteServiceMock.Verify(nsm => nsm.DeleteByIdAsync(id));
            _noteServiceMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenReceivesFalse_ShouldReturnNotFound()
        {
            _noteServiceMock
                .Setup(nsm => nsm.DeleteByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(false);

            var response = await _subject.DeleteByIdAsync(1L);
            response.Should().NotBeNull().And.BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenReceivesTrue_ShouldReturnOk()
        {
            _noteServiceMock
                .Setup(nsm => nsm.DeleteByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(true);

            var response = await _subject.DeleteByIdAsync(1L);
            response.Should().NotBeNull().And.BeOfType<OkResult>();
        }
    }
}
