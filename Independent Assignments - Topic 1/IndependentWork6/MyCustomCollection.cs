using System.Collections;
using System.Collections.Generic;

namespace IndependentWork5
{
    /// <summary>
    /// Власна колекція, що реалізує IEnumerable<string> для поліморфного перебору через foreach.
    /// </summary>
    public class MyCustomCollection : IEnumerable<string>
    {
        private readonly List<string> _items = new List<string>();

        public void Add(string item)
        {
            _items.Add(item);
        }

        // Реалізація узагальненого GetEnumerator
        public IEnumerator<string> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        // Неузагальнена реалізація GetEnumerator для підтримки класичного IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
