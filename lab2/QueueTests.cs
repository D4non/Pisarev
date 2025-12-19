using Xunit;

namespace ConsoleApp2;

public class QueueTests
{
    [Fact]
    public void Enqueue_AddsElementToEnd()
    {
        var queue = new Queue<int>();
        queue.Enqueue(19);
        queue.Enqueue(38);
        queue.Enqueue(57);

        Assert.Equal(3, queue.Count);
    }

    [Fact]
    public void Dequeue_RemovesElementFromBeginning()
    {
        var queue = new Queue<int>();
        queue.Enqueue(101);
        queue.Enqueue(202);
        queue.Enqueue(303);

        Assert.Equal(101, queue.Dequeue());
        Assert.Equal(2, queue.Count);
    }

    [Fact]
    public void Contains_FindsElementByValue()
    {
        var queue = new Queue<int>();
        queue.Enqueue(450);
        queue.Enqueue(550);
        queue.Enqueue(650);

        Assert.True(queue.Contains(550));
        Assert.False(queue.Contains(999));
    }

    [Fact]
    public void FIFO_OrderIsMaintained()
    {
        var queue = new Queue<int>();
        queue.Enqueue(90);
        queue.Enqueue(180);
        queue.Enqueue(270);

        Assert.Equal(90, queue.Dequeue());
        Assert.Equal(180, queue.Dequeue());
        Assert.Equal(270, queue.Dequeue());
        Assert.Empty(queue);
    }
}
