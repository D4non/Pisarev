using Xunit;
using Collections;

namespace Collections.Tests;

public class SimpleListTests
{
    [Fact]
    public void Constructor_Default_CreatesEmptyList()
    {
        var list = new SimpleList();
        Assert.Equal(0, list.Count);
    }

    [Fact]
    public void Constructor_WithCapacity_CreatesEmptyList()
    {
        var list = new SimpleList(10);
        Assert.Equal(0, list.Count);
    }

    [Fact]
    public void Constructor_NegativeCapacity_ThrowsException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SimpleList(-1));
    }

    [Fact]
    public void Add_Item_IncreasesCount()
    {
        var list = new SimpleList();
        list.Add("борщ");
        Assert.Equal(1, list.Count);
    }

    [Fact]
    public void Add_MultipleItems_IncreasesCount()
    {
        var list = new SimpleList();
        list.Add("пельмени");
        list.Add("макороны");
        list.Add("окрошка");
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void Add_ReturnsCorrectIndex()
    {
        var list = new SimpleList();
        int index1 = list.Add("табуретка");
        int index2 = list.Add("диван");
        Assert.Equal(0, index1);
        Assert.Equal(1, index2);
    }

    [Fact]
    public void Indexer_Get_ReturnsCorrectValue()
    {
        var list = new SimpleList();
        list.Add("мороженое");
        list.Add("шоколад");
        Assert.Equal("мороженое", list[0]);
        Assert.Equal("шоколад", list[1]);
    }

    [Fact]
    public void Indexer_Set_UpdatesValue()
    {
        var list = new SimpleList();
        list.Add("старый");
        list[0] = "новый";
        Assert.Equal("новый", list[0]);
    }

    [Fact]
    public void Indexer_Get_OutOfRange_ThrowsException()
    {
        var list = new SimpleList();
        list.Add("проверка");
        Assert.Throws<ArgumentOutOfRangeException>(() => list[1]);
        Assert.Throws<ArgumentOutOfRangeException>(() => list[-1]);
    }

    [Fact]
    public void Indexer_Set_OutOfRange_ThrowsException()
    {
        var list = new SimpleList();
        list.Add("проверка");
        Assert.Throws<ArgumentOutOfRangeException>(() => list[1] = "провал");
        Assert.Throws<ArgumentOutOfRangeException>(() => list[-1] = "провал");
    }

    [Fact]
    public void Contains_ExistingItem_ReturnsTrue()
    {
        var list = new SimpleList();
        list.Add("котлета");
        list.Add("сосиска");
        Assert.True(list.Contains("котлета"));
        Assert.True(list.Contains("сосиска"));
    }

    [Fact]
    public void Contains_NonExistingItem_ReturnsFalse()
    {
        var list = new SimpleList();
        list.Add("хомячок");
        Assert.False(list.Contains("бегемот"));
    }

    [Fact]
    public void Contains_Null_ReturnsTrueWhenNullExists()
    {
        var list = new SimpleList();
        list.Add(null);
        Assert.True(list.Contains(null));
    }

    [Fact]
    public void IndexOf_ExistingItem_ReturnsCorrectIndex()
    {
        var list = new SimpleList();
        list.Add("карусель");
        list.Add("качели");
        list.Add("горка");
        Assert.Equal(0, list.IndexOf("карусель"));
        Assert.Equal(1, list.IndexOf("качели"));
        Assert.Equal(2, list.IndexOf("горка"));
    }

    [Fact]
    public void IndexOf_NonExistingItem_ReturnsMinusOne()
    {
        var list = new SimpleList();
        list.Add("один");
        Assert.Equal(-1, list.IndexOf("два"));
    }

    [Fact]
    public void Remove_ExistingItem_DecreasesCount()
    {
        var list = new SimpleList();
        list.Add("понедельник");
        list.Add("вторник");
        list.Add("среда");
        list.Remove("вторник");
        Assert.Equal(2, list.Count);
        Assert.False(list.Contains("вторник"));
    }

    [Fact]
    public void Remove_NonExistingItem_DoesNotChangeCount()
    {
        var list = new SimpleList();
        list.Add("январь");
        list.Remove("февраль");
        Assert.Equal(1, list.Count);
    }

    [Fact]
    public void Remove_FirstItem_ShiftsRemainingItems()
    {
        var list = new SimpleList();
        list.Add("весна");
        list.Add("лето");
        list.Add("осень");
        list.Remove("весна");
        Assert.Equal("лето", list[0]);
        Assert.Equal("осень", list[1]);
    }

    [Fact]
    public void RemoveAt_ValidIndex_RemovesItem()
    {
        var list = new SimpleList();
        list.Add("Москва");
        list.Add("Петрозаводск");
        list.Add("Краснодар");
        list.RemoveAt(1);
        Assert.Equal(2, list.Count);
        Assert.Equal("Москва", list[0]);
        Assert.Equal("Краснодар", list[1]);
    }

    [Fact]
    public void RemoveAt_InvalidIndex_ThrowsException()
    {
        var list = new SimpleList();
        list.Add("проверка");
        Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(1));
        Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(-1));
    }

    [Fact]
    public void Insert_ValidIndex_InsertsItem()
    {
        var list = new SimpleList();
        list.Add("один");
        list.Add("три");
        list.Insert(1, "два");
        Assert.Equal(3, list.Count);
        Assert.Equal("один", list[0]);
        Assert.Equal("два", list[1]);
        Assert.Equal("три", list[2]);
    }

    [Fact]
    public void Insert_AtBeginning_InsertsItem()
    {
        var list = new SimpleList();
        list.Add("второй");
        list.Insert(0, "первый");
        Assert.Equal(2, list.Count);
        Assert.Equal("первый", list[0]);
        Assert.Equal("второй", list[1]);
    }

    [Fact]
    public void Insert_AtEnd_InsertsItem()
    {
        var list = new SimpleList();
        list.Add("начало");
        list.Insert(1, "конец");
        Assert.Equal(2, list.Count);
        Assert.Equal("начало", list[0]);
        Assert.Equal("конец", list[1]);
    }

    [Fact]
    public void Insert_InvalidIndex_ThrowsException()
    {
        var list = new SimpleList();
        list.Add("проверка");
        Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(2, "провал"));
        Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(-1, "провал"));
    }

    [Fact]
    public void Clear_RemovesAllItems()
    {
        var list = new SimpleList();
        list.Add("а");
        list.Add("б");
        list.Add("в");
        list.Clear();
        Assert.Equal(0, list.Count);
        Assert.False(list.Contains("а"));
    }

    [Fact]
    public void CopyTo_ValidArray_CopiesItems()
    {
        var list = new SimpleList();
        list.Add("х");
        list.Add("у");
        list.Add("з");
        var array = new object[5];
        list.CopyTo(array, 1);
        Assert.Null(array[0]);
        Assert.Equal("х", array[1]);
        Assert.Equal("у", array[2]);
        Assert.Equal("з", array[3]);
        Assert.Null(array[4]);
    }

    [Fact]
    public void CopyTo_NullArray_ThrowsException()
    {
        var list = new SimpleList();
        list.Add("проверка");
        Assert.Throws<ArgumentNullException>(() => list.CopyTo(null!, 0));
    }

    [Fact]
    public void CopyTo_InvalidIndex_ThrowsException()
    {
        var list = new SimpleList();
        list.Add("проверка");
        var array = new object[1];
        Assert.Throws<ArgumentOutOfRangeException>(() => list.CopyTo(array, -1));
    }

    [Fact]
    public void CopyTo_InsufficientSpace_ThrowsException()
    {
        var list = new SimpleList();
        list.Add("а");
        list.Add("б");
        var array = new object[2];
        Assert.Throws<ArgumentException>(() => list.CopyTo(array, 1));
    }

    [Fact]
    public void GetEnumerator_EnumeratesAllItems()
    {
        var list = new SimpleList();
        list.Add("первый");
        list.Add("второй");
        list.Add("третий");
        var items = new List<object?>();
        foreach (var item in list)
        {
            items.Add(item);
        }
        Assert.Equal(3, items.Count);
        Assert.Equal("первый", items[0]);
        Assert.Equal("второй", items[1]);
        Assert.Equal("третий", items[2]);
    }

    [Fact]
    public void Capacity_GrowsAutomatically()
    {
        var list = new SimpleList(2);
        for (int i = 0; i < 10; i++)
        {
            list.Add($"элемент{i}");
        }
        Assert.Equal(10, list.Count);
        for (int i = 0; i < 10; i++)
        {
            Assert.Equal($"элемент{i}", list[i]);
        }
    }

    [Fact]
    public void IsReadOnly_ReturnsFalse()
    {
        var list = new SimpleList();
        Assert.False(list.IsReadOnly);
    }

    [Fact]
    public void IsFixedSize_ReturnsFalse()
    {
        var list = new SimpleList();
        Assert.False(list.IsFixedSize);
    }
}
