using Xunit;

namespace ConsoleApp2;

public class StackTests
{
    [Fact]
    public void Push_AddsElementToTop()
    {
        var stack = new Stack<int>();
        stack.Push(123);
        stack.Push(456);
        stack.Push(789);

        Assert.Equal(3, stack.Count);
    }

    [Fact]
    public void Pop_RemovesElementFromTop()
    {
        var stack = new Stack<int>();
        stack.Push(321);
        stack.Push(654);
        stack.Push(987);

        Assert.Equal(987, stack.Pop());
        Assert.Equal(2, stack.Count);
    }

    [Fact]
    public void Contains_FindsElementByValue()
    {
        var stack = new Stack<int>();
        stack.Push(234);
        stack.Push(567);
        stack.Push(890);

        Assert.True(stack.Contains(567));
        Assert.False(stack.Contains(999));
    }

    [Fact]
    public void LIFO_OrderIsMaintained()
    {
        var stack = new Stack<int>();
        stack.Push(111);
        stack.Push(222);
        stack.Push(333);

        Assert.Equal(333, stack.Pop());
        Assert.Equal(222, stack.Pop());
        Assert.Equal(111, stack.Pop());
        Assert.Empty(stack);
    }
}

