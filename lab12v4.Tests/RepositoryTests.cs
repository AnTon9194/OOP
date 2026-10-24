using System;
using System.Linq;
using lab12v4;
using Xunit;

namespace lab12v4.Tests
{
    public class RepositoryTests
    {
        [Fact]
        public void Add_ShouldInsertItemCorrectly()
        {
            var repo = new InMemoryRepository<Category>();
            var cat = new Category { Id = 1, Name = "Тест" };

            repo.Add(cat);

            Assert.Single(repo.GetAll());
            Assert.Equal("Тест", repo.GetById(1)?.Name);
        }

        [Fact]
        public void GetById_ShouldReturnNull_WhenIdDoesNotExist()
        {
            var repo = new InMemoryRepository<Category>();

            var result = repo.GetById(999);

            Assert.Null(result);
        }

        [Fact]
        public void GetAll_ShouldReturnAllItems()
        {
            var repo = new InMemoryRepository<Category>();
            repo.Add(new Category { Id = 1, Name = "A" });
            repo.Add(new Category { Id = 2, Name = "B" });

            var result = repo.GetAll();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public void Update_ShouldModifyExistingItem()
        {
            var repo = new InMemoryRepository<Category>();
            repo.Add(new Category { Id = 1, Name = "Стара Назва" });

            var updatedCat = new Category { Id = 1, Name = "Нова Назва" };
            repo.Update(updatedCat);

            Assert.Equal("Нова Назва", repo.GetById(1)?.Name);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenItemDoesNotExist()
        {
            var repo = new InMemoryRepository<Category>();
            var nonExisting = new Category { Id = 999, Name = "Помилка" };

            Assert.Throws<InvalidOperationException>(() => repo.Update(nonExisting));
        }

        [Fact]
        public void Delete_ShouldRemoveItem()
        {
            var repo = new InMemoryRepository<Category>();
            repo.Add(new Category { Id = 1, Name = "На Видалення" });

            repo.Delete(1);

            Assert.Empty(repo.GetAll());
        }

        [Fact]
        public void Delete_ShouldThrowException_WhenIdDoesNotExist()
        {
            var repo = new InMemoryRepository<Category>();

            Assert.Throws<InvalidOperationException>(() => repo.Delete(999));
        }
    }
}