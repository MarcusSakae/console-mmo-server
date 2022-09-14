using System.Net;
using System.Net.Sockets;
class ServerTCP
{
    private static int port = 11122;
    private static TcpListener socket = new TcpListener(IPAddress.Any, port);
    private static List<Player> players = new List<Player>();

    ///
    ///
    ///
    public static void AcceptPlayers()
    {
        socket = new TcpListener(IPAddress.Any, port);
        WaitHandle[] waitHandles = new WaitHandle[2];
        socket.Start();
        Console.WriteLine("Server started on port " + port);

        while (true)
        {

            var result = socket.BeginAcceptTcpClient(null, null);
            waitHandles[0] = new AutoResetEvent(false);
            waitHandles[1] = result.AsyncWaitHandle;
            int w = WaitHandle.WaitAny(waitHandles);

            // Wait for next client to connect or StopEvent

            if (w == 0)  // StopEvent was set (from outside), terminate loop
                break;
            if (w == 1)
            {
                TcpClient tcp = socket.EndAcceptTcpClient(result);

                // client is connected, spawn thread for it and continue to wait for others
                var t = new Thread(ServeClient);
                t.IsBackground = true;
                t.Start(tcp);
            }

        }
        socket.Stop();

        Console.WriteLine("Listener stopped");
    }

    ///
    ///
    ///
    public static void ReceiveData()
    {
        foreach (Player player in players)
        {
            player.ReceiveData();
        }
    }

    ///
    /// We use this to create a separate thread for each client (player)
    ///
    private static void ServeClient(object? obj)
    {
        if (obj == null)
        {
            Console.WriteLine("ServeClient: obj is null");
            return;
        }
        TcpClient tcp = (TcpClient)obj;
        Player player = new Player(tcp);
        players.Add(player);
        Console.WriteLine("Player " + player.id + " connected!");
    }

    ///
    ///
    ///
    private static void ClientAccepted(IAsyncResult _result)
    {
        Console.WriteLine("new player accepted!");
        // Setup the new client
        TcpClient tcp = socket.EndAcceptTcpClient(_result);
        Player player = new Player(tcp);

        // Add to list of clients
        players.Add(player);
    }

    ///
    ///
    ///
    public static void CheckClients()
    {
        players.RemoveAll(p => p.disconnected);

        foreach (Player p in players)
        {
            p.Ping();
        }
    }
}
