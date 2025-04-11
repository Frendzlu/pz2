namespace lab06;

using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Server
{
    public int Port;
    private readonly TcpListener _tcpListener;
    private readonly int _bufferSize;
    private string MyDir { get; set; }
    private Thread? _monitoringThread;

    public Server(int port, int bufferSize)
    {
        Port = port;
        _tcpListener = new TcpListener(IPAddress.Any, port);;
        _bufferSize = bufferSize;
        MyDir = Directory.GetCurrentDirectory();
    }

    private void Ping()
    {
        _tcpListener.Start();
        Console.WriteLine("[SERVER] Serwer nasłuchuje...");

        using var client = _tcpListener.AcceptTcpClient();
        Console.WriteLine("[SERVER] Połączono z klientem.");

        using var stream = client.GetStream();
        using var reader = new BinaryReader(stream);
        using var writer = new BinaryWriter(stream);
        var msg = "";
        
        if (_bufferSize > 0)
        {
            var buffer = new byte[_bufferSize];
            var bytesRead = stream.Read(buffer, 0, buffer.Length);
            msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
        else
        {
            var length = IPAddress.NetworkToHostOrder(reader.ReadInt32());
            var msgBytes = reader.ReadBytes(length);
            msg = Encoding.UTF8.GetString(msgBytes);
        }
        
        Console.WriteLine("[SERVER] Odebrano: " + msg);
        var response = "odczytalem: " + msg;
        var responseBytes = Encoding.UTF8.GetBytes(response);
        if (_bufferSize > 0)
        {
            if (responseBytes.Length > _bufferSize)
                Array.Resize(ref responseBytes, _bufferSize);

            stream.Write(responseBytes, 0, responseBytes.Length);
            _tcpListener.Stop();
            return;
        }
        writer.Write(IPAddress.HostToNetworkOrder(responseBytes.Length));
        writer.Write(responseBytes);
        
    }

    public void Start()
    {
        _monitoringThread = new Thread(Handler);
        _monitoringThread.Start();
    }
    
    public void StartPing()
    {
        _monitoringThread = new Thread(Ping);
        _monitoringThread.Start();
    }
    
    private void Handler()
    {
        _tcpListener.Start();
        Console.WriteLine("[SERVER] Serwer katalogów działa w: " + MyDir);
        
        using var client = _tcpListener.AcceptTcpClient();
        using var stream = client.GetStream();
        using var reader = new BinaryReader(stream);
        using var writer = new BinaryWriter(stream);

        while (true)
        {
            var msgLength = IPAddress.NetworkToHostOrder(reader.ReadInt32());
            var command = Encoding.UTF8.GetString(reader.ReadBytes(msgLength)).Trim();
            Console.WriteLine("[SERVER] Komenda: " + command);

            if (command == "!end")
            {
                SendMsg(writer, "[SERVER] Zamykanie serwera.");
                break;
            }
            
            if (command == "list")
            {
                var files = Directory.GetFileSystemEntries(MyDir);
                SendMsg(writer, string.Join('\n', files));
            }
            else if (command.StartsWith("in "))
            {
                var sub = command[3..].Trim();
                var targetPath = Path.Combine(MyDir, sub);

                if (sub == "..")
                    targetPath = Directory.GetParent(MyDir)?.FullName ?? MyDir;

                if (Directory.Exists(targetPath))
                {
                    MyDir = targetPath;
                    var files = Directory.GetFileSystemEntries(MyDir);
                    SendMsg(writer, string.Join('\n', files));
                }
                else
                {
                    SendMsg(writer, "[SERVER] katalog nie istnieje");
                }
            }
            else
            {
                SendMsg(writer, "[SERVER] nieznane polecenie");
            }
        }
    }

    private static void SendMsg(BinaryWriter writer, string msg)
    {
        var msgBytes = Encoding.UTF8.GetBytes(msg);
        writer.Write(IPAddress.HostToNetworkOrder(msgBytes.Length));
        writer.Write(msgBytes);
    }
    
}

