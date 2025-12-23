using Xunit;
using Collections;

namespace Collections.Tests;

public class SimpleDictionaryTests
{
    [Fact]
    public void Constructor_Default_CreatesEmptyDictionary()
    {
        var dict = new SimpleDictionary<string, int>();
        Assert.Equal(0, dict.Count);
    }

    [Fact]
    public void Constructor_WithCapacity_CreatesEmptyDictionary()
    {
        var dict = new SimpleDictionary<string, int>(10);
        Assert.Equal(0, dict.Count);
    }

    [Fact]
    public void Add_KeyValuePair_AddsItem()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("возраст", 42);
        Assert.Equal(1, dict.Count);
        Assert.True(dict.ContainsKey("возраст"));
        Assert.Equal(42, dict["возраст"]);
    }

    [Fact]
    public void Add_DuplicateKey_ThrowsException()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("номер", 100);
        Assert.Throws<ArgumentException>(() => dict.Add("номер", 200));
    }

    [Fact]
    public void Add_KeyValuePairOverload_AddsItem()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add(new KeyValuePair<string, int>("годы", 25));
        Assert.Equal(1, dict.Count);
        Assert.True(dict.ContainsKey("годы"));
    }

    [Fact]
    public void Indexer_Get_ExistingKey_ReturnsValue()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("очки", 95);
        Assert.Equal(95, dict["очки"]);
    }

    [Fact]
    public void Indexer_Get_NonExistingKey_ThrowsException()
    {
        var dict = new SimpleDictionary<string, int>();
        Assert.Throws<KeyNotFoundException>(() => dict["отсутствует"]);
    }

    [Fact]
    public void Indexer_Set_NewKey_AddsItem()
    {
        var dict = new SimpleDictionary<string, int>();
        dict["цена"] = 150;
        Assert.Equal(1, dict.Count);
        Assert.Equal(150, dict["цена"]);
    }

    [Fact]
    public void Indexer_Set_ExistingKey_UpdatesValue()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("количество", 5);
        dict["количество"] = 10;
        Assert.Equal(1, dict.Count);
        Assert.Equal(10, dict["количество"]);
    }

    [Fact]
    public void ContainsKey_ExistingKey_ReturnsTrue()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("статус", 1);
        Assert.True(dict.ContainsKey("статус"));
    }

    [Fact]
    public void ContainsKey_NonExistingKey_ReturnsFalse()
    {
        var dict = new SimpleDictionary<string, int>();
        Assert.False(dict.ContainsKey("неизвестно"));
    }

    [Fact]
    public void Contains_ExistingKeyValuePair_ReturnsTrue()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("уровень", 3);
        Assert.True(dict.Contains(new KeyValuePair<string, int>("уровень", 3)));
    }

    [Fact]
    public void Contains_WrongValue_ReturnsFalse()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("оценка", 85);
        Assert.False(dict.Contains(new KeyValuePair<string, int>("оценка", 90)));
    }

    [Fact]
    public void Contains_NonExistingKey_ReturnsFalse()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("температура", 20);
        Assert.False(dict.Contains(new KeyValuePair<string, int>("влажность", 20)));
    }

    [Fact]
    public void TryGetValue_ExistingKey_ReturnsTrueAndValue()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("ширина", 800);
        bool result = dict.TryGetValue("ширина", out int value);
        Assert.True(result);
        Assert.Equal(800, value);
    }

    [Fact]
    public void TryGetValue_NonExistingKey_ReturnsFalse()
    {
        var dict = new SimpleDictionary<string, int>();
        bool result = dict.TryGetValue("высота", out int value);
        Assert.False(result);
        Assert.Equal(0, value);
    }

    [Fact]
    public void Remove_ExistingKey_RemovesItem()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("икс", 1);
        dict.Add("игрек", 2);
        bool result = dict.Remove("икс");
        Assert.True(result);
        Assert.Equal(1, dict.Count);
        Assert.False(dict.ContainsKey("икс"));
        Assert.True(dict.ContainsKey("игрек"));
    }

    [Fact]
    public void Remove_NonExistingKey_ReturnsFalse()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("а", 1);
        bool result = dict.Remove("б");
        Assert.False(result);
        Assert.Equal(1, dict.Count);
    }

    [Fact]
    public void Remove_KeyValuePair_RemovesItem()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("ключ", 123);
        bool result = dict.Remove(new KeyValuePair<string, int>("ключ", 123));
        Assert.True(result);
        Assert.Equal(0, dict.Count);
    }

    [Fact]
    public void Remove_KeyValuePair_WrongValue_ReturnsFalse()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("проверка", 50);
        bool result = dict.Remove(new KeyValuePair<string, int>("проверка", 51));
        Assert.False(result);
        Assert.Equal(1, dict.Count);
    }

    [Fact]
    public void Clear_RemovesAllItems()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("первый", 1);
        dict.Add("второй", 2);
        dict.Add("третий", 3);
        dict.Clear();
        Assert.Equal(0, dict.Count);
        Assert.False(dict.ContainsKey("первый"));
    }

    [Fact]
    public void Keys_ReturnsAllKeys()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("альфа", 1);
        dict.Add("бета", 2);
        dict.Add("гамма", 3);
        var keys = dict.Keys.ToList();
        Assert.Equal(3, keys.Count);
        Assert.Contains("альфа", keys);
        Assert.Contains("бета", keys);
        Assert.Contains("гамма", keys);
    }

    [Fact]
    public void Values_ReturnsAllValues()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("красный", 255);
        dict.Add("зеленый", 128);
        dict.Add("синий", 64);
        var values = dict.Values.ToList();
        Assert.Equal(3, values.Count);
        Assert.Contains(255, values);
        Assert.Contains(128, values);
        Assert.Contains(64, values);
    }

    [Fact]
    public void GetEnumerator_EnumeratesAllItems()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("янв", 1);
        dict.Add("фев", 2);
        dict.Add("мар", 3);
        var items = new List<KeyValuePair<string, int>>();
        foreach (var kvp in dict)
        {
            items.Add(kvp);
        }
        Assert.Equal(3, items.Count);
        Assert.Contains(new KeyValuePair<string, int>("янв", 1), items);
        Assert.Contains(new KeyValuePair<string, int>("фев", 2), items);
        Assert.Contains(new KeyValuePair<string, int>("мар", 3), items);
    }

    [Fact]
    public void CopyTo_CopiesAllItems()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("один", 1);
        dict.Add("два", 2);
        var array = new KeyValuePair<string, int>[5];
        dict.CopyTo(array, 1);
        Assert.Equal(default(KeyValuePair<string, int>), array[0]);
        Assert.NotEqual(default(KeyValuePair<string, int>), array[1]);
        Assert.NotEqual(default(KeyValuePair<string, int>), array[2]);
    }

    [Fact]
    public void CopyTo_NullArray_ThrowsException()
    {
        var dict = new SimpleDictionary<string, int>();
        dict.Add("проверка", 1);
        Assert.Throws<ArgumentNullException>(() => dict.CopyTo(null!, 0));
    }

    [Fact]
    public void Resize_HandlesManyItems()
    {
        var dict = new SimpleDictionary<int, string>(4);
        for (int i = 0; i < 100; i++)
        {
            dict[i] = $"значение{i}";
        }
        Assert.Equal(100, dict.Count);
        for (int i = 0; i < 100; i++)
        {
            Assert.Equal($"значение{i}", dict[i]);
        }
    }

    [Fact]
    public void HashCollisions_HandledCorrectly()
    {
        var dict = new SimpleDictionary<string, int>();
        dict["аа"] = 1;
        dict["бб"] = 2;
        dict["вв"] = 3;
        Assert.Equal(3, dict.Count);
        Assert.Equal(1, dict["аа"]);
        Assert.Equal(2, dict["бб"]);
        Assert.Equal(3, dict["вв"]);
    }

    [Fact]
    public void IsReadOnly_ReturnsFalse()
    {
        var dict = new SimpleDictionary<string, int>();
        Assert.False(dict.IsReadOnly);
    }

    [Fact]
    public void Add_NullKey_ThrowsException()
    {
        var dict = new SimpleDictionary<string, int>();
        Assert.Throws<ArgumentNullException>(() => dict.Add(null!, 10));
    }
}
