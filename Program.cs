using System.Net;
using System.Net.Sockets;
using System.Text;

internal class Program
{
    static async Task Main(string[] args)
    {
        TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 11122);
        listener.Start();
        Console.WriteLine("Listening...");
        Thread updateThread = new Thread(new ThreadStart(Update));
        updateThread.Start();
        await acceptClients(listener);
        Console.WriteLine("Server stopped...");
        Console.ReadLine();
    }

    static private void Update()
    {
        while (true)
        {
            PlayerList.Output();
            Thread.Sleep(1000);
        }
    }

    static private async Task acceptClients(TcpListener server)
    {
        do
        {
            var tcpClient = await server.AcceptTcpClientAsync();
            PlayerManager playerManager = PlayerList.Add(tcpClient);
            playerManager.StartReceiveMessage();
        } while (true);

    }

}
