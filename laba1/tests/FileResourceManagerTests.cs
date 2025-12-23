using ConsoleApp2.Services;

namespace ConsoleApp2;

class FileResourceManagerTests
{
    public static void RunAllTests()
    {
        using (var manager = new FileResourceManager("data/test.txt", FileMode.Create))
        {
            manager.OpenForWriting();
            manager.WriteLine("Привет");
            manager.WriteLine("Мир");
        }
        
        using (var manager = new FileResourceManager("data/test.txt", FileMode.Open))
        {
            manager.OpenForReading();
            Console.WriteLine(manager.ReadAllText());
            var info = manager.GetFileInfo();
            Console.WriteLine($"Размер файла: {info.Length}");
        }
    }
}

