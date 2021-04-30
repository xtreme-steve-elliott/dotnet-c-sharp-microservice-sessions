using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NotesApp.Models;
using NotesApp.Repositories;
using Xunit;

namespace NotesApp.Tests.Repositories
{
    public class NoteRepositoryTests : IDisposable
    {
        private static int _testDbIndex;
        private readonly NotesAppDbContext _dbContext;
        private readonly NoteRepository _subject;

        public NoteRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<NotesAppDbContext>()
                .UseInMemoryDatabase($"{nameof(NoteRepositoryTests)}_tests_{_testDbIndex++}")
                .Options;
            _dbContext = new NotesAppDbContext(dbContextOptions);
            _subject = new NoteRepository(_dbContext);
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Fact]
        public async Task GetAllAsync_WhenNoNotes_ShouldReturnEmptyList()
        {
            var response = await _subject.GetAllAsync();
            response.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_WhenNotes_ShouldReturnNoteList()
        {
            var initial = new List<Note>
            {
                new () { Id = 1L, Body = "Note 1" },
                new () { Id = 2L, Body = "Note 2" }
            };
            var expected = initial;
            {
                await _dbContext.Notes.AddRangeAsync(initial);
                await _dbContext.SaveChangesAsync();
            }

            var response = await _subject.GetAllAsync();
            response.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetByIdAsync_WhenInvalidId_ShouldReturnNull()
        {
            var actual = await _subject.GetByIdAsync(0L);
            actual.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_WhenValidId_ShouldReturnNote()
        {
            const long id = 1L;
            var initial = new List<Note>
            {
                new () { Id = id, Body = "Note 1" },
                new () { Id = 2L, Body = "Note 2" }
            };
            var expected = new Note { Id = initial[0].Id, Body = initial[0].Body };
            {
                await _dbContext.Notes.AddRangeAsync(initial);
                await _dbContext.SaveChangesAsync();
            }

            var actual = await _subject.GetByIdAsync(id);
            actual.Should().NotBeNull().And.BeEquivalentTo(expected);
        }

        [Fact]
        public async Task AddAsync_ShouldSaveNote()
        {
            var toAdd = new Note { Body = "Note 1" };
            var expected = new List<Note>
            {
                new () { Body = toAdd.Body }
            };

            _dbContext.Notes.Should().NotBeNull().And.BeEmpty();

            await _subject.AddAsync(toAdd);

            _dbContext.Notes.Should().NotBeNullOrEmpty();
            _dbContext.Notes.Should().BeEquivalentTo(expected, opts => opts.Excluding(n => n.Id));
        }

        [Fact]
        public async Task AddAsync_ShouldReturnSavedNote()
        {
            var toAdd = new Note { Body = "Note 1" };
            var expected = new Note { Body = toAdd.Body };

            var actual = await _subject.AddAsync(toAdd);
            actual.Should().NotBeNull().And.BeEquivalentTo(expected, opts => opts.Excluding(n => n.Id));
            actual.Id.Should().BePositive();
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenInvalidId_ShouldReturnFalse()
        {
            var actual = await _subject.DeleteByIdAsync(1L);
            actual.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenValidId_ShouldReturnTrue()
        {
            const long id = 1L;
            {
                await _dbContext.Notes.AddAsync(new Note {Id = id, Body = "Note 1"});
                await _dbContext.SaveChangesAsync();
            }
            var actual = await _subject.DeleteByIdAsync(id);
            actual.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteByIdAsync_WhenValidId_ShouldDeleteNote()
        {
            const long id = 1L;
            {
                await _dbContext.Notes.AddAsync(new Note {Id = id, Body = "Note 1"});
                await _dbContext.SaveChangesAsync();
            }
            await _subject.DeleteByIdAsync(id);
            _dbContext.Notes.Should().NotBeNull().And.BeEmpty();
        }
    }
}
