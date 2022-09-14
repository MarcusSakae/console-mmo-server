using System.Net.Sockets;

public class Player
{
    public bool IsConnected { get; set; } = true;
    public bool IsMessageLimitExpired { get; set; }
    public TcpClient TcpClient { get; private set; }
    public StreamWriter StreamWriter { get; private set; }
    public StreamReader StreamReader { get; private set; }
    public NetworkStream NetworkStream { get; private set; }

    public Player(TcpClient tcpClient)
    {
        TcpClient = tcpClient;
        NetworkStream = tcpClient.GetStream();
        StreamWriter = new StreamWriter(NetworkStream);
        StreamReader = new StreamReader(NetworkStream);
    }

    public void Disconnect()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Player disconnected!");
        Console.ResetColor();
        IsConnected = false;
        TcpClient.Close();
        StreamWriter.Close();
        StreamReader.Close();
        NetworkStream.Close();
    }
}

