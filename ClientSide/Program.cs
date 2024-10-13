using System.Net.Sockets;
using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

var ip = IPAddress.Parse("192.168.0.248");
var port = 27001;
var endPoint = new IPEndPoint(ip, port);
var client = new TcpClient();
Console.WriteLine("Client Started");

try
{
    client.Connect(endPoint);
    var stream = client.GetStream();

    if (client.Connected)
    {
        while (true)
        {
            Bitmap memoryImage = new Bitmap(1920, 1080);
            using (Graphics memoryGraphics = Graphics.FromImage(memoryImage))
            {
                memoryGraphics.CopyFromScreen(0, 0, 0, 0, memoryImage.Size);
            }

            using (var ms = new MemoryStream())
            {
                memoryImage.Save(ms, ImageFormat.Png);
                byte[] imageBytes = ms.ToArray();

                byte[] lengthBytes = BitConverter.GetBytes(imageBytes.Length);
                stream.Write(lengthBytes, 0, lengthBytes.Length);

                
                stream.Write(imageBytes, 0, imageBytes.Length);

                Console.WriteLine("ScreenShot Sent To Server.");
            }

            Thread.Sleep(10000); 
        }
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
finally
{
    client.Close();
    Console.WriteLine("Connection Closed");
}
