using StackExchange.Redis;

var connection = await ConnectionMultiplexer.ConnectAsync("localhost:1905");
ISubscriber subscriber = connection.GetSubscriber();
var channel = RedisChannel.Literal("mychannel");

await subscriber.SubscribeAsync(channel, (_, message) =>
{
    Console.WriteLine($"[{DateTime.Now:T}] {message}");
});

Console.WriteLine("Subscribed to mychannel. Press Ctrl+C to exit.");

await Task.Delay(Timeout.InfiniteTimeSpan);
