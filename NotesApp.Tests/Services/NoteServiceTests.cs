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

        [Fact]
        public async Task GetByIdAsync_ShouldCall_NoteRepository_GetByIdAsync()
        {
            const long id = 1L;
            await _subject.GetByIdAsync(id);
            _noteRepositoryMock.Verify(nrm => nrm.GetByIdAsync(id));
            _noteRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnNote()
        {
            const long id = 1L;
            var initial = new Note { Id = id, Body = "Note 1" };
            var expected = initial;

            _noteRepositoryMock
                .Setup(nrm => nrm.GetByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(initial);

            var actual = await _subject.GetByIdAsync(id);
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddAsync_ShouldCall_NoteRepository_AddAsync()
        {
            var toAdd = new Note { Body = "Note 1" };
            await _subject.AddAsync(toAdd);
            _noteRepositoryMock.Verify(nrm => nrm.AddAsync(toAdd));
            _noteRepositoryMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AddAsync_ShouldReturnNote()
        {
            var toAdd = new Note { Body = "Note 1" };
            var initial = new Note { Id = 1L, Body = toAdd.Body };
            var expected = initial;

            _noteRepositoryMock
                .Setup(nrm => nrm.AddAsync(It.IsAny<Note>()))
                .ReturnsAsync(initial);

            var actual = await _subject.AddAsync(toAdd);
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task DeleteByIdAsync_ShouldCall_NoteRepository_DeleteByIdAsync()
        {
            const long id = 1L;
            await _subject.DeleteByIdAsync(id);
            _noteRepositoryMock.Verify(nrm => nrm.DeleteByIdAsync(id));
            _noteRepositoryMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeleteByIdAsync_ShouldReturnBool(bool initial)
        {
            var expected = initial;
            _noteRepositoryMock
                .Setup(nrm => nrm.DeleteByIdAsync(It.IsAny<long>()))
                .ReturnsAsync(initial);

            var actual = await _subject.DeleteByIdAsync(1L);
            actual.Should().Be(expected);
        }
    }
}
