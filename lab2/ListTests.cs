using Xunit;

namespace ConsoleApp2;

public class ListTests
{
    [Fact]
    public void Add_AddsElementToEnd()
    {
        var list = new List<int>();
        list.Add(42);
        list.Add(87);
        list.Add(15);

        Assert.Equal(3, list.Count);
        Assert.Equal(42, list[0]);
        Assert.Equal(87, list[1]);
        Assert.Equal(15, list[2]);
    }

    [Fact]
    public void Insert_AddsElementAtBeginning()
    {
        var list = new List<int> { 5, 12, 99 };
        list.Insert(0, 33);

        Assert.Equal(4, list.Count);
        Assert.Equal(33, list[0]);
    }

    [Fact]
    public void Insert_AddsElementInMiddle()
    {
        var list = new List<int> { 10, 20, 40, 50 };
        list.Insert(2, 30);

        Assert.Equal(5, list.Count);
        Assert.Equal(30, list[2]);
    }

    [Fact]
    public void RemoveAt_RemovesFromEnd()
    {
        var list = new List<int> { 7, 14, 21, 28 };
        list.RemoveAt(list.Count - 1);

        Assert.Equal(3, list.Count);
        Assert.Equal(21, list[list.Count - 1]);
    }

    [Fact]
    public void RemoveAt_RemovesFromBeginning()
    {
        var list = new List<int> { 100, 200, 300, 400 };
        list.RemoveAt(0);

        Assert.Equal(3, list.Count);
        Assert.Equal(200, list[0]);
    }

    [Fact]
    public void RemoveAt_RemovesFromMiddle()
    {
        var list = new List<int> { 1, 2, 3, 4, 5 };
        list.RemoveAt(2);

        Assert.Equal(4, list.Count);
        Assert.Equal(4, list[2]);
        Assert.Equal(2, list[1]);
    }

    [Fact]
    public void IndexOf_FindsElementByValue()
    {
        var list = new List<int> { 25, 50, 75, 100, 125 };
        int index = list.IndexOf(75);

        Assert.Equal(2, index);
    }

    [Fact]
    public void IndexOf_ReturnsMinusOneWhenNotFound()
    {
        var list = new List<int> { 11, 22, 33 };
        int index = list.IndexOf(999);

        Assert.Equal(-1, index);
    }

    [Fact]
    public void Indexer_GetElementByIndex()
    {
        var list = new List<int> { 60, 70, 80, 90 };
        int value = list[2];

        Assert.Equal(80, value);
    }
}
