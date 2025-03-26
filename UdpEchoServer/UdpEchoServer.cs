using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Sunucunun dinleyeceği port
        int port = 9090; // UPD için bir port kullanalım
        UdpClient listener = null;

        try
        {
            // 1. Belirtilen portu dinlemek üzere UdpClient oluştur
            //    IPEndPoint, hangi IP ve portun dinleneceğini belirtir.
            //    IPAddress.Any: Herhangi bir ağ arayüzünden gelen paketleri kabul et.
            IPEndPoint listenEndPoint = new IPEndPoint(IPAddress.Any, port);
            listener = new UdpClient(listenEndPoint);
            Console.WriteLine($"UDP Sunucu başlatıldı. Dinleniyor: {listenEndPoint}");

            while (true) // Sürekli yeni datagramları bekle
            {
                Console.WriteLine("\nİstemciden datagram bekleniyor...");

                // 2. Bir datagram'ı asenkron olarak almayı bekle.
                //    ReceiveAsync, gelen veriyi ve gönderenin adresini (IPEndPoint) döndürür.
                UdpReceiveResult result = await listener.ReceiveAsync();

                // Gönderenin adresini ve portunu al
                IPEndPoint remoteEndPoint = result.RemoteEndPoint;
                // Gelen byte verisini al
                byte[] receivedBytes = result.Buffer;

                // Gelen byte'ları string'e çevir
                string receivedMessage = Encoding.UTF8.GetString(receivedBytes);

                // Yanıt
                Console.WriteLine($"Alındı [{remoteEndPoint}]: {receivedMessage}");
                Console.Write($"[{remoteEndPoint}] Yanıtınızı girin: ");
                string responseMessage = Console.ReadLine() ?? ""; // Null ise boş string
                Console.WriteLine($"Gönderiliyor [{remoteEndPoint}]: {responseMessage}");

                byte[] responseBytes = Encoding.UTF8.GetBytes(responseMessage);
                await listener.SendAsync(responseBytes, responseBytes.Length, remoteEndPoint);
                
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Socket Hatası: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Genel Hata: {ex.Message}");
        }
        finally
        {
            // 4. UdpClient'ı kapat (kaynakları serbest bırak)
            listener?.Close(); // Close(), Dispose() metodunu çağırır.
            Console.WriteLine("Sunucu durduruldu.");
        }
    }
}