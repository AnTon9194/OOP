using System;

namespace lab5v1
{
    /// <summary>
    /// Клас IntArray — Масив цілих чисел з індексатором та перевантаженими операторами (Варіант 1).
    /// </summary>
    public class IntArray
    {
        private int[] _data;

        public int Length => _data.Length;

        public IntArray(int size)
        {
            if (size < 0)
                throw new ArgumentOutOfRangeException(nameof(size), "Розмір масиву не може бути від'ємним.");
            _data = new int[size];
        }

        public IntArray(params int[] items)
        {
            _data = items ?? throw new ArgumentNullException(nameof(items));
        }

        // Індексатор для доступу до елементів за індексом
        public int this[int index]
        {
            get
            {
                ValidateIndex(index);
                return _data[index];
            }
            set
            {
                ValidateIndex(index);
                _data[index] = value;
            }
        }

        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= _data.Length)
            {
                throw new IndexOutOfRangeException($"Індекс {index} виходить за межі масиву [0..{_data.Length - 1}].");
            }
        }

        // Перевантаження оператора + (поелементне додавання масивів)
        public static IntArray operator +(IntArray a, IntArray b)
        {
            if (a is null || b is null)
                throw new ArgumentNullException(a is null ? nameof(a) : nameof(b));

            int maxLen = Math.Max(a.Length, b.Length);
            int[] result = new int[maxLen];

            for (int i = 0; i < maxLen; i++)
            {
                int valA = i < a.Length ? a[i] : 0;
                int valB = i < b.Length ? b[i] : 0;
                result[i] = valA + valB;
            }

            return new IntArray(result);
        }

        // Перевантаження операторів порівняння == та !=
        public static bool operator ==(IntArray? a, IntArray? b)
        {
            if (ReferenceEquals(a, b)) return true;
            if (a is null || b is null) return false;
            return a.Equals(b);
        }

        public static bool operator !=(IntArray? a, IntArray? b)
        {
            return !(a == b);
        }

        // Перевизначення Equals
        public override bool Equals(object? obj)
        {
            if (obj is not IntArray other) return false;
            if (Length != other.Length) return false;

            for (int i = 0; i < Length; i++)
            {
                if (_data[i] != other._data[i]) return false;
            }

            return true;
        }

        // Перевизначення GetHashCode
        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            foreach (var item in _data)
            {
                hash.Add(item);
            }
            return hash.ToHashCode();
        }

        // Перевизначення ToString
        public override string ToString()
        {
            return "[" + string.Join(", ", _data) + "]";
        }
    }
}
