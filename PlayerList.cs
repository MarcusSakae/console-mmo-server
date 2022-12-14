using System.Net.Sockets;
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

    // Note that we return a PlayerManager for the player upon adding it to the list.
    public static PlayerManager Add(TcpClient tcpClient)
    {
        Console.WriteLine("New player connected!");
        Player player = new(tcpClient);
        players.Add(player);
        return new PlayerManager(player);
    }

    public static void ForEach(Player currentClient, Action<Player> callback)
    {
        players.ForEach(client =>
        {
            if (client != currentClient && client.IsConnected)
                callback(client);
        });
    }

    public static void Output()
    {
        Console.WriteLine("Players:");
        players.ForEach(player =>
        {
            Console.WriteLine($"Player: {player.X},{player.Y}");
        });
    }
}