using IndependentWork7;

namespace IndependentWork7.Tests;

public class CacheTests
{
    // ---------- Конструктор ----------
    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void Constructor_NonPositiveCapacity_Throws(int capacity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Cache<int, string>(capacity));
    }

    // ---------- Add ----------
    [Fact]
    public void Add_NewItem_IsStored()
    {
        var cache = new Cache<string, int>(3);

        cache.Add("a", 1);

        Assert.True(cache.ContainsKey("a"));
        Assert.Equal(1, cache.Count);
    }

    [Fact]
    public void Add_ExistingKey_UpdatesValueWithoutChangingCount()
    {
        var cache = new Cache<string, int>(3);
        cache.Add("a", 1);

        cache.Add("a", 99);

        Assert.Equal(99, cache.Get("a"));
        Assert.Equal(1, cache.Count);
    }

    // ---------- Get ----------
    [Fact]
    public void Get_ExistingKey_ReturnsValue()
    {
        var cache = new Cache<string, string>(2);
        cache.Add("k", "v");

        Assert.Equal("v", cache.Get("k"));
    }

    [Fact]
    public void Get_MissingKey_ThrowsKeyNotFoundException()
    {
        var cache = new Cache<string, string>(2);

        Assert.Throws<KeyNotFoundException>(() => cache.Get("missing"));
    }

    // ---------- Переповнення (FIFO) ----------
    [Fact]
    public void Add_WhenFull_RemovesOldestItem()
    {
        var cache = new Cache<int, string>(3);
        cache.Add(1, "one");
        cache.Add(2, "two");
        cache.Add(3, "three");

        cache.Add(4, "four");

        Assert.False(cache.ContainsKey(1));   // найстаріший видалено
        Assert.True(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
        Assert.True(cache.ContainsKey(4));
        Assert.Equal(3, cache.Count);
    }

    [Fact]
    public void Add_WhenFull_RemovesInInsertionOrder()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "a");
        cache.Add(2, "b");
        cache.Add(3, "c");   // витісняє 1
        cache.Add(4, "d");   // витісняє 2

        Assert.False(cache.ContainsKey(1));
        Assert.False(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
        Assert.True(cache.ContainsKey(4));
    }

    [Fact]
    public void Get_DoesNotAffectEvictionOrder_InFifo()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "a");
        cache.Add(2, "b");

        cache.Get(1);        // у FIFO читання не «оновлює» елемент
        cache.Add(3, "c");

        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
    }

    [Fact]
    public void Add_ExistingKeyWhenFull_DoesNotEvictAnything()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "a");
        cache.Add(2, "b");

        cache.Add(2, "b2");

        Assert.True(cache.ContainsKey(1));
        Assert.Equal("b2", cache.Get(2));
        Assert.Equal(2, cache.Count);
    }

    [Fact]
    public void Capacity_One_KeepsOnlyLastItem()
    {
        var cache = new Cache<int, string>(1);
        cache.Add(1, "a");
        cache.Add(2, "b");

        Assert.False(cache.ContainsKey(1));
        Assert.Equal("b", cache.Get(2));
        Assert.Equal(1, cache.Count);
    }

    // ---------- ContainsKey ----------
    [Fact]
    public void ContainsKey_ReturnsTrue_ForExistingKey()
    {
        var cache = new Cache<string, int>(2);
        cache.Add("x", 10);

        Assert.True(cache.ContainsKey("x"));
    }

    [Fact]
    public void ContainsKey_ReturnsFalse_ForMissingKey()
    {
        var cache = new Cache<string, int>(2);

        Assert.False(cache.ContainsKey("nope"));
    }

    // ---------- Count ----------
    [Fact]
    public void Count_NewCache_IsZero()
    {
        Assert.Equal(0, new Cache<int, int>(5).Count);
    }

    [Fact]
    public void Count_NeverExceedsCapacity()
    {
        var cache = new Cache<int, int>(3);

        for (int i = 0; i < 10; i++)
            cache.Add(i, i * i);

        Assert.Equal(3, cache.Count);
    }
}
