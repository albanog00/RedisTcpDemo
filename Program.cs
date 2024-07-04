using RedisClient;

Client client = new();

while (true)
{
    Console.WriteLine("Send a command (GET, SET, PING, quit):");
    var command = Console.ReadLine();

    switch (command)
    {
        case "PING":
            Console.WriteLine(await client.Ping());
            break;
        case "GET":
            Console.WriteLine("The key you want to look for: ");
            var key = Console.ReadLine()?.Trim();

            if (key is null || key == string.Empty)
            {
                Console.WriteLine("Error - Empty key");
                break;
            }

            Console.WriteLine(await client.GET(key));
            break;
        case "SET":
            Console.WriteLine("The key you want to look for: ");
            key = Console.ReadLine()?.Trim();
            if (key is null || key == string.Empty)
            {
                Console.WriteLine("Error - Empty key");
                break;
            }

            Console.WriteLine("The value for the key");
            var val = Console.ReadLine()?.Trim();
            if (val is null || val == string.Empty)
            {
                Console.WriteLine("Error - Empty val");
                break;
            }

            Console.WriteLine(await client.SET(key, val));
            break;
        case "quit":
            return;
        default:
            break;
    }
}
