using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Program
{
    static void Main(string[] args)
    {
        TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 11122);
        Console.WriteLine("Listening...");
        listener.Start();
        _ = acceptClients(server);

        while (true)
        {
            // get new clients
            TcpClient client = listener.AcceptTcpClientAsync();
            NetworkStream stream = client.GetStream();


            while (stream.CanRead)
            {

                byte[] rxBuf = new byte[client.ReceiveBufferSize];

                int bytesRead = stream.Read(rxBuf, 0, client.ReceiveBufferSize);
                string dataReceived = Encoding.ASCII.GetString(rxBuf, 0, bytesRead);
                Console.WriteLine("Received : " + dataReceived);
                Console.WriteLine("Sending back : " + dataReceived);

                // sending data
                byte[] txBuf = new byte[client.SendBufferSize];
                txBuf = Encoding.ASCII.GetBytes("Hello World!");
                stream.Write(txBuf, 0, txBuf.Length);
            }
            // client.Close();
        }
        listener.Stop();
        Console.WriteLine("Server stopped...");
        Console.ReadLine();
    }

    private async Task acceptClients(TcpListener server)
    {
        do
        {
            var tcpClient = await server.AcceptTcpClientAsync();
            PlayerManager playerManager = PlayerManager.Add(tcpClient);
            _ = playerManager.StartReceiveMessageAsync();

        } while (true);

    }

}

///
///
///
public class PlayerManager
{
    private readonly Player _player;
    public PlayerManager(Player player)
    {
        _player = player;
    }

    //
    public async Task StartReceiveMessageAsync()
    {
        string lastReceviedMessageTime = string.Empty;
        do
        {
            var message = await _player.StreamReader.ReadLineAsync();
            string currentTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            Console.WriteLine(message);

        } while (_player.NetworkStream.CanRead && _player.IsConnected);
    }
}

///
///
///
public static class PlayerList
{
    private static readonly List<Player> players;
    static PlayerList()
    {
        players = new();
    }
    
    public static PlayerManager Add(TcpClient tcpClient)
    {
        var client = new Player(tcpClient);
        players.Add(client);
        return new PlayerManager(client);
    }

    public static void ForEach(Player currentClient, Action<Player> callback)
    {
        players.ForEach(client =>
        {
            if (client != currentClient && client.IsConnected)
                callback(client);
        });
    }
}