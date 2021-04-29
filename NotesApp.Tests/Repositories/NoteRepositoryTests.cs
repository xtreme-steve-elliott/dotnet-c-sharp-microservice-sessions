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
    }
}
