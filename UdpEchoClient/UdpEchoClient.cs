using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // Sunucunun çalıştığı IP adresi ve port
        string serverIp = "127.0.0.1"; // Kendi makinemiz (localhost)
        int serverPort = 9090;         // Sunucu ile aynı port

        UdpClient client = null;

        try
        {
            // 1. UdpClient oluştur. Belirli bir porta bağlamak zorunda değiliz,
            //    OS geçici bir port atayacaktır.
            client = new UdpClient();
            Console.WriteLine("UDP İstemci başlatıldı.");

            // Sunucunun adresini IPEndPoint olarak tanımla
            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);

            Console.Write("Gönderilecek mesajı girin (çıkmak için 'çıkış' yazın): ");
            string messageToSend = Console.ReadLine();

            while (messageToSend?.ToLower() != "çıkış")
            {
                // 2. Mesajı byte dizisine çevir
                byte[] dataToSend = Encoding.UTF8.GetBytes(messageToSend);

                // 3. Mesajı sunucuya gönder (hedef adresi belirterek)
                await client.SendAsync(dataToSend, dataToSend.Length, serverEndPoint);
                Console.WriteLine($"Gönderildi [{serverEndPoint}]: {messageToSend}");

                // 4. Sunucudan yanıt bekle (echo)
                Console.WriteLine("Sunucudan yanıt bekleniyor...");
                // ReceiveAsync, herhangi bir yerden gelen ilk UDP paketini alır.
                // Yanıtın gerçekten bizim sunucudan geldiğini doğrulamak için
                // result.RemoteEndPoint kontrol edilebilir, ancak basitlik için atlıyoruz.
                UdpReceiveResult result = await client.ReceiveAsync();

                // Gelen byte'ları string'e çevir ve ekrana yazdır
                byte[] receivedBytes = result.Buffer;
                IPEndPoint remoteEndPoint = result.RemoteEndPoint; // Yanıtın kimden geldiği
                string receivedEcho = Encoding.UTF8.GetString(receivedBytes);
                Console.WriteLine($"Alındı [{remoteEndPoint}]: {receivedEcho}");


                // Tekrar mesaj iste
                Console.Write("\nGönderilecek mesajı girin (çıkmak için 'çıkış' yazın): ");
                messageToSend = Console.ReadLine();
            }
        }
        catch (SocketException ex)
        {
            Console.WriteLine($"Socket Hatası: {ex.Message}");
            Console.WriteLine($"Sunucunun ({serverIp}:{serverPort}) çalıştığından emin olun.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Genel Hata: {ex.Message}");
        }
        finally
        {
            // 5. UdpClient'ı kapat
            client?.Close();
            Console.WriteLine("İstemci kapatıldı.");
        }

        Console.WriteLine("\nProgram sonlandı. Çıkmak için bir tuşa basın...");
        Console.ReadKey();
    }
}