
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
        if (player.NetworkStream.CanRead && player.IsConnected)
        {

            byte[] rxBuf = new byte[player.TcpClient.ReceiveBufferSize];
            try
            {
                int bytesRead = player.NetworkStream.Read(rxBuf, 0, player.TcpClient.ReceiveBufferSize);
                string message = Encoding.ASCII.GetString(rxBuf, 0, bytesRead);
                if (message.Equals(TcpMessage.GET_POSITION))
                {
                }
                Console.WriteLine(message);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                player.Disconnect();
            }

        }
    }

    public void SendPosition(string message)
    {
        byte[] txBuf = new byte[player.TcpClient.SendBufferSize];
        txBuf = Encoding.ASCII.GetBytes($"POS:{player.X},{player.Y}");
        player.NetworkStream.Write(txBuf, 0, txBuf.Length);
    }
}




//         // sending data

