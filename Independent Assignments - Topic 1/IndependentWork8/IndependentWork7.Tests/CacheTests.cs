using System;
using System.Collections.Generic;
using Xunit;

namespace IndependentWork7.Tests;

public class CacheTests
{
    [Fact]
    public void Add_And_Get_ValidKey_ReturnsValue()
    {
        // Arrange
        var cache = new Cache<int, string>(3);

        // Act
        cache.Add(1, "One");

        // Assert
        Assert.Equal("One", cache.Get(1));
    }

    [Fact]
    public void Count_ReflectsNumberOfAddedItems()
    {
        // Arrange
        var cache = new Cache<string, int>(5);

        // Act & Assert
        Assert.Equal(0, cache.Count);

        cache.Add("A", 100);
        Assert.Equal(1, cache.Count);

        cache.Add("B", 200);
        Assert.Equal(2, cache.Count);
    }

    [Fact]
    public void ContainsKey_ReturnsTrueForExistingKeyAndFalseForMissingKey()
    {
        // Arrange
        var cache = new Cache<string, string>(3);
        cache.Add("user1", "Alice");

        // Act & Assert
        Assert.True(cache.ContainsKey("user1"));
        Assert.False(cache.ContainsKey("user2"));
    }

    [Fact]
    public void Get_MissingKey_ThrowsKeyNotFoundException()
    {
        // Arrange
        var cache = new Cache<string, int>(3);

        // Act & Assert
        var exception = Assert.Throws<KeyNotFoundException>(() => cache.Get("non_existent"));
        Assert.Contains("non_existent", exception.Message);
    }

    [Fact]
    public void Add_WhenCapacityExceeded_EvictsOldestItemFIFO()
    {
        // Arrange (ємність = 2)
        var cache = new Cache<int, string>(2);

        cache.Add(1, "First");
        cache.Add(2, "Second");

        // Act - додаємо 3-й елемент, 1-й повинен видалитися (FIFO)
        cache.Add(3, "Third");

        // Assert
        Assert.Equal(2, cache.Count);
        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
        Assert.Throws<KeyNotFoundException>(() => cache.Get(1));
        Assert.Equal("Second", cache.Get(2));
        Assert.Equal("Third", cache.Get(3));
    }

    [Fact]
    public void Add_ExistingKey_UpdatesValueWithoutIncreasingCount()
    {
        // Arrange
        var cache = new Cache<string, string>(3);
        cache.Add("key", "initial");

        // Act
        cache.Add("key", "updated");

        // Assert
        Assert.Equal(1, cache.Count);
        Assert.Equal("updated", cache.Get("key"));
    }

    [Fact]
    public void Constructor_InvalidCapacity_ThrowsArgumentOutOfRangeException()
    {
        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => new Cache<string, string>(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Cache<string, string>(-5));
    }
}
