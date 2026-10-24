using System;
using System.Collections.Generic;
using System.Linq;

namespace lab12v4
{
    public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly List<T> _items = new();

        public T? GetById(int id)
        {
            return _items.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return _items;
        }

        public void Add(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            _items.Add(item);
        }

        public void Update(T item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            
            var existing = GetById(item.Id);
            if (existing == null)
            {
                throw new InvalidOperationException($"Елемент з ID {item.Id} не знайдено для оновлення.");
            }

            var index = _items.IndexOf(existing);
            _items[index] = item;
        }

        public void Delete(int id)
        {
            var existing = GetById(id);
            if (existing == null)
            {
                throw new InvalidOperationException($"Елемент з ID {id} не знайдено для видалення.");
            }
            _items.Remove(existing);
        }
    }
}