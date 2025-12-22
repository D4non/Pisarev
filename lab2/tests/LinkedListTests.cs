using Xunit;

namespace ConsoleApp2;

public class LinkedListTests
{
    [Fact]
    public void AddLast_AddsElementToEnd()
    {
        var list = new LinkedList<int>();
        list.AddLast(44);
        list.AddLast(88);
        list.AddLast(22);

        Assert.Equal(3, list.Count);
        Assert.Equal(44, list.First?.Value);
        Assert.Equal(22, list.Last?.Value);
    }

    [Fact]
    public void AddFirst_AddsElementToBeginning()
    {
        var list = new LinkedList<int>();
        list.AddFirst(77);
        list.AddFirst(55);
        list.AddFirst(33);

        Assert.Equal(3, list.Count);
        Assert.Equal(33, list.First?.Value);
    }

    [Fact]
    public void RemoveFirst_RemovesFromBeginning()
    {
        var list = new LinkedList<int>();
        list.AddLast(111);
        list.AddLast(222);
        list.AddLast(333);
        list.RemoveFirst();

        Assert.Equal(2, list.Count);
        Assert.Equal(222, list.First?.Value);
    }

    [Fact]
    public void RemoveLast_RemovesFromEnd()
    {
        var list = new LinkedList<int>();
        list.AddLast(500);
        list.AddLast(600);
        list.AddLast(700);
        list.RemoveLast();

        Assert.Equal(2, list.Count);
        Assert.Equal(600, list.Last?.Value);
    }

    [Fact]
    public void Contains_FindsElementByValue()
    {
        var list = new LinkedList<int>();
        list.AddLast(150);
        list.AddLast(250);
        list.AddLast(350);

        Assert.Contains(250, list);
        Assert.DoesNotContain(999, list);
    }
}

