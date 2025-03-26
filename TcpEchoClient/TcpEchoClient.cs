using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

// Client Side
class Program
{
    static async Task Main(string[] args)
    {
        // Sunucunun çalıştığı IP adresi ve port
        string serverIp = "127.0.0.1"; // localhost
        int port = 8080;               // Sunucu ile aynı port

        try
        {
            // 1. TcpClient oluştur ve sunucuya bağlanmayı dene
            using (TcpClient client = new TcpClient())
            {
                Console.WriteLine($"Sunucuya bağlanılıyor: {serverIp}:{port}");
                await client.ConnectAsync(serverIp, port); // Asenkron bağlan
                Console.WriteLine("Sunucuya bağlandı!");

                // 2. Bağlantı üzerinden veri okuma/yazma için NetworkStream al
                using (NetworkStream stream = client.GetStream())
                {
                    Console.Write("Gönderilecek mesajı girin (çıkmak için 'çıkış' yazın): ");
                    string messageToSend = Console.ReadLine();

                    while (messageToSend?.ToLower() != "çıkış")
                    {
                        // 3. Mesajı byte dizisine çevir
                        byte[] dataToSend = Encoding.UTF8.GetBytes(messageToSend);

                        // 4. Mesajı sunucuya gönder
                        await stream.WriteAsync(dataToSend, 0, dataToSend.Length);
                        Console.WriteLine("Mesaj gönderildi.");

                        // 5. Sunucudan gelen yanıtı (echo) oku
                        byte[] buffer = new byte[1024];
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                        // Gelen byte'ları string'e çevir ve ekrana yazdır
                        string receivedEcho = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Sunucudan Gelen Echo: {receivedEcho}");

                        // Tekrar mesaj iste
                        Console.Write("\nGönderilecek mesajı girin (çıkmak için 'çıkış' yazın): ");
                        messageToSend = Console.ReadLine();
                    }
                } // NetworkStream using bloğu sonu (stream kapanır)
            } // TcpClient using bloğu sonu (client kapanır)
            Console.WriteLine("Sunucu bağlantısı kapandı.");
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Socket Hatası: {ex.Message}");
            Console.WriteLine($"Sunucunun çalıştığından ve portun ({port}) doğru olduğundan emin olun.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Genel Hata: {ex.Message}");
        }
    }
}