using Collections;

var список = new SimpleList();
список.Add("борщ");
список.Add("пельмени");
список.Add("шашлык");
Console.WriteLine($"Всего: {список.Count}");
Console.WriteLine($"Первый: {список[0]}");
Console.WriteLine($"Есть пельмени: {список.Contains("пельмени")}");
список.Remove("пельмени");
Console.WriteLine($"После удаления: {список.Count}");
foreach (var элемент in список)
{
    Console.WriteLine(элемент);
}

var словарь = new SimpleDictionary<string, int>();
словарь.Add("котлета", 150);
словарь.Add("сосиска", 80);
словарь["колбаса"] = 200;
Console.WriteLine($"Всего: {словарь.Count}");
Console.WriteLine($"Цена котлеты: {словарь["котлета"]}");
Console.WriteLine($"Есть сосиска: {словарь.ContainsKey("сосиска")}");
словарь["котлета"] = 180;
Console.WriteLine($"Новая цена котлеты: {словарь["котлета"]}");
словарь.Remove("колбаса");
Console.WriteLine($"После удаления: {словарь.Count}");
foreach (var пара in словарь)
{
    Console.WriteLine($"{пара.Key}: {пара.Value}");
}

var список2 = new DoublyLinkedList();
список2.Add("понедельник");
список2.Add("вторник");
список2.AddFirst("воскресенье");
Console.WriteLine($"Всего: {список2.Count}");
Console.WriteLine($"Первый: {список2[0]}");
Console.WriteLine($"Последний: {список2[2]}");
Console.WriteLine($"Индекс вторника: {список2.IndexOf("вторник")}");
список2.Insert(2, "пятница");
Console.WriteLine($"После вставки: {список2.Count}");
список2.RemoveAt(1);
Console.WriteLine($"После удаления: {список2.Count}");
foreach (var день in список2)
{
    Console.WriteLine(день);
}
