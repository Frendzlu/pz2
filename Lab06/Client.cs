using System.Net;

namespace lab06;

using System;
using System.Net.Sockets;
using System.Text;

internal class Client
{
    private readonly int _bufferSize;
    private readonly int _port;
    private Thread? _thread;
    
    public Client(int port, int bufferSize)
    {
        _port = port;
        _bufferSize = bufferSize;
    }

    public void StartPing()
    {
        _thread = new Thread(Ping);
        _thread.Start();
    }

    public void Start()
    {
        _thread = new Thread(DirectoryTest);
        _thread.Start();
    }

    private void Ping()
    {
        using var client = new TcpClient("127.0.0.1", _port);
        using var stream = client.GetStream();
        
        Console.WriteLine("[CLIENT] Wpisz wiadomość: ");
        var message = Console.ReadLine();
        
        if (message?.Length > _bufferSize && _bufferSize > 0)
            message = message[.._bufferSize];

        if (message != null)
        {
            var msgBytes = Encoding.UTF8.GetBytes(message);
        
            if (_bufferSize <= 0)
            {
                using var reader = new BinaryReader(stream);
                using var writer = new BinaryWriter(stream);
            
                writer.Write(IPAddress.HostToNetworkOrder(msgBytes.Length));
                writer.Write(msgBytes);
            
                var len = IPAddress.NetworkToHostOrder(reader.ReadInt32());
                var responseBytes = reader.ReadBytes(len);
                var response = Encoding.UTF8.GetString(responseBytes);

                Console.WriteLine("[CLIENT] Serwer: " + response);

                return;
            }
        
            stream.Write(msgBytes, 0, msgBytes.Length);
        }

        var buffer = new byte[_bufferSize];
        var bytesRead = stream.Read(buffer, 0, buffer.Length);
        Console.WriteLine("[CLIENT] Serwer: " + Encoding.UTF8.GetString(buffer, 0, bytesRead));
    }

    private void DirectoryTest()
    {
        using var client = new TcpClient("127.0.0.1", _port);
        using var stream = client.GetStream();
        using var reader = new BinaryReader(stream);
        using var writer = new BinaryWriter(stream);

        while (true)
        {
            Console.WriteLine("[CLIENT] Polecenie: ");
            var command = Console.ReadLine();

            if (command == null)
            {
                continue;
            }

            var msgBytes = Encoding.UTF8.GetBytes(command);
            writer.Write(IPAddress.HostToNetworkOrder(msgBytes.Length));
            writer.Write(msgBytes);

            var len = IPAddress.NetworkToHostOrder(reader.ReadInt32());
            var response = Encoding.UTF8.GetString(reader.ReadBytes(len));
            Console.WriteLine("[CLIENT] Serwer:\n" + response);

            if (command == "!end")
                break;
        }
    }
}
