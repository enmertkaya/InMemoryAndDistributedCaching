using StackExchange.Redis;

var connection = await ConnectionMultiplexer.ConnectAsync("localhost:1905");
ISubscriber subscriber = connection.GetSubscriber();

Console.WriteLine("Redis publisher ready. Press Ctrl+C to exit.");

while (true)
{
    Console.Write("Mesaj : ");
    string? message = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(message))
    {
        continue;
    }

    await subscriber.PublishAsync("mychannel", message);
    Console.WriteLine("Message published.\n");
}
