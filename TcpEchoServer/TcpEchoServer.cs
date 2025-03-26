using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class TcpEchoServer
{
    static async Task Main(string[] args)
    {
        // Sunucunun dinleyeceği IP ve Port
        IPAddress ipAddress = IPAddress.Any; // Gelen tüm bağlantıları kabul et
        int port = 8080; // Örnek bir port numarası

        TcpListener listener = null;

        try
        {
            // 1. Dinleyici Oluştur ve Başlat
            listener = new TcpListener(ipAddress, port);
            listener.Start(); 
            Console.WriteLine($"Sunucu başlatıldı. Dinleniyor: {ipAddress}:{port}");

            while (true) // Sürekli yeni bağlantıları dinle
            {
                Console.WriteLine("İstemci bağlantısı bekleniyor...");

                // 2. Bir istemci bağlantısını asenkron olarak kabul et
                using (TcpClient client = await listener.AcceptTcpClientAsync())
                {
                    Console.WriteLine("İstemci bağlandı!");

                    // 3. Bağlantı üzerinden veri okuma/yazma için NetworkStream al
                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] buffer = new byte[1024]; // Veri okumak için tampon bellek
                        int bytesRead;

                        // 4. İstemciden veri oku (bağlantı kapanana kadar)
                        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                        {
                            // Gelen byte'ları string'e çevir
                            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                            Console.WriteLine($"Alınan: {receivedMessage}");

                            // 5. Aynı mesajı (byte olarak) istemciye geri gönder (echo)
                            byte[] echoBytes = Encoding.UTF8.GetBytes("Echo: " + receivedMessage);
                            await stream.WriteAsync(echoBytes, 0, echoBytes.Length);
                            Console.WriteLine($"Gönderilen: Echo: {receivedMessage}");
                        }
                    } // NetworkStream using bloğu sonu (stream kapanır)
                } // TcpClient using bloğu sonu (client kapanır)
                Console.WriteLine("İstemci bağlantısı kapandı.");
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
            // 6. Sunucuyu durdur (program kapanırken)
            listener?.Stop();
            Console.WriteLine("Sunucu durduruldu.");
        }

    }
}