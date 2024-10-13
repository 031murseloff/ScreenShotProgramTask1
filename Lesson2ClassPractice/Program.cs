using System.IO;
using System.Net;
using System.Net.Sockets;

var ip = IPAddress.Parse("192.168.0.248");
var port = 27001;
var endPoint = new IPEndPoint(ip, port);
var listener = new TcpListener(endPoint);
listener.Start();

Console.WriteLine($"Server  begin in {ip}:{port}");
try
{
    while (true)
    {
        var client = listener.AcceptTcpClient();
        var remoteEndpoint = client.Client.RemoteEndPoint as IPEndPoint;
        var stream = client.GetStream();
        Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/{remoteEndpoint!.Address}");

        while (client.Connected)
        {

            byte[] lengthBytes = new byte[4];
            long  bytesRead = stream.Read(lengthBytes, 0, lengthBytes.Length);
            if (bytesRead == 0) break;

            int imageLength = BitConverter.ToInt32(lengthBytes, 0);

            byte[] imageBytes = new byte[imageLength];

            bytesRead = stream.Read(imageBytes, 0, imageLength);
            if (bytesRead == 0)
            {
                break;
            }



            string fileName = $"{Directory.GetCurrentDirectory()}/{remoteEndpoint.Address}/Foto_{DateTime.Now:yyyyMMdd_HHmmss}.png";
            File.WriteAllBytes(fileName, imageBytes);

            Console.WriteLine($"SreenShot  got in {remoteEndpoint.Address}./n it saved as {fileName}");
        }

        stream.Close();
        client.Close();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Exception: {ex.Message}");
}
