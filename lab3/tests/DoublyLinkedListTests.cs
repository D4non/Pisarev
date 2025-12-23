using Xunit;
using Collections;

namespace Collections.Tests;

public class DoublyLinkedListTests
{
    [Fact]
    public void Constructor_CreatesEmptyList()
    {
        var list = new DoublyLinkedList();
        Assert.Equal(0, list.Count);
    }

    [Fact]
    public void Add_Item_IncreasesCount()
    {
        var list = new DoublyLinkedList();
        list.Add("табуретка");
        Assert.Equal(1, list.Count);
    }

    [Fact]
    public void Add_MultipleItems_IncreasesCount()
    {
        var list = new DoublyLinkedList();
        list.Add("котлета");
        list.Add("сосиска");
        list.Add("колбаса");
        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void Add_ReturnsCorrectIndex()
    {
        var list = new DoublyLinkedList();
        int index1 = list.Add("карандаш");
        int index2 = list.Add("ручка");
        Assert.Equal(0, index1);
        Assert.Equal(1, index2);
    }

    [Fact]
    public void AddLast_AddsItemAtEnd()
    {
        var list = new DoublyLinkedList();
        list.Add("начало");
        list.AddLast("конец");
        Assert.Equal(2, list.Count);
        Assert.Equal("начало", list[0]);
        Assert.Equal("конец", list[1]);
    }

    [Fact]
    public void AddFirst_AddsItemAtBeginning()
    {
        var list = new DoublyLinkedList();
        list.Add("второй");
        list.AddFirst("первый");
        Assert.Equal(2, list.Count);
        Assert.Equal("первый", list[0]);
        Assert.Equal("второй", list[1]);
    }

    [Fact]
    public void Indexer_Get_ReturnsCorrectValue()
    {
        var list = new DoublyLinkedList();
        list.Add("шаурма");
        list.Add("шашлык");
        list.Add("пицца");
        Assert.Equal("шаурма", list[0]);
        Assert.Equal("шашлык", list[1]);
        Assert.Equal("пицца", list[2]);
    }

    [Fact]
    public void Indexer_Set_UpdatesValue()
    {
        var list = new DoublyLinkedList();
        list.Add("старый");
        list[0] = "новый";
        Assert.Equal("новый", list[0]);
    }

    [Fact]
    public void Indexer_Get_OutOfRange_ThrowsException()
    {
        var list = new DoublyLinkedList();
        list.Add("проверка");
        Assert.Throws<ArgumentOutOfRangeException>(() => list[1]);
        Assert.Throws<ArgumentOutOfRangeException>(() => list[-1]);
    }

    [Fact]
    public void Indexer_Set_OutOfRange_ThrowsException()
    {
        var list = new DoublyLinkedList();
        list.Add("проверка");
        Assert.Throws<ArgumentOutOfRangeException>(() => list[1] = "провал");
        Assert.Throws<ArgumentOutOfRangeException>(() => list[-1] = "провал");
    }

    [Fact]
    public void Contains_ExistingItem_ReturnsTrue()
    {
        var list = new DoublyLinkedList();
        list.Add("крокодил");
        list.Add("бегемот");
        Assert.True(list.Contains("крокодил"));
        Assert.True(list.Contains("бегемот"));
    }

    [Fact]
    public void Contains_NonExistingItem_ReturnsFalse()
    {
        var list = new DoublyLinkedList();
        list.Add("хомячок");
        Assert.False(list.Contains("жираф"));
    }

    [Fact]
    public void IndexOf_ExistingItem_ReturnsCorrectIndex()
    {
        var list = new DoublyLinkedList();
        list.Add("понедельник");
        list.Add("вторник");
        list.Add("среда");
        Assert.Equal(0, list.IndexOf("понедельник"));
        Assert.Equal(1, list.IndexOf("вторник"));
        Assert.Equal(2, list.IndexOf("среда"));
    }

    [Fact]
    public void IndexOf_NonExistingItem_ReturnsMinusOne()
    {
        var list = new DoublyLinkedList();
        list.Add("январь");
        Assert.Equal(-1, list.IndexOf("февраль"));
    }

    [Fact]
    public void Remove_ExistingItem_DecreasesCount()
    {
        var list = new DoublyLinkedList();
        list.Add("весна");
        list.Add("лето");
        list.Add("зима");
        list.Remove("лето");
        Assert.Equal(2, list.Count);
        Assert.False(list.Contains("лето"));
    }

    [Fact]
    public void Remove_FirstItem_RemovesCorrectly()
    {
        var list = new DoublyLinkedList();
        list.Add("альфа");
        list.Add("бета");
        list.Add("гамма");
        list.Remove("альфа");
        Assert.Equal(2, list.Count);
        Assert.Equal("бета", list[0]);
        Assert.Equal("гамма", list[1]);
    }

    [Fact]
    public void Remove_LastItem_RemovesCorrectly()
    {
        var list = new DoublyLinkedList();
        list.Add("один");
        list.Add("два");
        list.Add("три");
        list.Remove("три");
        Assert.Equal(2, list.Count);
        Assert.Equal("один", list[0]);
        Assert.Equal("два", list[1]);
    }

    [Fact]
    public void Remove_NonExistingItem_DoesNotChangeCount()
    {
        var list = new DoublyLinkedList();
        list.Add("присутствует");
        list.Remove("отсутствует");
        Assert.Equal(1, list.Count);
    }

    [Fact]
    public void RemoveAt_ValidIndex_RemovesItem()
    {
        var list = new DoublyLinkedList();
        list.Add("первый");
        list.Add("второй");
        list.Add("третий");
        list.RemoveAt(1);
        Assert.Equal(2, list.Count);
        Assert.Equal("первый", list[0]);
        Assert.Equal("третий", list[1]);
    }

    [Fact]
    public void RemoveAt_FirstIndex_RemovesFirstItem()
    {
        var list = new DoublyLinkedList();
        list.Add("а");
        list.Add("б");
        list.RemoveAt(0);
        Assert.Equal(1, list.Count);
        Assert.Equal("б", list[0]);
    }

    [Fact]
    public void RemoveAt_LastIndex_RemovesLastItem()
    {
        var list = new DoublyLinkedList();
        list.Add("х");
        list.Add("у");
        list.RemoveAt(1);
        Assert.Equal(1, list.Count);
        Assert.Equal("х", list[0]);
    }

    [Fact]
    public void RemoveAt_InvalidIndex_ThrowsException()
    {
        var list = new DoublyLinkedList();
        list.Add("проверка");
        Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(1));
        Assert.Throws<ArgumentOutOfRangeException>(() => list.RemoveAt(-1));
    }

