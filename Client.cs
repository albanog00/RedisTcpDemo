using System.Net.Sockets;
using System.Text;

namespace RedisClient;

public class Client
{
    private string Hostname = "localhost";
    private int Port = 6379;

    private TcpClient _client;
    private NetworkStream _stream => _client.GetStream();

    public Client(string? hostname, int? port)
    {
        Hostname = hostname ?? Hostname;
        Port = port ?? Port;

        _client = new();
        Connect();
    }

    public Client()
        : this(null, null) { }

    private void Connect() => ConnectAsync().GetAwaiter().GetResult();

    private async Task ConnectAsync() => await _client.ConnectAsync(Hostname, Port);

    public async Task<string> Ping()
    {
        string command = String.Format("PING\r\n");
        return await SendCommand(command);
    }

    public async Task<string> GET(string key)
    {
        string command = String.Format("GET {0}\r\n", key);
        return await SendCommand(command);
    }

    public async Task<string> SET(string key, string value)
    {
        string command = String.Format("SET {0} {1}\r\n", key, value);
        return await SendCommand(command);
    }

    private async Task<string> SendCommand(string command)
    {
        byte[] buffer = Encoding.UTF8.GetBytes(command);
        await _client.Client.SendAsync(buffer);

        await WaitForResponse();

        string message = string.Empty;
        int messageLength = _client.Available;
        if (messageLength > 0)
        {
            buffer = new byte[messageLength];
            _stream.Read(buffer, 0, messageLength);

            message = Encoding.UTF8.GetString(buffer, 0, messageLength);
        }
        return message;
    }

    private async Task WaitForResponse()
    {
        await Task.Run(() =>
        {
            while (_client.Available == 0)
            {
                Thread.Sleep(1);
            }
        });
    }
}
