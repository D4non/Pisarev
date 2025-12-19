using System.Collections.Immutable;
using Xunit;

namespace ConsoleApp2;

public class ImmutableListTests
{
    [Fact]
    public void Add_AddsElementToEnd()
    {
        var list = ImmutableList<int>.Empty;
        list = list.Add(345);
        list = list.Add(678);
        list = list.Add(901);

        Assert.Equal(3, list.Count);
        Assert.Equal(345, list[0]);
        Assert.Equal(901, list[2]);
    }

    [Fact]
    public void Insert_AddsElementAtBeginning()
    {
        var list = ImmutableList<int>.Empty.AddRange(new[] { 135, 246, 357 });
        list = list.Insert(0, 999);

        Assert.Equal(4, list.Count);
        Assert.Equal(999, list[0]);
    }

    [Fact]
    public void RemoveAt_RemovesFromEnd()
    {
        var list = ImmutableList<int>.Empty.AddRange(new[] { 147, 258, 369, 470 });
        list = list.RemoveAt(list.Count - 1);

        Assert.Equal(3, list.Count);
    }

    [Fact]
    public void IndexOf_FindsElementByValue()
    {
        var list = ImmutableList<int>.Empty.AddRange(new[] { 200, 400, 600, 800, 1000 });
        int index = list.IndexOf(600);

        Assert.Equal(2, index);
    }

    [Fact]
    public void Immutability_OriginalListIsNotModified()
    {
        var originalList = ImmutableList<int>.Empty.AddRange(new[] { 111, 222, 333 });
        var newList = originalList.Add(444);

        Assert.Equal(3, originalList.Count);
        Assert.Equal(4, newList.Count);
        Assert.DoesNotContain(444, originalList);
    }
}