    [Fact]
    public void Insert_ValidIndex_InsertsItem()
    {
        var list = new DoublyLinkedList();
        list.Add("начало");
        list.Add("конец");
        list.Insert(1, "середина");
        Assert.Equal(3, list.Count);
        Assert.Equal("начало", list[0]);
        Assert.Equal("середина", list[1]);
        Assert.Equal("конец", list[2]);
    }

    [Fact]
    public void Insert_AtBeginning_InsertsItem()
    {
        var list = new DoublyLinkedList();
        list.Add("второй");
        list.Insert(0, "первый");
        Assert.Equal(2, list.Count);
        Assert.Equal("первый", list[0]);
        Assert.Equal("второй", list[1]);
    }

    [Fact]
    public void Insert_AtEnd_InsertsItem()
    {
        var list = new DoublyLinkedList();
        list.Add("первый");
        list.Insert(1, "второй");
        Assert.Equal(2, list.Count);
        Assert.Equal("первый", list[0]);
        Assert.Equal("второй", list[1]);
    }

    [Fact]
    public void Insert_InvalidIndex_ThrowsException()
    {
        var list = new DoublyLinkedList();
        list.Add("проверка");
        Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(2, "провал"));
        Assert.Throws<ArgumentOutOfRangeException>(() => list.Insert(-1, "провал"));
    }

    [Fact]
    public void Clear_RemovesAllItems()
    {
        var list = new DoublyLinkedList();
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
        var list = new DoublyLinkedList();
        list.Add("п");
        list.Add("р");
        list.Add("о");
        var array = new object[5];
        list.CopyTo(array, 1);
        Assert.Null(array[0]);
        Assert.Equal("п", array[1]);
        Assert.Equal("р", array[2]);
        Assert.Equal("о", array[3]);
        Assert.Null(array[4]);
    }

    [Fact]
    public void CopyTo_NullArray_ThrowsException()
    {
        var list = new DoublyLinkedList();
        list.Add("проверка");
        Assert.Throws<ArgumentNullException>(() => list.CopyTo(null!, 0));
    }

    [Fact]
    public void CopyTo_InvalidIndex_ThrowsException()
    {
        var list = new DoublyLinkedList();
        list.Add("проверка");
        var array = new object[1];
        Assert.Throws<ArgumentOutOfRangeException>(() => list.CopyTo(array, -1));
    }

    [Fact]
    public void CopyTo_InsufficientSpace_ThrowsException()
    {
        var list = new DoublyLinkedList();
        list.Add("а");
        list.Add("б");
        var array = new object[2];
        Assert.Throws<ArgumentException>(() => list.CopyTo(array, 1));
    }

    [Fact]
    public void GetEnumerator_EnumeratesAllItems()
    {
        var list = new DoublyLinkedList();
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
    public void GetEnumerator_ModificationDuringEnumeration_ThrowsException()
    {
        var list = new DoublyLinkedList();
        list.Add("а");
        list.Add("б");
        var enumerator = list.GetEnumerator();
        enumerator.MoveNext();
        list.Add("в");
        Assert.Throws<InvalidOperationException>(() => enumerator.MoveNext());
    }

    [Fact]
    public void Indexer_AccessFromEnd_Optimized()
    {
        var list = new DoublyLinkedList();
        for (int i = 0; i < 100; i++)
        {
            list.Add($"элемент{i}");
        }
        Assert.Equal("элемент52", list[52]);
        Assert.Equal("элемент42", list[42]);
        Assert.Equal("элемент67", list[67]);
    }

    [Fact]
    public void IsReadOnly_ReturnsFalse()
    {
        var list = new DoublyLinkedList();
        Assert.False(list.IsReadOnly);
    }

    [Fact]
    public void IsFixedSize_ReturnsFalse()
    {
        var list = new DoublyLinkedList();
        Assert.False(list.IsFixedSize);
    }

    [Fact]
    public void SingleItem_OperationsWorkCorrectly()
    {
        var list = new DoublyLinkedList();
        list.Add("одиночка");
        Assert.Equal(1, list.Count);
        Assert.Equal("одиночка", list[0]);
        Assert.True(list.Contains("одиночка"));
        list.Remove("одиночка");
        Assert.Equal(0, list.Count);
    }
}
