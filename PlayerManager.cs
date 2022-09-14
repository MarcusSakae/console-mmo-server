
using System.Text;
///
///
///
public class PlayerManager
{
    private readonly Player player;
    public PlayerManager(Player player)
    {
        this.player = player;
    }

    //
    public void StartReceiveMessage()
    {
        string lastReceviedMessageTime = string.Empty;
        while (player.NetworkStream.CanRead && player.IsConnected)
        {

            byte[] rxBuf = new byte[player.TcpClient.ReceiveBufferSize];
            try
            {
                int bytesRead = player.NetworkStream.Read(rxBuf, 0, player.TcpClient.ReceiveBufferSize);
                string message = Encoding.ASCII.GetString(rxBuf, 0, bytesRead);
                Console.WriteLine(message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                player.Disconnect();
            }

        }
    }
}




//         // sending data
//         byte[] txBuf = new byte[client.SendBufferSize];
//         txBuf = Encoding.ASCII.GetBytes("Hello World!");
//         stream.Write(txBuf, 0, txBuf.Length);
