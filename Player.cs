using System.Net.Sockets;

public class Player
{
    public bool IsConnected { get; set; } = true;
    public bool IsMessageLimitExpired { get; set; }
    public TcpClient TcpClient { get; private set; }
    public StreamWriter StreamWriter { get; private set; }
    public StreamReader StreamReader { get; private set; }
    public NetworkStream NetworkStream { get; private set; }
    public int X { get; set; }
    public int Y { get; set; }

    public Player(TcpClient tcpClient)
    {
        // Random x and y position (-2 for borders)
        X = new Random().Next(1, Constants.FIELD_SIZE_X - 2);
        Y = new Random().Next(1, Constants.FIELD_SIZE_Y - 2);
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

