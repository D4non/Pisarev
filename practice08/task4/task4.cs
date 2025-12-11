using System;
using System.Threading.Channels;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var channel = Channel.CreateUnbounded<int>();
        
        Task producer = ProduceAsync(channel.Writer);
        Task consumer = ConsumeAsync(channel.Reader);
        
        await Task.WhenAll(producer, consumer);
    }
    
    static async Task ProduceAsync(ChannelWriter<int> writer)
    {
        for (int i = 1; i <= 5; i++)
        {
            await Task.Delay(200);
            await writer.WriteAsync(i);
            Console.WriteLine($"Отправлено: {i}");
        }
        writer.Complete();
    }
    
    static async Task ConsumeAsync(ChannelReader<int> reader)
    {
        await foreach (int item in reader.ReadAllAsync())
        {
            Console.WriteLine($"Обработано: {item * 10}");
        }
    }
}