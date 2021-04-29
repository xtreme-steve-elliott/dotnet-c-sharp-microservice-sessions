using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NotesApp.Models;
using NotesApp.Repositories;
using NotesApp.Services;
using Xunit;

namespace NotesApp.Tests.Services
{
    public class NoteServiceTests
    {
        private readonly Mock<IModelRepository<Note>> _noteRepositoryMock;
        private readonly NoteService _subject;

        public NoteServiceTests()
        {
            _noteRepositoryMock = new Mock<IModelRepository<Note>>();
            _subject = new NoteService(_noteRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldCall_NoteRepository_GetAllAsync()
        {
            await _subject.GetAllAsync();
            _noteRepositoryMock.Verify(nrm => nrm.GetAllAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnNoteList()
        {
            var initial = new List<Note>
            {
                new () { Id = 1L, Body = "Note 1" },
                new () { Id = 2L, Body = "Note 2" }
            };
            var expected = initial;

            _noteRepositoryMock
                .Setup(nrm => nrm.GetAllAsync())
                .ReturnsAsync(initial);

            var actual = await _subject.GetAllAsync();
            actual.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }
    }
}
